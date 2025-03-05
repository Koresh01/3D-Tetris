using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Скрипт спавнит очередную детальку на сцене.
/// </summary>
[AddComponentMenu("Custom/Details Spawner (Скрипт спавнит очередную детальку на сцене.)")]
public class DetailsSpawner : MonoBehaviour
{
    [Tooltip("Середина игровой области")]
    [SerializeField] Transform centerPoint;

    [Tooltip("Префаб точки спавна деталей")]
    [SerializeField] GameObject spawnPoint;

    [Tooltip("Родительский объект для деталей.")]
    [SerializeField] Transform detailsSceneContainer;

    [Tooltip("Префабы деталей")]
    [SerializeField] List<GameObject> detailPrefabs;

    private void Start()
    {
        centerPoint.GetComponent<CenterFinder>()?.FindCenter();
        // Точка спавна должна быть выше на gridHeight:
        Vector3 spawnPos = centerPoint.position + Vector3.up * GameManager.gridHeight;
        Instantiate(spawnPoint, spawnPos, Quaternion.identity, transform);
    }

    public void SpawnDetail()
    {
        Instantiate(detailPrefabs[0], centerPoint.transform.position, Quaternion.identity, centerPoint);
    }
}
