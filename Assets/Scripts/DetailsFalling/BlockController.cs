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
    [SerializeField] Vector3Int roundedWorldPos;    // Округлённые мировые координаты
    [SerializeField] Vector3Int alignedWorldPos;    // Округлённые мировые координаты

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
        roundedWorldPos = GetRoundedPosition();
        alignedWorldPos = GetAlignedPosition();

        isTouchingGround = IsTouchingGround(GetRoundedPosition());

        // Действие:
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
    /// Перемещает блок вниз.
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
    /// Проверяет, есть ли под блоком препятствие.
    /// </summary>
    public bool IsTouchingGround(Vector3Int roundedPos)
    {
        // return roundedPos.y == 0 || Grid.GetCellState(roundedPos) == CellState.Taken;
        return Grid.GetCellState(roundedPos) == CellState.Taken;
    }

    /// <summary>
    /// Возвращает округлённую позицию кубика.
    /// (Округлённую до нижнего значения по оси Y).
    /// </summary>
    public Vector3Int GetRoundedPosition()
    {
        worldPos = transform.position; // Уже глобальная позиция

        return new Vector3Int(
            Mathf.RoundToInt(worldPos.x),
            Mathf.FloorToInt(worldPos.y),
            Mathf.RoundToInt(worldPos.z)
        );
    }

    /// <summary>
    /// Возвращает позицию блока по сетке.
    /// (Не округляет до нижнего значения по оси Y).
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
