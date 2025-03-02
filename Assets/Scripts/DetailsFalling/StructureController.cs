using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Скрипт висит на детальке тетриса и упарвляет логикой кубиков из которых он состоит.
/// </summary>
public class StructureController : MonoBehaviour
{
    [SerializeField] private List<BlockController> blocks;

    private void Start()
    {
        // Находим все блоки в подчинении этого объекта
        blocks = new List<BlockController>(GetComponentsInChildren<BlockController>());
    }

    /// <summary>
    /// Метод вращает всю падающую детальку.
    /// </summary>
    /// <param name="rotation"></param>
    public void RotateStructure(Vector3 rotation)
    {
        transform.Rotate(rotation);
    }

    /// <summary>
    /// Метод как бы говорит что все кубики существуют сами
    /// по себе, то есть отдельно от своего родительского 
    /// GameObj на котором висит StructureController.
    /// </summary>
    public void Collapse()
    {
        foreach (BlockController block in blocks)
        {
            // Запускаем падение блоков
            block.isFalling = true;
        }
    }

    /// <summary>
    /// Метод проверяет, возможно какой то блок уже уперся,
    /// тогда тормозим всю конструкцию. Метод имеет смысл,
    /// только если целостность всей детали еще не была разрушена
    /// методом Collapse.
    /// </summary>
    private void CheckIfAnyBlockStopped()
    {
        foreach (BlockController block in blocks)
        {
            if (!block.isFalling)
            {
                StopAllBlocks();
                break; // Если один блок остановился, останавливаем проверку
            }
        }
    }

    /// <summary>
    /// Метод, останавливающий все блоки текущей детали.
    /// </summary>
    private void StopAllBlocks()
    {
        foreach (BlockController block in blocks)
        {
            block.isFalling = false; // Метод для остановки движения блока
        }
    }
}
