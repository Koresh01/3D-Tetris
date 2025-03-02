using UnityEngine;

/// <summary>
/// ������ ����� �� ������. ����� - ��� ��������� ����� ������.
/// </summary>
public class BlockController : MonoBehaviour
{
    [SerializeField] StructureController structureController;
    public bool isFalling = true;

    private void Start()
    {
        structureController = GetComponentInParent<StructureController>();
    }

    private void Update()
    {
        if (isFalling)
        {
            // ������� ����� ����
            transform.Translate(Vector3.down * Time.deltaTime, Space.World);

            // ���������, ���� ����� ������ �����������
            if (IsGrounded())
            {
                // ������������� �������, ������������� ������� �� �����������
                RaycastHit hit;
                if (Physics.Raycast(transform.position, Vector3.down, out hit, 1f))
                {
                    transform.position = new Vector3(
                        transform.position.x,
                        hit.collider.transform.position.y + 1f, // ��������� �� 1, ��� ��� ����� 1x1x1
                        transform.position.z);
                }
                isFalling = false;
            }
        }
    }

    /// <summary>
    /// ������ �� ����� �����������(����� ������������ ����).
    /// </summary>
    private bool IsGrounded()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.6f))
        {
            BlockController otherBlock = hit.collider.GetComponent<BlockController>();
            if (otherBlock != null && otherBlock.isFalling)
            {
                return false; // ���� ������ ����� ��� ������ � ���������� ���
            }
            return true;
        }
        return false;
    }
}
