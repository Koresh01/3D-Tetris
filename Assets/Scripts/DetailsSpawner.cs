using UnityEngine;
using System.Collections.Generic;

[AddComponentMenu("Custom/Details Spawner (������ ������� ��������� �������� �� �����.)")]
public class DetailsSpawner : MonoBehaviour
{
    public static DetailsSpawner Instance { get; private set; }

    [Tooltip("�������� ������� �������")]
    [SerializeField] Transform centerPoint;

    [Tooltip("������ ����� ������ �������")]
    [SerializeField] GameObject spawnPoint;

    [Tooltip("������������ ������ ��� �������.")]
    [SerializeField] Transform detailsSceneContainer;

    [Tooltip("������� �������")]
    [SerializeField] List<GameObject> detailPrefabs;

    private int lastDetailId = -1;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        Random.InitState(System.DateTime.Now.Millisecond); // ������������� ��������� seed

        centerPoint.GetComponent<CenterFinder>()?.FindCenter();

        // ����� ������ ������ ���� ���� �� gridHeight:
        Vector3 spawnPos = centerPoint.position + Vector3.up * GameManager.gridHeight + Vector3.down;
        spawnPoint.transform.position = spawnPos;
    }

    /// <summary>
    /// ������� ����� ������ �� ������� ����.
    /// </summary>
    public void SpawnNextDetail()
    {
        int randId = Random.Range(0, detailPrefabs.Count);

        // ���� ���������� ������ �������, ����� ��������� �� ������
        if (detailPrefabs.Count > 1 && randId == lastDetailId)
        {
            randId = (randId + 1) % detailPrefabs.Count;
        }

        lastDetailId = randId;

        GameObject currentDetail = Instantiate(detailPrefabs[randId], spawnPoint.transform.position, Quaternion.identity, detailsSceneContainer);
        GameManager.currentDetail = currentDetail;
    }
}
