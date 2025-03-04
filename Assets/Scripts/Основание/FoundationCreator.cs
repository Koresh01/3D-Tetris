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
        int gridHeight = gameManager.gridHeight;

        // Создаем кубики:
        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridWidth; z++)
            {
                Vector3 position = new Vector3(x, 0, z);
                Instantiate(cubePrefab, position, Quaternion.identity, transform);
            }
        }

        // Задаём статус клеткам у основания.
        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridWidth; z++)
            {
                Vector3Int positionBottomLayer = new Vector3Int(x, 0, z);
                Grid.SetCellState(positionBottomLayer, CellState.Filled);
            }
        }
    }
}
