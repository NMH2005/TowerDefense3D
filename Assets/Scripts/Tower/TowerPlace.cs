using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class TowerPlace : MonoBehaviour {
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask blockedLayers;

    private GameObject towerPreview;
    private LineRenderer rangeCircle;
    private float currentRange;
    private bool isPlacing = false;
    private bool isOnGround = false;

    public bool IsPlacing => isPlacing;

    private Color validColor = new Color(0f, 1f, 0f, 0.5f);
    private Color invalidColor = new Color(1f, 0f, 0f, 0.5f);

    private Color rangeValidColor = new Color(0f, 0.7f, 1f, 0.9f);   // xanh dương - dễ nhìn trên nền cỏ
    private Color rangeInvalidColor = new Color(1f, 0.6f, 0f, 0.9f); // cam - dễ nhìn trên nền cỏ

    private Dictionary<Renderer, Color[]> originalColors = new Dictionary<Renderer, Color[]>();

    private TowersData currentData;

    public void StartPlacing(TowersData data)
    {
        // Đang preview đúng loại tower này rồi, bấm lại số đó lần nữa = huỷ preview
        if (isPlacing && currentData == data)
        {
            StopPlacing();
            return;
        }

        if (EconomyManager.Instance != null && !EconomyManager.Instance.CanAfford(data.buyCost))
            return; // không đủ tiền, không cho bắt đầu đặt tower

        if (towerPreview != null)
            Destroy(towerPreview);

        currentData = data;
        isPlacing = true;
        isOnGround = false;
        currentRange = data.levels[0].Range;
        EventManager.RaiseTowerSelectionChanged(data);
        towerPreview = Instantiate(data.prefab);
        TowerManager towerManager = towerPreview.GetComponent<TowerManager>();

        if (data.WeaponPrefab != null)
        {
            GameObject weaponPreview = Instantiate(data.WeaponPrefab, towerPreview.transform);
            foreach (var wb in weaponPreview.GetComponentsInChildren<WeaponBase>())
                wb.enabled = false;
            foreach (var wa in weaponPreview.GetComponentsInChildren<WeaponAttack>())
                wa.enabled = false;

            if (towerManager != null)
                towerManager.RegisterWeapon(weaponPreview);
        }

        // Initialize sẽ gọi ApplyLevel -> tự đặt weapon vào đúng WeaponPlace
        // đang active (base/bottom/middle/build), và làm lại mỗi khi upgrade.
        if (towerManager != null)
            towerManager.Initialize(data, 0);
        originalColors.Clear();
        foreach (var r in towerPreview.GetComponentsInChildren<Renderer>())
        {
            if (r is LineRenderer) continue;
            originalColors[r] = r.materials.Select(m => m.color).ToArray();
        }

        SetupRangeCircle();
    }

    public void StopPlacing()
    {
        if (towerPreview != null)
            Destroy(towerPreview);
        isPlacing = false;
        currentData = null;
        EventManager.RaiseTowerSelectionChanged(null);
    }

    void SetupRangeCircle()
    {
        rangeCircle = new GameObject("RangeCircle").AddComponent<LineRenderer>();
        rangeCircle.transform.SetParent(towerPreview.transform);
        rangeCircle.transform.localPosition = Vector3.zero;

        rangeCircle.loop = true;
        rangeCircle.widthMultiplier = 0.1f;
        rangeCircle.useWorldSpace = false;
        rangeCircle.material = new Material(Shader.Find("Sprites/Default"));

        int segments = 64;
        rangeCircle.positionCount = segments;
        Vector3[] points = new Vector3[segments];
        for (int i = 0; i < segments; i++)
        {
            float angle = 2 * Mathf.PI * i / segments;
            points[i] = new Vector3(Mathf.Cos(angle) * currentRange, 0f, Mathf.Sin(angle) * currentRange);
        }
        rangeCircle.SetPositions(points);
    }

    void SetPreviewColor(bool valid)
    {
        isOnGround = valid;
        Color modelColor = valid ? validColor : invalidColor;
        Color rangeColor = valid ? rangeValidColor : rangeInvalidColor;

        rangeCircle.startColor = rangeColor;
        rangeCircle.endColor = rangeColor;

        foreach (var renderer in towerPreview.GetComponentsInChildren<Renderer>())
            foreach (var mat in renderer.materials)
                mat.color = modelColor;
    }

    void Update()
    {
        if (!isPlacing) return;

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (EventSystem.current.IsPointerOverGameObject())
        {
            SetPreviewColor(false);
        }
        else if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            towerPreview.transform.position = hit.point;

            int towerPlacedLayer = LayerMask.NameToLayer("TowerPlaced");
            Collider[] overlaps = Physics.OverlapSphere(hit.point, 2f, 1 << towerPlacedLayer);
            bool hasTower = overlaps.Length > 0;

            Collider[] blockedOverlaps = Physics.OverlapSphere(hit.point, 2f, blockedLayers);
            bool isBlocked = blockedOverlaps.Length > 0;

            bool isGround = ((1 << hit.collider.gameObject.layer) & groundLayer) != 0;
            SetPreviewColor(isGround && !hasTower && !isBlocked);
        }
        else
        {
            SetPreviewColor(false);
        }

        if (Keyboard.current.eKey.isPressed)
        {
            towerPreview.transform.Rotate(0f, 90f * Time.deltaTime, 0);
        }

        if (Keyboard.current.qKey.isPressed)
        {
            towerPreview.transform.Rotate(0f, -90f * Time.deltaTime, 0);
        }

        if (Mouse.current.leftButton.wasPressedThisFrame && isOnGround)
        {
            if (EconomyManager.Instance != null && !EconomyManager.Instance.TrySpend(currentData.buyCost))
                return; // không đủ tiền (vd tiền bị tiêu chỗ khác trong lúc đang kéo đặt)

            foreach (var r in towerPreview.GetComponentsInChildren<Renderer>())
            {
                if (r is LineRenderer) continue;
                if (originalColors.TryGetValue(r, out Color[] colors))
                    for (int i = 0; i < r.materials.Length; i++)
                        r.materials[i].color = colors[i];
            }
            towerPreview.GetComponentsInChildren<WeaponBase>().ToList().ForEach(wb => wb.enabled = true);
            towerPreview.GetComponentsInChildren<WeaponAttack>().ToList().ForEach(wa => wa.enabled = true);
            SetLayerRecursively(towerPreview, LayerMask.NameToLayer("TowerPlaced"));
            towerPreview = null;
            isPlacing = false;
            currentData = null;
            EventManager.RaiseTowerSelectionChanged(null);
            EventManager.RaiseTowerPlaced();
        }
    }

    private void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
            SetLayerRecursively(child.gameObject, layer);
    }
}