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
        Move(Vector3.forward);
    }

    /// <summary>
    /// ������� ������� ������ ����� (�� ��� Z).
    /// </summary>
    public void MoveBackward()
    {
        Move(Vector3.back);
    }

    /// <summary>
    /// ������� ������� ������ ����� (�� ��� X).
    /// </summary>
    public void MoveLeft()
    {
        Move(Vector3.left);
    }

    /// <summary>
    /// ������� ������� ������ ������ (�� ��� X).
    /// </summary>
    public void MoveRight()
    {
        Move(Vector3.right);
    }

    /// <summary>
    /// ��������� �������� ����������� ������ �� 1 ������� � ��������� �����������.
    /// </summary>
    private void Move(Vector3 direction)
    {
        if (GameManager.currentDetail == null) return;

        bool hasGroundContact = GameManager.currentDetail.GetComponent<StructureController>().hasGroundContact;
        if (hasGroundContact) return;

        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(MoveOverTime(GameManager.currentDetail.transform, direction));
    }

    /// <summary>
    /// ������ ���������� ������ � ��������� ����������� �� 1 �������.
    /// </summary>
    private IEnumerator MoveOverTime(Transform target, Vector3 direction)
    {
        Vector3 startPosition = target.position;
        Vector3 endPosition = startPosition + direction;
        float moved = 0f;

        while (moved < 1f)
        {
            float step = moveSpeed * Time.deltaTime;
            moved += step;
            target.position = Vector3.Lerp(startPosition, endPosition, moved);
            yield return null;
        }

        target.position = endPosition; // ����������, ��� ������� ����� �� ����� �����������
    }
}
