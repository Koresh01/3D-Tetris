using UnityEngine;
using System.Collections.Generic;


[AddComponentMenu("Custom/Details Spawner (Скрипт спавнит очередную детальку на сцене.)")]
public class DetailsSpawner : MonoBehaviour
{
    public static DetailsSpawner Instance { get; private set; }

    [Tooltip("Середина игровой области")]
    [SerializeField] Transform centerPoint;

    [Tooltip("Префаб точки спавна деталей")]
    [SerializeField] GameObject spawnPoint;

    [Tooltip("Родительский объект для деталей.")]
    [SerializeField] Transform detailsSceneContainer;

    [Tooltip("Префабы деталей")]
    [SerializeField] List<GameObject> detailPrefabs;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        centerPoint.GetComponent<CenterFinder>()?.FindCenter();
        
        // Точка спавна должна быть выше на gridHeight:
        Vector3 spawnPos = centerPoint.position + Vector3.up * GameManager.gridHeight + Vector3.down;
        spawnPoint.transform.position = spawnPos;
    }

    /// <summary>
    /// Спавнит новую деталь на игровом поле.
    /// </summary>
    /// <returns></returns>
    public void SpawnNextDetail()
    {
        int randId = Random.Range(0, detailPrefabs.Count);
        GameObject currentDetail = Instantiate(detailPrefabs[randId], spawnPoint.transform.position, Quaternion.identity, detailsSceneContainer);
        GameManager.currentDetail = currentDetail;
    }
}
