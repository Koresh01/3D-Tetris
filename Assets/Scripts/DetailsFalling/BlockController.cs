using UnityEngine;

/// <summary>
/// Контроллер блока — управляет поведением отдельного кубика,
/// который является частью тетромино (детали).
/// </summary>
public class BlockController : MonoBehaviour
{
    [SerializeField] private StructureController parentStructure; // Ссылка на родительскую деталь
    public bool isFalling = false; // Флаг падения отдельного блока

    private void Start()
    {
        // Получаем ссылку на родительский объект (деталь, к которой относится блок)
        parentStructure = GetComponentInParent<StructureController>();
    }

    private void Update()
    {
        bool isParentFalling = parentStructure.isFalling;

        // Если родительская деталь уже остановилась, даем блоку падать отдельно
        if (isFalling && !isParentFalling)
        {
            Fall();
        }
    }

    /// <summary>
    /// Перемещает блок вниз.
    /// </summary>
    private void Fall()
    {
        transform.Translate(Vector3.down * Time.deltaTime, Space.World);

        // Если достигли поверхности, фиксируем блок
        if (IsTouchingGround())
        {
            AlignToGround();
            isFalling = false;
        }
    }

    /// <summary>
    /// Проверяет, находится ли под блоком поверхность или другой неподвижный блок.
    /// </summary>
    public bool IsTouchingGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.5f))
        {
            BlockController otherBlock = hit.collider.GetComponent<BlockController>();

            if (otherBlock != null && otherBlock.isFalling)
            {
                return false; // Игнорируем падающие блоки
            }
            return true; // Достигли поверхности или неподвижного объекта
        }
        return false;
    }

    /// <summary>
    /// Выравнивает блок по поверхности.
    /// </summary>
    private void AlignToGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.6f))
        {
            transform.position = new Vector3(
                transform.position.x,
                hit.collider.transform.position.y + 1f, // Поднимаем на 1, так как размер кубика 1x1x1
                transform.position.z);
        }
    }
}
