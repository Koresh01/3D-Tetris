using UnityEngine;

/// <summary>
/// Контейнер, хранит GameObject блока и состояние ячейки.
/// </summary>
public class Cell
{
    public CellState State;
    public GameObject gameObject;
}

/// <summary>
/// Статический менеджер для хранения состояния клеток игрового поля.
/// </summary>
public static class Grid
{
    public static Cell[,,] grid; // Трехмерный массив клеток
    private static int sizeX, sizeY, sizeZ; // Размеры поля

    /// <summary>
    /// Инициализирует игровое поле с заданными размерами.
    /// </summary>
    public static void InitializeGrid()
    {
        sizeX = GameManager.gridWidth;
        sizeY = GameManager.gridHeight;
        sizeZ = GameManager.gridWidth;
        grid = new Cell[sizeX, sizeY, sizeZ];

        // Заполняем поле свободными клетками
        for (int x = 0; x < sizeX; x++)
            for (int y = 0; y < sizeY; y++)
                for (int z = 0; z < sizeZ; z++)
                    grid[x, y, z] = new Cell { State = CellState.Free, gameObject = null }; // Без явного new
    }

    /// <summary>
    /// Устанавливает состояние клетки в сетке.
    /// </summary>
    /// <param name="block">Блок который устанавливается в эту ячейку</param>
    public static void SetCellState(Vector3Int position, GameObject block, CellState state)
    {
        if (IsInsideGrid(position))
        {
            grid[position.x, position.y, position.z].State = state;
            if (state == CellState.Filled)
            {
                grid[position.x, position.y, position.z].gameObject = block;
            }
            else if (state == CellState.Free)
            {
                grid[position.x, position.y, position.z].gameObject = null;
            }
        }
        else
        {
            // Debug.LogError($"Переданные координаты [x:{position.x}, y:{position.y}, z:{position.z}] не попали в игровую область");
        }
    }

    /// <summary>
    /// Получает состояние клетки в сетке.
    /// </summary>
    public static CellState GetCellState(Vector3Int position)
    {
        return IsInsideGrid(position) ? grid[position.x, position.y, position.z].State : CellState.Free;
    }

    /// <summary>
    /// Получает кубик.
    /// </summary>
    public static GameObject GetCellGameObject(Vector3Int position)
    {
        return IsInsideGrid(position) ? grid[position.x, position.y, position.z].gameObject : null;
    }

    /// <summary>
    /// Проверяет, находится ли клетка в пределах сетки.
    /// </summary>
    private static bool IsInsideGrid(Vector3Int position)
    {
        return position.x >= 0 && position.x < sizeX &&
               position.y >= 0 && position.y < sizeY &&
               position.z >= 0 && position.z < sizeZ;
    }

    /// <summary>
    /// Проверяет занят ли весь слой.
    /// </summary>
    public static bool IsLayerFilled(int layerInx)
    {
        for (int x = 0 ; x < sizeX; x++)
        {
            for (int z = 0; z < sizeZ; z++)
            {
                CellState cellState = grid[x, layerInx, z].State;
                if (cellState == CellState.Free)
                    return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Метод возвращает true, если под текущей ячейкой пусто.
    /// </summary>
    /// <param name="startPos">Позиция кубика по сетке.</param>
    public static bool CanFall(Vector3Int startPos)
    {
        // Проверяем состояние клетки, расположенной ниже текущей
        Vector3Int belowPos = new Vector3Int(startPos.x, startPos.y - 1, startPos.z);

        // Если клетка пустая, то кубик может падать
        return Grid.GetCellState(belowPos) == CellState.Free;
    }

    /// <summary>
    /// Удаляет слой игрового поля.
    /// </summary>
    public static void DestroyLayer(int layerInx)
    {
        for (int x = 0; x < GameManager.gridWidth; x++)    // grid width не видит
        {
            for (int z = 0; z < GameManager.gridWidth; z++)
            {
                Vector3Int CellPosition = new Vector3Int(x, layerInx, z);
                CellState state = Grid.GetCellState(CellPosition);
                GameObject block = Grid.GetCellGameObject(CellPosition);

                if (state == CellState.Filled)
                {
                    Grid.SetCellState(CellPosition, null, CellState.Free);
                    GameObject.Destroy(block);
                }

                // Отрисовка состояния Grid на данный момент.
                if (CellsVizualizer.Instance != null)
                {
                    CellsVizualizer.Instance.UpdateCellMaterial(CellPosition);
                }
            }
        }
    }

}
