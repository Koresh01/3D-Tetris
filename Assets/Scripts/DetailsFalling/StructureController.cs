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

    private void Start()
    {
        // Находим все блоки внутри этой детали
        blocks = new List<BlockController>(GetComponentsInChildren<BlockController>());
    }

    private void Update()
    {
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
        if (IsTouchingGround())
        {
            isFalling = false;
            StopAllBlocks();
        }
    }

    /// <summary>
    /// Проверяет, касается ли хотя бы один блок земли или неподвижного объекта.
    /// </summary>
    private bool IsTouchingGround()
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
            block.isFalling = true;
            block.transform.parent = transform.parent; // Освобождаем блоки от родительского объекта
        }

        Destroy(gameObject); // Удаляем объект, на котором висел StructureController
    }

    /// <summary>
    /// Останавливает все блоки внутри детали.
    /// </summary>
    private void StopAllBlocks()
    {
        foreach (BlockController block in blocks)
        {
            block.isFalling = false;
        }
    }
}
