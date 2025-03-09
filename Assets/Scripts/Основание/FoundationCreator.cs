using UnityEngine;

/// <summary>
/// Создает основание из кубиков.
/// </summary>
class FoundationCreator : MonoBehaviour
{
    [SerializeField] GameObject cubePrefab;

    private void Start()
    {
        int gridWidth = GameManager.gridWidth;
        int gridHeight = GameManager.gridHeight;

        // Создаем кубики:
        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridWidth; z++)
            {
                Vector3Int position = new Vector3Int(x, 0, z);
                GameObject block = Instantiate(cubePrefab, position, Quaternion.identity, transform);
                
                
                Grid.SetCellState(position, block, CellState.Filled);
            }
        }

        // Отрисовывем состояние ячеек:
        if (CellsVizualizer.Instance != null)
        {
            CellsVizualizer.Instance.ReGenerate();
        }
    }
}
