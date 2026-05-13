using UnityEngine;
using UnityEngine.InputSystem;

public class TowerPlace : MonoBehaviour {
    [SerializeField] private LayerMask groundLayer;

    private GameObject towerPreview;
    private LineRenderer rangeCircle;
    private float currentRange;
    private bool isPlacing = false;
    private bool isOnGround = false;

    private Color validColor = new Color(0f, 1f, 0f, 0.5f);
    private Color invalidColor = new Color(1f, 0f, 0f, 0.5f);

    public void StartPlacing(TowersData data)
    {
        if (towerPreview != null)
            Destroy(towerPreview);

        isPlacing = true;
        isOnGround = false;
        currentRange = data.levels[0].Range;
        towerPreview = Instantiate(data.prefab);

        TowerManager towerManager = towerPreview.GetComponent<TowerManager>();
        if (towerManager != null)
            towerManager.ApplyLevel(data.levels[0]);

        if (data.WeaponPrefab != null)
        {
            foreach (var weaponPlace in towerPreview.GetComponentsInChildren<Transform>())
            {
                if (weaponPlace.name == "WeaponPlace" && weaponPlace.gameObject.activeInHierarchy)
                {
                    GameObject weaponPreview = Instantiate(data.WeaponPrefab, weaponPlace);
                    foreach (var wb in weaponPreview.GetComponentsInChildren<WeaponBase>())
                        wb.enabled = false;
                    foreach (var wa in weaponPreview.GetComponentsInChildren<WeaponAttack>())
                        wa.enabled = false;
                    break;
                }
            }
        }
        SetupRangeCircle();
    }

    public void StopPlacing()
    {
        if (towerPreview != null)
            Destroy(towerPreview);
        isPlacing = false;
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
        Color color = valid ? validColor : invalidColor;
        rangeCircle.startColor = color;
        rangeCircle.endColor = color;

        foreach (var renderer in towerPreview.GetComponentsInChildren<Renderer>())
            foreach (var mat in renderer.materials)
                mat.color = color;
    }

    void Update()
    {
        if (!isPlacing) return;

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            towerPreview.transform.position = hit.point;
            SetPreviewColor(true);
            Debug.Log("On ground: " + hit.point);
        }
        else if (Physics.Raycast(ray, out RaycastHit hitAny, Mathf.Infinity))
        {
            towerPreview.transform.position = hitAny.point;
            SetPreviewColor(false);
            Debug.Log("Not on ground, hit: " + hitAny.collider.gameObject.name + " layer: " + hitAny.collider.gameObject.layer);
        }
        else
        {
            Debug.Log("No hit at all");
        }
    }
}