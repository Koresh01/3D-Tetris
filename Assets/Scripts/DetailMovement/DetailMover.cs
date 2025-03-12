using System.Collections;
using UnityEngine;

[AddComponentMenu("Custom/DetailMover (����������� �������� ������)")]
public class DetailMover : MonoBehaviour
{
    /// <summary>
    /// �������� ����������� (������ � �������).
    /// </summary>
    [SerializeField] private float moveSpeed = 5f;

    private Coroutine moveCoroutine;

    /// <summary>
    /// ������� ������� ������ ����� (�� ��� Z).
    /// </summary>
    public void MoveForward()
    {
        Move(Vector3Int.forward);
    }

    /// <summary>
    /// ������� ������� ������ ����� (�� ��� Z).
    /// </summary>
    public void MoveBackward()
    {
        Move(Vector3Int.back);
    }

    /// <summary>
    /// ������� ������� ������ ����� (�� ��� X).
    /// </summary>
    public void MoveLeft()
    {
        Move(Vector3Int.left);
    }

    /// <summary>
    /// ������� ������� ������ ������ (�� ��� X).
    /// </summary>
    public void MoveRight()
    {
        Move(Vector3Int.right);
    }

    /// <summary>
    /// ��������� �������� ����������� ������ �� 1 ������� � ��������� �����������.
    /// </summary>
    private void Move(Vector3Int direction)
    {
        if (GameManager.currentDetail == null) return;


        StructureController structureController = GameManager.currentDetail.GetComponent<StructureController>();
        bool hasGroundContact = structureController.hasGroundContact;


        if (!hasGroundContact && CanMoveDirection(structureController, direction))
        {
            if (moveCoroutine != null)
                StopCoroutine(moveCoroutine);

            moveCoroutine = StartCoroutine(MoveOverTime(GameManager.currentDetail.transform, direction));
        }
        else
        {
            Debug.Log($"���������� ��������� � ����������� {direction}");
        }
    }

    /// <summary>
    /// ������ ���������� ������ � ��������� ����������� �� 1 �������.
    /// </summary>
    private IEnumerator MoveOverTime(Transform target, Vector3 direction)
    {
        Vector3 startPosition = target.position;

        // ��������� �������� ������� �����, ����� �������� ���������� ������������
        Vector3 endPosition = new Vector3(
            Mathf.Round(startPosition.x + direction.x),
            startPosition.y + direction.y,  // Mathf.Round(startPosition.y + direction.y),
            Mathf.Round(startPosition.z + direction.z)
        );

        float moved = 0f;

        while (moved < 1f)
        {
            float step = moveSpeed * Time.deltaTime;
            moved = Mathf.Min(moved + step, 1f); // �� ��� ����� �� 1.0

            target.position = Vector3.Lerp(startPosition, endPosition, moved); // ������ Lerp ���������

            yield return null;
        }

        // � ����� ����������� ������ ��������� � endPosition
        target.position = endPosition;
    }


    /// <summary>
    /// ��������� �������� �� �������� ��� ����������� � ���� �����������.
    /// </summary>
    bool CanMoveDirection(StructureController structureController, Vector3Int direction)
    {
        foreach (BlockController block in structureController.blocks)
        {
            if (!block) continue; // �������� �� ������������ ������

            Vector3Int movedPositionOfBlock = block.GetAlignedPosition() + direction;
            if (Grid.GetCellState(movedPositionOfBlock) == CellState.Filled)
            {
                return false;
            }
        }
        return true;
    }
}
