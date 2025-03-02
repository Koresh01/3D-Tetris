using UnityEngine;

/// <summary>
/// Создает основание из кубиков.
/// </summary>
class FoundationCreator : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject cubePrefab;

    private void Start()
    {
        int gridWidth = gameManager.gridWidth;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridWidth; z++)
            {
                Vector3 position = new Vector3(x, 0, z);
                Instantiate(cubePrefab, position, Quaternion.identity, transform);
            }
        }
    }
}
