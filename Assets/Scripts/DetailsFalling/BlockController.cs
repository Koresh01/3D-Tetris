using UnityEngine;

/// <summary>
/// ���������� ����� � ��������� ���������� ���������� ������,
/// ������� �������� ������ ��������� (������).
/// </summary>
public class BlockController : MonoBehaviour
{
    [SerializeField] private StructureController parentStructure; // ������ �� ������������ ������
    public bool isFalling = false; // ���� ������� ���������� �����

    [Header("��������� ������:")]
    [SerializeField] Vector3 localPos;       // ��������� ���������� ������������ ��������
    [SerializeField] Vector3 worldPos;       // ���������� ����������
    [SerializeField] Vector3Int roundedWorldPos;    // ���������� ������� ����������
    [SerializeField] Vector3Int alignedWorldPos;    // ���������� ������� ����������

    [Header("��������� ������:")]
    [SerializeField] bool isTouchingGround;

    private void Start()
    {
        parentStructure = GetComponentInParent<StructureController>();
    }

    private void LateUpdate()
    {
        // ���� ������ � ���������:
        localPos = transform.localPosition;
        worldPos = transform.position;
        roundedWorldPos = GetRoundedPosition();
        alignedWorldPos = GetAlignedPosition();

        isTouchingGround = IsTouchingGround(GetRoundedPosition());

        // ��������:
        if (!parentStructure.isFalling)
        {
            if (isFalling)
                Fall();
            else if (parentStructure == null)
            {
                isFalling = true;
            }
        }
    }

    /// <summary>
    /// ���������� ���� ����.
    /// </summary>
    private void Fall()
    {
        Vector3Int roundedPos = GetRoundedPosition();

        if (IsTouchingGround(roundedPos))
        {
            //AlignToGround(roundedPos);
            isFalling = false;

            Vector3Int alignedGridPos = GetAlignedPosition();
            Grid.SetCellState(alignedGridPos, CellState.Taken);
        }
        else
        {
            transform.Translate(Vector3.down * Time.deltaTime, Space.World);
        }
    }

    /// <summary>
    /// ���������, ���� �� ��� ������ �����������.
    /// </summary>
    public bool IsTouchingGround(Vector3Int roundedPos)
    {
        // return roundedPos.y == 0 || Grid.GetCellState(roundedPos) == CellState.Taken;
        return Grid.GetCellState(roundedPos) == CellState.Taken;
    }

    /// <summary>
    /// ���������� ���������� ������� ������.
    /// (���������� �� ������� �������� �� ��� Y).
    /// </summary>
    public Vector3Int GetRoundedPosition()
    {
        worldPos = transform.position; // ��� ���������� �������

        return new Vector3Int(
            Mathf.RoundToInt(worldPos.x),
            Mathf.FloorToInt(worldPos.y),
            Mathf.RoundToInt(worldPos.z)
        );
    }

    /// <summary>
    /// ���������� ������� ����� �� �����.
    /// (�� ��������� �� ������� �������� �� ��� Y).
    /// </summary>
    public Vector3Int GetAlignedPosition()
    {
        return new Vector3Int(
            Mathf.RoundToInt(worldPos.x),
            Mathf.RoundToInt(worldPos.y),
            Mathf.RoundToInt(worldPos.z)
        );
    }
}
