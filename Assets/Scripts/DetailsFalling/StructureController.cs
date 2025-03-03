using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��������� ���������� �������� ������-������ � � ������.
/// </summary>
public class StructureController : MonoBehaviour
{
    [SerializeField] private List<BlockController> blocks;

    [Tooltip("������ �� ��� ������ ��� ������ �����.")]
    public bool isFalling = true;

    public bool hasGroundContact;

    private void Start()
    {
        // ������� ��� ����� ������ ���� ������
        blocks = new List<BlockController>(GetComponentsInChildren<BlockController>());
    }

    private void LateUpdate()
    {
        // ���� ����������:
        hasGroundContact = HasGroundContact();
        // ��������
        if (isFalling)
        {
            Fall();
        }
    }

    /// <summary>
    /// ���������� ��� ������ ����.
    /// </summary>
    private void Fall()
    {
        transform.Translate(Vector3.down * Time.deltaTime, Space.World);

        // ���� ���� �� ���� ���� ������ �����������, ������������� ������
        if (HasGroundContact())
        {
            isFalling = false;
            FillGrid();
        }
        else // ������ ����� ����� ���������� ������� ������.
        {
            isFalling = true;
        }
    }

    /// <summary>
    /// ���������, �������� �� ���� �� ���� ���� ����� ��� ������� ������.
    /// </summary>
    private bool HasGroundContact()
    {
        foreach (BlockController block in blocks)
        {
            Vector3Int roundedPos = block.GetRoundedPosition();

            // ���������, ���� �� ���� ��� ���� ������ � ������ ��� �� ������
            if (!IsBlockCoveredByOtherBlock(block, roundedPos))
            {
                if (block.IsTouchingGround(roundedPos))
                {
                    return true;
                }
            }
        }
        return false;
    }
    /// <summary>
    /// ���������, ���� ��� ������� ������ ���� ������ ���� ��� �� ������.
    /// </summary>
    /// <param name="block">������� ����, ��� �������� ����� ��������� ������� ������ ��� ���.</param>
    /// <param name="position">������� �������� �����.</param>
    /// <returns>True, ���� ��� ������ ���� ������ ���� ���� �� ������, ����� False.</returns>
    private bool IsBlockCoveredByOtherBlock(BlockController block, Vector3Int position)
    {
        foreach (BlockController otherBlock in blocks)
        {
            // ���� ��� ������ ����, � ��� ������� ����� ��� ������� ������
            if (otherBlock != block)
            {
                Vector3Int otherBlockPos = otherBlock.GetRoundedPosition();
                // ���������, ���� �� ���� ����� ��� ������� ������
                if (otherBlockPos.x == position.x && otherBlockPos.z == position.z && otherBlockPos.y == position.y - 1)
                {
                    return true; // ������ ���� ����� ��� ����
                }
            }
        }
        return false; // ���� �� ����� ����� ��� ���, ���������� false
    }

    /// <summary>
    /// ��������� ������ �� ��������� �����, ������ ������������ ������.
    /// ���� ����� ����� ������� ������� ����� ���������.
    /// </summary>
    [ContextMenu("��������� ������")]
    private void Collapse()
    {
        foreach (BlockController block in blocks)
        {
            // ������� ��� ��� ������ ���������:
            Vector3Int takenCellPosition = block.GetAlignedPosition();
            if (Grid.CanFall(takenCellPosition))
                Grid.SetCellState(takenCellPosition, CellState.Free);

            // ��������� ������� �������:
            block.isFalling = true;
            block.transform.parent = transform.parent; // ����������� ����� �� ������������� �������
        }

        Destroy(gameObject); // ������� ������, �� ������� ����� StructureController
    }

    /// <summary>
    /// ������� Grid, ����� ������ ������.
    /// </summary>
    private void FillGrid()
    {
        foreach (BlockController block in blocks)
        {
            // ������� ��� ��� ������ ������:
            Vector3Int takenCellPosition = block.GetAlignedPosition();
            Grid.SetCellState(takenCellPosition, CellState.Taken);

            // Debug.Log($"Cell -> Taken [{takenCellPosition.x}][{takenCellPosition.y}][{takenCellPosition.z}]");
        }
    }
}
