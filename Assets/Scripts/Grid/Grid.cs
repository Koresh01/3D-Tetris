using UnityEngine;

/// <summary>
/// ���������, ������ GameObject ����� � ��������� ������.
/// </summary>
public class Cell
{
    public CellState State;
    public GameObject gameObject;
}

/// <summary>
/// ����������� �������� ��� �������� ��������� ������ �������� ����.
/// </summary>
public static class Grid
{
    public static Cell[,,] grid; // ���������� ������ ������
    private static int sizeX, sizeY, sizeZ; // ������� ����

    /// <summary>
    /// �������������� ������� ���� � ��������� ���������.
    /// </summary>
    public static void InitializeGrid()
    {
        sizeX = GameManager.gridWidth;
        sizeY = GameManager.gridHeight;
        sizeZ = GameManager.gridWidth;
        grid = new Cell[sizeX, sizeY, sizeZ];

        // ��������� ���� ���������� ��������
        for (int x = 0; x < sizeX; x++)
            for (int y = 0; y < sizeY; y++)
                for (int z = 0; z < sizeZ; z++)
                    grid[x, y, z] = new Cell { State = CellState.Free, gameObject = null }; // ��� ������ new
    }

    /// <summary>
    /// ������������� ��������� ������ � �����.
    /// </summary>
    /// <param name="block">���� ������� ��������������� � ��� ������</param>
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
            // Debug.LogError($"���������� ���������� [x:{position.x}, y:{position.y}, z:{position.z}] �� ������ � ������� �������");
        }
    }

    /// <summary>
    /// �������� ��������� ������ � �����.
    /// </summary>
    public static CellState GetCellState(Vector3Int position)
    {
        return IsInsideGrid(position) ? grid[position.x, position.y, position.z].State : CellState.Free;
    }

    /// <summary>
    /// �������� �����.
    /// </summary>
    public static GameObject GetCellGameObject(Vector3Int position)
    {
        return IsInsideGrid(position) ? grid[position.x, position.y, position.z].gameObject : null;
    }

    /// <summary>
    /// ���������, ��������� �� ������ � �������� �����.
    /// </summary>
    private static bool IsInsideGrid(Vector3Int position)
    {
        return position.x >= 0 && position.x < sizeX &&
               position.y >= 0 && position.y < sizeY &&
               position.z >= 0 && position.z < sizeZ;
    }




    /// <summary>
    /// ����� ���������� true, ���� ��� ������� ������� �����.
    /// </summary>
    /// <param name="startPos">������� ������ �� �����.</param>
    public static bool CanFall(Vector3Int startPos)
    {
        // ��������� ��������� ������, ������������� ���� �������
        Vector3Int belowPos = new Vector3Int(startPos.x, startPos.y - 1, startPos.z);

        // ���� ������ ������, �� ����� ����� ������
        return Grid.GetCellState(belowPos) == CellState.Free;
    }


}
