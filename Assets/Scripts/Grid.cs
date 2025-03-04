using UnityEngine;

/// <summary>
/// Статический менеджер для хранения состояния клеток игрового поля.
/// </summary>
public static class Grid
{
    public static CellState[,,] grid; // Трехмерный массив клеток
    private static int sizeX, sizeY, sizeZ; // Размеры поля

    /// <summary>
    /// Инициализирует игровое поле с заданными размерами.
    /// </summary>
    public static void InitializeGrid(Vector3Int size)
    {
        sizeX = size.x;
        sizeY = size.y;
        sizeZ = size.z;
        grid = new CellState[sizeX, sizeY, sizeZ];

        // Заполняем поле свободными клетками
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                for (int z = 0; z < sizeZ; z++)
                {
                    grid[x, y, z] = CellState.Free;
                }
            }
        }
    }

    /// <summary>
    /// Устанавливает состояние клетки в сетке.
    /// </summary>
    public static void SetCellState(Vector3Int position, CellState state)
    {
        if (IsInsideGrid(position))
        {
            grid[position.x, position.y, position.z] = state;
        }
        else
        {
            Debug.LogError($"Переданные координаты [x:{position.x}, y:{position.y}, z:{position.z}] не попали в игровую область");
        }
    }

    /// <summary>
    /// Получает состояние клетки в сетке.
    /// </summary>
    public static CellState GetCellState(Vector3Int position)
    {
        return IsInsideGrid(position) ? grid[position.x, position.y, position.z] : CellState.Free;
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


}
