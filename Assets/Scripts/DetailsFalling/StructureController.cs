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

    private void Start()
    {
        // ������� ��� ����� ������ ���� ������
        blocks = new List<BlockController>(GetComponentsInChildren<BlockController>());
    }

    private void Update()
    {
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
        if (IsTouchingGround())
        {
            isFalling = false;
            StopAllBlocks();
        }
    }

    /// <summary>
    /// ���������, �������� �� ���� �� ���� ���� ����� ��� ������������ �������.
    /// </summary>
    private bool IsTouchingGround()
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
            block.isFalling = true;
            block.transform.parent = transform.parent; // ����������� ����� �� ������������� �������
        }

        Destroy(gameObject); // ������� ������, �� ������� ����� StructureController
    }

    /// <summary>
    /// ������������� ��� ����� ������ ������.
    /// </summary>
    private void StopAllBlocks()
    {
        foreach (BlockController block in blocks)
        {
            block.isFalling = false;
        }
    }
}
