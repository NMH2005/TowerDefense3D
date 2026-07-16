using UnityEngine;

[System.Serializable]
public class CrystalStage {
    [Header("Crystal")]
    public GameObject crystalObject;

    [Header("Nứt - cảnh báo trước khi vỡ")]
    [Range(0f, 1f)]
    [Tooltip("Đổi màu crystal khi máu base <= % này. Phải LỚN HƠN Break At Hp Percent.")]
    public float crackAtHpPercent = 0.6f;

    [Tooltip("Màu crystal đổi sang khi bị nứt (vd đỏ sậm/xám để báo hiệu sắp vỡ). Để trắng/không đổi nếu chỉ muốn dùng đường nứt bên dưới.")]
    public Color crackColor = new Color(0.6f, 0.2f, 0.2f);

    [Tooltip("Số đường nứt vẽ trên MỖI mặt của crystal (tổng số đường = số này x số mặt trên model).")]
    public int crackLinesPerFace = 1;

    [Tooltip("Độ dài mỗi đường nứt (theo local scale của crystal).")]
    public float crackLineLength = 0.3f;

    [Tooltip("Đẩy đường nứt ra khỏi bề mặt bao nhiêu để không bị mesh che (thường để rất nhỏ).")]
    public float crackSurfaceOffset = 0.02f;

    [Header("Vỡ")]
    [Range(0f, 1f)]
    [Tooltip("Crystal biến mất khi máu base <= % này.")]
    public float breakAtHpPercent = 0.4f;

    [Tooltip("Prefab hiệu ứng mảnh vỡ (Particle System...), spawn 1 lần lúc vỡ rồi tự huỷ.")]
    public GameObject shatterEffectPrefab;

    [HideInInspector] public bool isCracked;
    [HideInInspector] public bool isBroken;
    [HideInInspector] public Renderer renderer;
    [HideInInspector] public Color originalColor;
}

/// Gắn lên GameObject cha của "Crystal-Compound" (hoặc chính GameObject đó).
public class BaseCrystal : MonoBehaviour {
    [SerializeField] private CrystalStage[] stages;

    private void Start()
    {
        foreach (var stage in stages)
        {
            if (stage.crystalObject == null) continue;

            stage.renderer = stage.crystalObject.GetComponent<Renderer>();
            if (stage.renderer != null)
                stage.originalColor = stage.renderer.material.color;
        }
    }

    private void OnEnable()
    {
        EventManager.OnLivesChanged += HandleLivesChanged;
    }

    private void OnDisable()
    {
        EventManager.OnLivesChanged -= HandleLivesChanged;
    }

    private void HandleLivesChanged(int currentLives)
    {
        if (PlayerBase.Instance == null || PlayerBase.Instance.MaxLives <= 0) return;

        float percent = (float)currentLives / PlayerBase.Instance.MaxLives;

        foreach (var stage in stages)
        {
            if (stage.crystalObject == null || stage.isBroken) continue;

            if (!stage.isCracked && percent <= stage.crackAtHpPercent)
                Crack(stage);

            if (percent <= stage.breakAtHpPercent)
                Break(stage);
        }
    }

    private void Crack(CrystalStage stage)
    {
        stage.isCracked = true;

        if (stage.renderer != null)
            stage.renderer.material.color = stage.crackColor;

        ProceduralCrack.SpawnOn(stage.crystalObject, stage.crackLinesPerFace, stage.crackLineLength,
            Color.black, stage.crackSurfaceOffset);
    }

    private void Break(CrystalStage stage)
    {
        stage.isBroken = true;

        if (stage.shatterEffectPrefab != null)
        {
            Transform t = stage.crystalObject.transform;
            Instantiate(stage.shatterEffectPrefab, t.position, t.rotation);
        }

        stage.crystalObject.SetActive(false);
    }
}