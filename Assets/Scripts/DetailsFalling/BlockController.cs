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
    [SerializeField] Vector3Int alignedWorldPos;    // ���������� ������� ����������
    [SerializeField] bool haveParent;

    [Header("��������� ������:")]
    [SerializeField] bool isTouchingGround;

    private void Start()
    {
        parentStructure = GetComponentInParent<StructureController>();
    }

    private void Update()
    {
        // ���� ������ � ���������:
        localPos = transform.localPosition;
        worldPos = transform.position;
        alignedWorldPos = GetAlignedPosition();
        isTouchingGround = IsTouchingGround();
        haveParent = parentStructure != null;

        // ��������:
        if (!haveParent)
            Fall();
    }

    /// <summary>
    /// ���������� ���� ����.
    /// </summary>
    private void Fall()
    {
        if (IsTouchingGround())
        {
            // ��� ����� ������������ ������.
            FillCell();
        }
        else
        {
            FreeCell();
            transform.Translate(Vector3.down * Time.deltaTime, Space.World);
        }
    }

    /// <summary>
    /// ���������, �������� �� ������.
    /// </summary>
    /// <param name="roundedPos">�������</param>
    public CellState GetCellState(Vector3Int position)
    {
        return Grid.GetCellState(position);
    }
    
    /// <summary>
    /// ���������� ������� ����� �� �����.
    /// </summary>
    public Vector3Int GetAlignedPosition()
    {
        return new Vector3Int(
            Mathf.RoundToInt(transform.position.x),
            Mathf.CeilToInt(transform.position.y), // ���������� �����
            Mathf.RoundToInt(transform.position.z)
        );
    }

    /// <summary>
    /// ���������, �������� �� ���� �����.
    /// </summary>
    /// <returns></returns>
    public bool IsTouchingGround()
    {
        Vector3Int alignedCellPos = GetAlignedPosition();
        if (GetCellState(alignedCellPos + Vector3Int.down) == CellState.Filled)
        {
            return true;
        }
        else
            return false;
    }

    /// <summary>
    /// ��������� ������ ��� ���������� ���� �����.
    /// </summary>
    public void FillCell()
    {
        Vector3Int CellPosition = GetAlignedPosition();
        
        // ������� � Grid ���� �����, ������ ��� �� ����� ������.
        Grid.SetCellState(CellPosition, transform.gameObject, CellState.Filled);

        // ��������� ��������� Grid �� ������ ������.
        if (CellsVizualizer.Instance != null)
        {
            CellsVizualizer.Instance.UpdateCellMaterial(CellPosition);
        }
    }

    /// <summary>
    /// ����������� ������ ��� ���������� ���� �����.
    /// </summary>
    public void FreeCell()
    {
        Vector3Int CellPosition = GetAlignedPosition();

        // ������� � Grid ���� �����, ������ ��� �� ��������� ������.
        Grid.SetCellState(CellPosition, transform.gameObject, CellState.Free);

        // ��������� ��������� Grid �� ������ ������.
        if (CellsVizualizer.Instance != null)
        {
            CellsVizualizer.Instance.UpdateCellMaterial(CellPosition);
        }
    }
}
