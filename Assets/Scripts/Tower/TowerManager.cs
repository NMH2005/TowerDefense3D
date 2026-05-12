using UnityEngine;

public class TowerManager : MonoBehaviour
{
    [SerializeField] private GameObject basePrefab;
    [SerializeField] private GameObject bottomPrefab;
    [SerializeField] private GameObject middlePrefab;
    [SerializeField] private GameObject buildPrefab;



    public void ApplyLevel(TowerLevelData levelData)
    {
        basePrefab.SetActive(levelData.enableBase);
        bottomPrefab.SetActive(levelData.enableBottom);
        middlePrefab.SetActive(levelData.enableMiddle);
        buildPrefab.SetActive(levelData.enableBuild);
    }
}
