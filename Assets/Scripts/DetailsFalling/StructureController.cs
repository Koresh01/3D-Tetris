using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ��������� ���������� �������� ������-������ � � ������.
/// </summary>
public class StructureController : MonoBehaviour
{
    public List<BlockController> blocks;

    [Tooltip("������ �� ��� ������ ��� ������ �����.")]
    public bool hasGroundContact;

    [Tooltip("������� �������� ����")]
    public static UnityAction OnLayerDeleted;

    private void Start()
    {
        // ������� ��� ����� ������ ���� ������
        blocks = new List<BlockController>(GetComponentsInChildren<BlockController>());
    }

    private void LateUpdate()
    {
        // ���� ���� �� ����� �� ������������� ������:
        if (GameManager.isPaused) return;

        // ���� ����������:
        hasGroundContact = HasGroundContact(out BlockController block);

        // ��������
        Fall();

        // ����������������:
        if (IsEmpty())
            Destroy(gameObject);
    }

    /// <summary>
    /// ������ ������� ���� ������.
    /// </summary>
    private void Fall()
    {
        // ���� ���� �� ���� ���� ������ �����������, ������������� ������
        if (HasGroundContact(out BlockController block))
        {
            // �������� ��� ������:
            FillCells();
            // ��� ����� ������������ ��� ������...

            // ��������� ����� ���� ���� ��� ��������
            int layerInx = block.GetAlignedPosition().y;
            if (Grid.IsLayerFilled(layerInx))
            {
                OnLayerDeleted?.Invoke();
                Grid.DestroyLayer(layerInx);
            }
        }
        else
        {
            FreeCells();
            transform.Translate(Vector3.down * Time.deltaTime, Space.World);
        }
    }

    /// <summary>
    /// ���������, �������� �� ���� �� ���� ���� ����� ��� ������� ������,
    /// ��������� ����� ���� �� ������.
    /// </summary>
    private bool HasGroundContact(out BlockController touchingBlock)
    {
        touchingBlock = null; // �������������� ����������
        foreach (BlockController block in blocks)
        {
            if (!block) continue; // Unity-������������� �������� �� ������������ ������

            Vector3Int blockPos = block.GetAlignedPosition();
            Vector3Int belowPos = blockPos + Vector3Int.down;

            // ���� ���� �������� ����� ��� ������� ������
            if (Grid.GetCellState(belowPos) == CellState.Filled)
            {
                // ���������, �� �������� �� ��� ������ ������ ��� �� ������
                bool touchingOurBlock = blocks
                    .Where(other => other && other != block) // ��������� ������������ �������
                    .Any(other => other.GetAlignedPosition() == belowPos);

                if (!touchingOurBlock) // ������� ���� �� ���� ������, ������ ��������� ���� � �����.
                {
                    touchingBlock = block;
                    return true;
                }
            }
        }
        return false;
    }



    /// <summary>
    /// ��������� ������ �� ��������� �����, ������ ������������ ������.
    /// ���� ����� ����� ������� ������� ����� ���������.
    /// </summary>
    [ContextMenu("��������� ������")]
    public void Collapse()
    {
        foreach (BlockController block in blocks)
        {
            if (!block) continue; // Unity-������������� �������� �� ������������ ������
            // ������� ��� ��� ������ ���������:
            // FreeCells();

            // ��������� ������� �������(������ �� ��������):
            block.transform.parent = transform.parent; // ����������� ����� �� ������������� �������
        }

        Destroy(gameObject); // ������� ������, �� ������� ����� StructureController
    }

    /// <summary>
    /// ��������� ������, ��� ������ ����� ������ ����� ������.
    /// </summary>
    private void FillCells()
    {
        // ������� ��� ��� ������ ������:
        foreach (BlockController block in blocks)
        {
            if (!block) continue; // Unity-������������� �������� �� ������������ ������
            block.FillCell();
        }
    }

    /// <summary>
    /// ����������� ������, ��� ������ ����� ������ ����� ������.
    /// </summary>
    private void FreeCells()
    {
        // ������� ��� ��� ������ ��������:
        foreach (BlockController block in blocks)
        {
            if (!block) continue; // Unity-������������� �������� �� ������������ ������
            block.FreeCell();
        }
    }

    /// <summary>
    /// ��������� �������� �� ������ ������. �� ���� ��������� �� � ��� ������.
    /// </summary>
    private bool IsEmpty()
    {
        foreach (var block in blocks)
        {
            if (block) return false;
        }
        return true;
    }
}
