using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Управляет поведением падающей тетрис-детали и её блоков.
/// </summary>
public class StructureController : MonoBehaviour
{
    public List<BlockController> blocks;

    [Tooltip("Падает ли вся деталь как единое целое.")]
    public bool hasGroundContact;

    private void Start()
    {
        // Находим все блоки внутри этой детали
        blocks = new List<BlockController>(GetComponentsInChildren<BlockController>());
    }

    private void LateUpdate()
    {
        // Сбор статистики:
        hasGroundContact = HasGroundContact(out BlockController block);

        // Действие
        Fall();
    }

    /// <summary>
    /// Логика падения всей детали.
    /// </summary>
    private void Fall()
    {
        // Если хотя бы один блок достиг поверхности, останавливаем деталь
        if (HasGroundContact(out BlockController block))
        {
            // Занимаем эти ячейки:
            FillCells();
            // тут можно центрировать всю деталь...

            // Проверяем вдруг этот слой уже заполнен
            int layerInx = block.GetAlignedPosition().y;
            if (Grid.IsLayerFilled(layerInx))
            {
                GameManager.DestroyLayer(layerInx);
            }
        }
        else
        {
            FreeCells();
            transform.Translate(Vector3.down * Time.deltaTime, Space.World);
        }
    }

    /// <summary>
    /// Проверяет, касается ли хотя бы один блок земли или занятой клетки,
    /// игнорируя блоки этой же детали.
    /// </summary>
    private bool HasGroundContact(out BlockController touchingBlock)
    {
        touchingBlock = null; // Инициализируем переменную
        foreach (BlockController block in blocks)
        {
            if (!block) continue; // Unity-специфическая проверка на уничтоженный объект

            Vector3Int blockPos = block.GetAlignedPosition();
            Vector3Int belowPos = blockPos + Vector3Int.down;

            // Если блок касается земли или занятой клетки
            if (Grid.GetCellState(belowPos) == CellState.Filled)
            {
                // Проверяем, не является ли эта клетка частью той же детали
                bool touchingOurBlock = blocks
                    .Where(other => other && other != block) // Фильтруем уничтоженные объекты
                    .Any(other => other.GetAlignedPosition() == belowPos);

                if (!touchingOurBlock) // Тронули блок не этой детали, значит фиксируем упор в землю.
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
    /// Этот метод можно вызвать вручную через инспектор.
    /// </summary>
    [ContextMenu("Разобрать деталь")]
    private void Collapse()
    {
        foreach (BlockController block in blocks)
        {
            if (!block) continue; // Unity-специфическая проверка на уничтоженный объект
            // Говорим что эти ячейки свободные:
            // FreeCells();

            // Запускаем падение кубиков(удаляя им родителя):
            block.transform.parent = transform.parent; // Освобождаем блоки от родительского объекта
        }

        Destroy(gameObject); // Удаляем объект, на котором висел StructureController
    }

    /// <summary>
    /// Заполняет ячейки, где сейчас стоят кубики нашей детали.
    /// </summary>
    private void FillCells()
    {
        // Говорим что эти ячейки заняты:
        foreach (BlockController block in blocks)
        {
            if (!block) continue; // Unity-специфическая проверка на уничтоженный объект
            block.FillCell();
        }
    }

    /// <summary>
    /// Освобождает ячейки, где сейчас стоят кубики нашей детали.
    /// </summary>
    private void FreeCells()
    {
        // Говорим что эти ячейки свободны:
        foreach (BlockController block in blocks)
        {
            if (!block) continue; // Unity-специфическая проверка на уничтоженный объект
            block.FreeCell();
        }
    }
}
