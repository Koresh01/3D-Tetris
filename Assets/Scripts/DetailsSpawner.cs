using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ������ ������� ��������� �������� �� �����.
/// </summary>
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
        Vector3 spawnPos = centerPoint.position + Vector3.up * GameManager.gridHeight;
        Instantiate(spawnPoint, spawnPos, Quaternion.identity, transform);
    }

    public void SpawnDetail()
    {
        Instantiate(detailPrefabs[0], centerPoint.transform.position, Quaternion.identity, centerPoint);
    }
}
