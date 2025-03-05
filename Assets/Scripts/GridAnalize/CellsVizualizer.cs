using UnityEngine;

/// <summary>
/// Отображает текущее состояние матрицы Grid.
/// </summary>
public class CellsVizualizer : MonoBehaviour
{
    public static CellsVizualizer Instance { get; private set; } // Синглтон

    [Header("Настройки")]
    public GameObject cellPrefab; // Префаб кубика
    public Material defaultMaterial; // Материал для свободных клеток
    public Material occupiedMaterial; // Материал для занятых клеток

    private GameObject[,,] cells; // Массив для хранения объектов клеток
    private Vector3Int size; // Размеры сетки

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        GenerateGrid();
    }

    /// <summary>
    /// Создаёт игровую сетку на основе размеров из Grid.
    /// </summary>
    private void GenerateGrid()
    {
        size = new Vector3Int(GameManager.gridWidth, GameManager.gridHeight, GameManager.gridWidth);
        cells = new GameObject[size.x, size.y, size.z];

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                for (int z = 0; z < size.z; z++)
                {
                    Vector3 position = new Vector3(x, y, z);
                    GameObject cell = Instantiate(cellPrefab, position, Quaternion.identity, transform);
                    cells[x, y, z] = cell;

                    UpdateCellMaterial(new Vector3Int(x, y, z));
                }
            }
        }
    }

    /// <summary>
    /// Пересоздаёт игровую сетку, если размеры изменились. 
    /// В противном случае обновляет материалы существующих клеток.
    /// </summary>
    [ContextMenu("Обновить визуализацию Grid.")]
    public void ReGenerate()
    {
        Vector3Int newSize = new Vector3Int(GameManager.gridWidth, GameManager.gridHeight, GameManager.gridWidth);

        if (cells != null && newSize == size)
        {
            // Размеры не изменились – просто обновляем клетки
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    for (int z = 0; z < size.z; z++)
                    {
                        UpdateCellMaterial(new Vector3Int(x, y, z));
                    }
                }
            }
        }
        else
        {
            // Размеры изменились – пересоздаём сетку
            ClearGrid();
            size = newSize;
            GenerateGrid();
        }
    }

    /// <summary>
    /// Удаляет все кубики из сетки.
    /// </summary>
    private void ClearGrid()
    {
        if (cells == null) return;

        foreach (var cell in cells)
        {
            if (cell != null) Destroy(cell);
        }

        cells = null;
    }

    /// <summary>
    /// Обновляет цвет клетки в зависимости от её состояния.
    /// </summary>
    public void UpdateCellMaterial(Vector3Int position)
    {
        Material mat = Grid.GetCellState(position) == CellState.Free ? defaultMaterial : occupiedMaterial;
        cells[position.x, position.y, position.z].GetComponent<Renderer>().material = mat;
    }

    /// <summary>
    /// Устанавливает состояние клетки и обновляет её визуальное представление.
    /// </summary>
    public void SetCellState(Vector3Int position, CellState state)
    {
        Grid.SetCellState(position, state);
        UpdateCellMaterial(position);
    }
}
