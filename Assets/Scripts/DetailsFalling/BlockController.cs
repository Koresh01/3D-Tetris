using UnityEngine;

/// <summary>
/// Контроллер блока — управляет поведением отдельного кубика,
/// который является частью тетромино (детали).
/// </summary>
public class BlockController : MonoBehaviour
{
    [SerializeField] private StructureController parentStructure; // Ссылка на родительскую деталь
    public bool isFalling = false; // Флаг падения отдельного блока

    [Header("Положение кубика:")]
    [SerializeField] Vector3 localPos;       // Локальные координаты относительно родителя
    [SerializeField] Vector3 worldPos;       // Глобальные координаты
    [SerializeField] Vector3Int alignedWorldPos;    // Округлённые мировые координаты
    [SerializeField] bool haveParent;

    [Header("Состояник кубика:")]
    [SerializeField] bool isTouchingGround;

    private void Start()
    {
        parentStructure = GetComponentInParent<StructureController>();
    }

    private void LateUpdate()
    {
        // Сбор данных о положении:
        localPos = transform.localPosition;
        worldPos = transform.position;
        alignedWorldPos = GetAlignedPosition();
        isTouchingGround = IsTouchingGround();
        haveParent = parentStructure != null;

        // Действие:
        if (!haveParent)
            Fall();
    }

    /// <summary>
    /// Перемещает блок вниз.
    /// </summary>
    private void Fall()
    {
        if (IsTouchingGround())
        {
            // тут можно центрировать кубики.
            FillCell();
        }
        else
        {
            FreeCell();
            transform.Translate(Vector3.down * Time.deltaTime, Space.World);
        }
    }

    /// <summary>
    /// Проверяет, свободна ли клетка.
    /// </summary>
    /// <param name="roundedPos">позиция</param>
    public CellState GetCellState(Vector3Int position)
    {
        return Grid.GetCellState(position);
    }
    
    /// <summary>
    /// Возвращает позицию блока по сетке.
    /// </summary>
    public Vector3Int GetAlignedPosition()
    {
        return new Vector3Int(
            Mathf.RoundToInt(transform.position.x),
            Mathf.CeilToInt(transform.position.y), // Округление вверх
            Mathf.RoundToInt(transform.position.z)
        );
    }

    /// <summary>
    /// Проверяет, касается ли блок земли.
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
    /// Заполняет ячейку где расположен этот кубик.
    /// </summary>
    public void FillCell()
    {
        Vector3Int CellPosition = GetAlignedPosition();
        Grid.SetCellState(CellPosition, CellState.Filled);

        CellsVizualizer.Instance.ReGenerate();
    }

    /// <summary>
    /// Освобождает ячейку где расположен этот кубик.
    /// </summary>
    public void FreeCell()
    {
        Vector3Int CellPosition = GetAlignedPosition();

        if (CellPosition == Vector3Int.zero)
        {
            Debug.LogError("Обращение к нулевому квадратику.");
            Debug.Log($"parent Name: {parentStructure.name}");
            Debug.Log($"block Name: {transform.name}");
            Debug.Log($"block pos: {transform.position}");
            Debug.Log($"block aligned Position: {GetAlignedPosition()}");
        }

        Grid.SetCellState(CellPosition, CellState.Free);

        CellsVizualizer.Instance.ReGenerate();
    }
}
