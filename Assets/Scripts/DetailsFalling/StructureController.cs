using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��������� ���������� �������� ������-������ � � ������.
/// </summary>
public class StructureController : MonoBehaviour
{
    [SerializeField] private List<BlockController> blocks;

    [Tooltip("������ �� ��� ������ ��� ������ �����.")]
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
        Fall();
    }

    /// <summary>
    /// ������ ������� ���� ������.
    /// </summary>
    private void Fall()
    {
        // ���� ���� �� ���� ���� ������ �����������, ������������� ������
        if (HasGroundContact())
        {
            FillCells();
            // ��� ����� ������������ ��� ������.
        }
        else
        {
            FreeCells();
            transform.Translate(Vector3.down * Time.deltaTime, Space.World);
        }
    }

    /// <summary>
    /// ���������, �������� �� ���� �� ���� ���� ����� ��� ������� ������.
    /// </summary>
    private bool HasGroundContact()
    {
        foreach (BlockController block in blocks)
        {
            if (block.IsTouchingGround())
            {
                return true;
            }
        }
        return false;
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
            FreeCells();

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
        foreach (BlockController block in blocks)
        {
            // ������� ��� ��� ������ ������:
            block.FillCell();
        }
    }

    /// <summary>
    /// ����������� ������, ��� ������ ����� ������ ����� ������.
    /// </summary>
    private void FreeCells()
    {
        foreach (BlockController block in blocks)
        {
            // ������� ��� ��� ������ ��������:
            block.FreeCell();
        }
    }
}
