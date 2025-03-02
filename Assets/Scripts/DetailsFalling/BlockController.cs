using UnityEngine;

/// <summary>
/// ���������� ����� � ��������� ���������� ���������� ������,
/// ������� �������� ������ ��������� (������).
/// </summary>
public class BlockController : MonoBehaviour
{
    [SerializeField] private StructureController parentStructure; // ������ �� ������������ ������
    public bool isFalling = false; // ���� ������� ���������� �����

    private void Start()
    {
        // �������� ������ �� ������������ ������ (������, � ������� ��������� ����)
        parentStructure = GetComponentInParent<StructureController>();
    }

    private void Update()
    {
        bool isParentFalling = parentStructure.isFalling;

        // ���� ������������ ������ ��� ������������, ���� ����� ������ ��������
        if (isFalling && !isParentFalling)
        {
            Fall();
        }
    }

    /// <summary>
    /// ���������� ���� ����.
    /// </summary>
    private void Fall()
    {
        transform.Translate(Vector3.down * Time.deltaTime, Space.World);

        // ���� �������� �����������, ��������� ����
        if (IsTouchingGround())
        {
            AlignToGround();
            isFalling = false;
        }
    }

    /// <summary>
    /// ���������, ��������� �� ��� ������ ����������� ��� ������ ����������� ����.
    /// </summary>
    public bool IsTouchingGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.5f))
        {
            BlockController otherBlock = hit.collider.GetComponent<BlockController>();

            if (otherBlock != null && otherBlock.isFalling)
            {
                return false; // ���������� �������� �����
            }
            return true; // �������� ����������� ��� ������������ �������
        }
        return false;
    }

    /// <summary>
    /// ����������� ���� �� �����������.
    /// </summary>
    private void AlignToGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.6f))
        {
            transform.position = new Vector3(
                transform.position.x,
                hit.collider.transform.position.y + 1f, // ��������� �� 1, ��� ��� ������ ������ 1x1x1
                transform.position.z);
        }
    }
}
