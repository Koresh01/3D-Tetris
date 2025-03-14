using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Управляет поведением падающей тетрис-детали и её блоков.
/// </summary>
public class StructureController : MonoBehaviour
{
    public List<BlockController> blocks;

    [Tooltip("Падает ли вся деталь как единое целое.")]
    public bool hasGroundContact;

    [Tooltip("Событие отчистки слоя")]
    public static UnityAction OnLayerDeleted;

    private void Start()
    {
        // Находим все блоки внутри этой детали
        blocks = new List<BlockController>(GetComponentsInChildren<BlockController>());
    }

    private void Update()
    {
        if (GameManager.isPaused) return;

        // Проверяем наличие контакта с землёй
        hasGroundContact = HasGroundContact(out BlockController touchingBlock);

        if (hasGroundContact)
        {
            HandleGroundContact(touchingBlock);
        }
        else
        {
            TryFall();
        }

        CheckAndDestroySelf();
    }

    /// <summary>
    /// Обрабатывает касание поверхности.
    /// </summary>
    private void HandleGroundContact(BlockController touchingBlock)
    {
        FillCells();

        if (touchingBlock != null)
        {
            int layerIndex = touchingBlock.GetAlignedPosition().y;

            if (Grid.IsLayerFilled(layerIndex))
            {
                OnLayerDeleted?.Invoke();  // Сигнализируем, что слой удаляется
                // DetailsSpawner.Instance.SpawnNextDetail();
                Grid.DestroyLayer(layerIndex);
            }
        }
    }

    /// <summary>
    /// Логика падения всей детали.
    /// </summary>
    private void TryFall()
    {
        FreeCells();
        transform.Translate(Vector3.down * Time.deltaTime, Space.World);
    }

    /// <summary>
    /// Проверяет, касается ли хотя бы один блок земли или занятой клетки.
    /// </summary>
    public bool HasGroundContact(out BlockController touchingBlock)
    {
        touchingBlock = null;

        foreach (BlockController block in blocks)
        {
            if (!block) continue;

            Vector3Int blockPos = block.GetAlignedPosition();
            Vector3Int belowPos = blockPos + Vector3Int.down;

            if (Grid.GetCellState(belowPos) == CellState.Filled)
            {
                bool isOurBlock = blocks.Exists(other => other && other.GetAlignedPosition() == belowPos);
                if (!isOurBlock)
                {
                    touchingBlock = block;
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Разбирает деталь на отдельные блоки, удаляя родительский объект.
    /// </summary>
    [ContextMenu("Разобрать деталь")]
    public void Collapse()
    {
        foreach (BlockController block in blocks)
        {
            if (!block) continue;
            block.transform.parent = transform.parent;
        }
        Destroy(gameObject);
    }

    /// <summary>
    /// Заполняет ячейки, где стоят кубики этой детали.
    /// </summary>
    private void FillCells()
    {
        foreach (BlockController block in blocks)
        {
            block?.FillCell();
        }
    }

    /// <summary>
    /// Освобождает ячейки, где находятся кубики этой детали.
    /// </summary>
    private void FreeCells()
    {
        foreach (BlockController block in blocks)
        {
            block?.FreeCell();
        }
    }

    /// <summary>
    /// Проверяет, является ли деталь пустой.
    /// </summary>
    private void CheckAndDestroySelf()
    {
        if (blocks.TrueForAll(block => block == null))
        {
            Destroy(gameObject);
        }
    }
}
