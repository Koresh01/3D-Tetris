using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ ����� �� �������� ������� � ��������� ������� ������� �� ������� �� �������.
/// </summary>
public class StructureController : MonoBehaviour
{
    [SerializeField] private List<BlockController> blocks;

    private void Start()
    {
        // ������� ��� ����� � ���������� ����� �������
        blocks = new List<BlockController>(GetComponentsInChildren<BlockController>());
    }

    /// <summary>
    /// ����� ������� ��� �������� ��������.
    /// </summary>
    /// <param name="rotation"></param>
    public void RotateStructure(Vector3 rotation)
    {
        transform.Rotate(rotation);
    }

    /// <summary>
    /// ����� ��� �� ������� ��� ��� ������ ���������� ����
    /// �� ����, �� ���� �������� �� ������ ������������� 
    /// GameObj �� ������� ����� StructureController.
    /// </summary>
    public void Collapse()
    {
        foreach (BlockController block in blocks)
        {
            // ��������� ������� ������
            block.isFalling = true;
        }
    }

    /// <summary>
    /// ����� ���������, �������� ����� �� ���� ��� ������,
    /// ����� �������� ��� �����������. ����� ����� �����,
    /// ������ ���� ����������� ���� ������ ��� �� ���� ���������
    /// ������� Collapse.
    /// </summary>
    private void CheckIfAnyBlockStopped()
    {
        foreach (BlockController block in blocks)
        {
            if (!block.isFalling)
            {
                StopAllBlocks();
                break; // ���� ���� ���� �����������, ������������� ��������
            }
        }
    }

    /// <summary>
    /// �����, ��������������� ��� ����� ������� ������.
    /// </summary>
    private void StopAllBlocks()
    {
        foreach (BlockController block in blocks)
        {
            block.isFalling = false; // ����� ��� ��������� �������� �����
        }
    }
}
