using UnityEngine;

/// <summary>
/// ����������� �������� ��� �������� ��������� ������ �������� ����.
/// </summary>
public static class Grid
{
    public static CellState[,,] grid; // ���������� ������ ������
    private static int sizeX, sizeY, sizeZ; // ������� ����

    /// <summary>
    /// �������������� ������� ���� � ��������� ���������.
    /// </summary>
    public static void InitializeGrid(Vector3Int size)
    {
        sizeX = size.x;
        sizeY = size.y;
        sizeZ = size.z;
        grid = new CellState[sizeX, sizeY, sizeZ];

        // ��������� ���� ���������� ��������
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
    /// ������������� ��������� ������ � �����.
    /// </summary>
    public static void SetCellState(Vector3Int position, CellState state)
    {
        if (IsInsideGrid(position))
        {
            grid[position.x, position.y, position.z] = state;
        }
        else
        {
            Debug.LogError($"���������� ���������� [x:{position.x}, y:{position.y}, z:{position.z}] �� ������ � ������� �������");
        }
    }

    /// <summary>
    /// �������� ��������� ������ � �����.
    /// </summary>
    public static CellState GetCellState(Vector3Int position)
    {
        return IsInsideGrid(position) ? grid[position.x, position.y, position.z] : CellState.Free;
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
