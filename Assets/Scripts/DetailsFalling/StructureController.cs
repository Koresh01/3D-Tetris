using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Управляет поведением падающей тетрис-детали и её блоков.
/// </summary>
public class StructureController : MonoBehaviour
{
    [SerializeField] private List<BlockController> blocks;

    [Tooltip("Падает ли вся деталь как единое целое.")]
    public bool isFalling = true;

    public bool hasGroundContact;

    private void Start()
    {
        // Находим все блоки внутри этой детали
        blocks = new List<BlockController>(GetComponentsInChildren<BlockController>());
    }

    private void LateUpdate()
    {
        // Сбор статистики:
        hasGroundContact = HasGroundContact();
        // Действие
        if (isFalling)
        {
            Fall();
        }
    }

    /// <summary>
    /// Перемещает всю деталь вниз.
    /// </summary>
    private void Fall()
    {
        transform.Translate(Vector3.down * Time.deltaTime, Space.World);

        // Если хотя бы один блок достиг поверхности, останавливаем деталь
        if (HasGroundContact())
        {
            isFalling = false;
            FillGrid();
        }
        else // Значит снова можно продолжить падение детали.
        {
            isFalling = true;
        }
    }

    /// <summary>
    /// Проверяет, касается ли хотя бы один блок земли или занятой клетки.
    /// </summary>
    private bool HasGroundContact()
    {
        foreach (BlockController block in blocks)
        {
            Vector3Int roundedPos = block.GetRoundedPosition();

            // Проверяем, есть ли блок под этим блоком в рамках той же детали
            if (!IsBlockCoveredByOtherBlock(block, roundedPos))
            {
                if (block.IsTouchingGround(roundedPos))
                {
                    return true;
                }
            }
        }
        return false;
    }
    /// <summary>
    /// Проверяет, если под текущим блоком есть другой блок той же детали.
    /// </summary>
    /// <param name="block">Текущий блок, для которого нужно проверить наличие блоков под ним.</param>
    /// <param name="position">Позиция текущего блока.</param>
    /// <returns>True, если под блоком есть другой блок этой же детали, иначе False.</returns>
    private bool IsBlockCoveredByOtherBlock(BlockController block, Vector3Int position)
    {
        foreach (BlockController otherBlock in blocks)
        {
            // Если это другой блок, и его позиция прямо под текущим блоком
            if (otherBlock != block)
            {
                Vector3Int otherBlockPos = otherBlock.GetRoundedPosition();
                // Проверяем, есть ли блок прямо под текущим блоком
                if (otherBlockPos.x == position.x && otherBlockPos.z == position.z && otherBlockPos.y == position.y - 1)
                {
                    return true; // Другой блок стоит под этим
                }
            }
        }
        return false; // Если не нашли блока под ним, возвращаем false
    }

    /// <summary>
    /// Разбирает деталь на отдельные блоки, удаляя родительский объект.
    /// Этот метод можно вызвать вручную через инспектор.
    /// </summary>
    [ContextMenu("Разобрать деталь")]
    private void Collapse()
    {
        foreach (BlockController block in blocks)
        {
            // Говорим что эти ячейки свободные:
            Vector3Int takenCellPosition = block.GetAlignedPosition();
            if (Grid.CanFall(takenCellPosition))
                Grid.SetCellState(takenCellPosition, CellState.Free);

            // Запускаем падение кубиков:
            block.isFalling = true;
            block.transform.parent = transform.parent; // Освобождаем блоки от родительского объекта
        }

        Destroy(gameObject); // Удаляем объект, на котором висел StructureController
    }

    /// <summary>
    /// Говорит Grid, какие ячейки заняты.
    /// </summary>
    private void FillGrid()
    {
        foreach (BlockController block in blocks)
        {
            // Говорим что эти ячейки заняты:
            Vector3Int takenCellPosition = block.GetAlignedPosition();
            Grid.SetCellState(takenCellPosition, CellState.Taken);

            // Debug.Log($"Cell -> Taken [{takenCellPosition.x}][{takenCellPosition.y}][{takenCellPosition.z}]");
        }
    }
}
