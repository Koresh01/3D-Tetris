using System.Collections.Generic;
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

    private void Update()
    {
        if (GameManager.isPaused) return;

        // ��������� ������� �������� � �����
        hasGroundContact = HasGroundContact(out BlockController touchingBlock);

        if (hasGroundContact)
        {
            HandleGroundContact(touchingBlock);
        }
        else
        {
            TryFall();
        }

        CheckAndDestroySelf();
    }

    /// <summary>
    /// ������������ ������� �����������.
    /// </summary>
    private void HandleGroundContact(BlockController touchingBlock)
    {
        FillCells();

        if (touchingBlock != null)
        {
            int layerIndex = touchingBlock.GetAlignedPosition().y;

            if (Grid.IsLayerFilled(layerIndex))
            {
                OnLayerDeleted?.Invoke();  // �������������, ��� ���� ���������
                // DetailsSpawner.Instance.SpawnNextDetail();
                Grid.DestroyLayer(layerIndex);
            }
        }
    }

    /// <summary>
    /// ������ ������� ���� ������.
    /// </summary>
    private void TryFall()
    {
        FreeCells();
        transform.Translate(Vector3.down * Time.deltaTime, Space.World);
    }

    /// <summary>
    /// ���������, �������� �� ���� �� ���� ���� ����� ��� ������� ������.
    /// </summary>
    public bool HasGroundContact(out BlockController touchingBlock)
    {
        touchingBlock = null;

        foreach (BlockController block in blocks)
        {
            if (!block) continue;

            Vector3Int blockPos = block.GetAlignedPosition();
            Vector3Int belowPos = blockPos + Vector3Int.down;

            if (Grid.GetCellState(belowPos) == CellState.Filled)
            {
                bool isOurBlock = blocks.Exists(other => other && other.GetAlignedPosition() == belowPos);
                if (!isOurBlock)
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
    /// </summary>
    [ContextMenu("��������� ������")]
    public void Collapse()
    {
        foreach (BlockController block in blocks)
        {
            if (!block) continue;
            block.transform.parent = transform.parent;
        }
        Destroy(gameObject);
    }

    /// <summary>
    /// ��������� ������, ��� ����� ������ ���� ������.
    /// </summary>
    private void FillCells()
    {
        foreach (BlockController block in blocks)
        {
            block?.FillCell();
        }
    }

    /// <summary>
    /// ����������� ������, ��� ��������� ������ ���� ������.
    /// </summary>
    private void FreeCells()
    {
        foreach (BlockController block in blocks)
        {
            block?.FreeCell();
        }
    }

    /// <summary>
    /// ���������, �������� �� ������ ������.
    /// </summary>
    private void CheckAndDestroySelf()
    {
        if (blocks.TrueForAll(block => block == null))
        {
            Destroy(gameObject);
        }
    }
}
