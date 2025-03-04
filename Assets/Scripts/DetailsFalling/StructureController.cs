using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Управляет поведением падающей тетрис-детали и её блоков.
/// </summary>
public class StructureController : MonoBehaviour
{
    [SerializeField] private List<BlockController> blocks;

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
        hasGroundContact = HasGroundContact();

        // Действие
        Fall();
    }

    /// <summary>
    /// Логика падения всей детали.
    /// </summary>
    private void Fall()
    {
        // Если хотя бы один блок достиг поверхности, останавливаем деталь
        if (HasGroundContact())
        {
            FillCells();
            // тут можно центрировать всю деталь.
        }
        else
        {
            FreeCells();
            transform.Translate(Vector3.down * Time.deltaTime, Space.World);
        }
    }

    /// <summary>
    /// Проверяет, касается ли хотя бы один блок земли или занятой клетки.
    /// </summary>
    private bool HasGroundContact()
    {
        foreach (BlockController block in blocks)
        {
            if (block.IsTouchingGround())
            {
                return true;
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
            // Говорим что эти ячейки свободные:
            FreeCells();

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
        foreach (BlockController block in blocks)
        {
            // Говорим что эти ячейки заняты:
            block.FillCell();
        }
    }

    /// <summary>
    /// Освобождает ячейки, где сейчас стоят кубики нашей детали.
    /// </summary>
    private void FreeCells()
    {
        foreach (BlockController block in blocks)
        {
            // Говорим что эти ячейки свободны:
            block.FreeCell();
        }
    }
}
