using UnityEngine;
using System.Collections.Generic;


[AddComponentMenu("Custom/Details Spawner (������ ������� ��������� �������� �� �����.)")]
public class DetailsSpawner : MonoBehaviour
{
    [Tooltip("�������� ������� �������")]
    [SerializeField] Transform centerPoint;

    [Tooltip("������ ����� ������ �������")]
    [SerializeField] GameObject spawnPoint;

    [Tooltip("������������ ������ ��� �������.")]
    [SerializeField] Transform detailsSceneContainer;

    [Tooltip("������� �������")]
    [SerializeField] List<GameObject> detailPrefabs;

    private void Start()
    {
        centerPoint.GetComponent<CenterFinder>()?.FindCenter();
        
        // ����� ������ ������ ���� ���� �� gridHeight:
        Vector3 spawnPos = centerPoint.position + Vector3.up * GameManager.gridHeight + Vector3.down;
        spawnPoint.transform.position = spawnPos;
    }

    /// <summary>
    /// ������� ����� ������ �� ������� ����.
    /// </summary>
    /// <returns></returns>
    public GameObject SpawnDetail()
    {
        int randId = Random.Range(0, detailPrefabs.Count);
        GameObject currentDetail = Instantiate(detailPrefabs[randId], spawnPoint.transform.position, Quaternion.identity, detailsSceneContainer);
        return currentDetail;
    }
}
