using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour {
    [SerializeField] private Image fillImage;

    private EnemyManager enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<EnemyManager>();
    }

    private void LateUpdate()
    {
        if (Camera.main == null)
        {
            Debug.Log("Camera null");
            return;
        }

        if (enemy == null)
        {
            Debug.Log("Enemy null");
            return;
        }

        if (fillImage == null)
        {
            Debug.Log("Fill Image null");
            return;
        }

        transform.forward = Camera.main.transform.forward;
        fillImage.fillAmount = (float)enemy.CurrentHp / enemy.MaxHp;
    }
}