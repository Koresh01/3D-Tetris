using UnityEngine;

/// <summary>
/// Скрипт висит на кубике. Кубик - это состовная часть детали.
/// </summary>
public class BlockController : MonoBehaviour
{
    [SerializeField] StructureController structureController;
    public bool isFalling = true;

    private void Start()
    {
        structureController = GetComponentInParent<StructureController>();
    }

    private void Update()
    {
        if (isFalling)
        {
            // Двигаем кубик вниз
            transform.Translate(Vector3.down * Time.deltaTime, Space.World);

            // Проверяем, если кубик достиг поверхности
            if (IsGrounded())
            {
                // Останавливаем падение, устанавливаем позицию на поверхность
                RaycastHit hit;
                if (Physics.Raycast(transform.position, Vector3.down, out hit, 1f))
                {
                    transform.position = new Vector3(
                        transform.position.x,
                        hit.collider.transform.position.y + 1f, // Поднимаем на 1, так как кубик 1x1x1
                        transform.position.z);
                }
                isFalling = false;
            }
        }
    }

    /// <summary>
    /// Достиг ли кубик поверхности(снизу относительно себя).
    /// </summary>
    private bool IsGrounded()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.6f))
        {
            BlockController otherBlock = hit.collider.GetComponent<BlockController>();
            if (otherBlock != null && otherBlock.isFalling)
            {
                return false; // Если другой кубик еще падает — игнорируем его
            }
            return true;
        }
        return false;
    }
}
