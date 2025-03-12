using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu("Custom/DetailMover (Перемещение падающей детали)")]
public class DetailMover : MonoBehaviour
{
    [Tooltip("Событие невозможности смещения")]
    public UnityAction OnCanNotMove;

    /// <summary>
    /// Скорость перемещения (единиц в секунду).
    /// </summary>
    [SerializeField] private float moveSpeed = 5f;

    private Coroutine moveCoroutine;

    /// <summary>
    /// Смещает текущую деталь вперёд (по оси Z).
    /// </summary>
    public void MoveForward()
    {
        Move(Vector3Int.forward);
    }

    /// <summary>
    /// Смещает текущую деталь назад (по оси Z).
    /// </summary>
    public void MoveBackward()
    {
        Move(Vector3Int.back);
    }

    /// <summary>
    /// Смещает текущую деталь влево (по оси X).
    /// </summary>
    public void MoveLeft()
    {
        Move(Vector3Int.left);
    }

    /// <summary>
    /// Смещает текущую деталь вправо (по оси X).
    /// </summary>
    public void MoveRight()
    {
        Move(Vector3Int.right);
    }

    /// <summary>
    /// Запускает корутину перемещения детали на 1 единицу в указанном направлении.
    /// </summary>
    private void Move(Vector3Int direction)
    {
        if (GameManager.currentDetail == null) return;

        var structureController = GameManager.currentDetail.GetComponent<StructureController>();

        bool canMove = !structureController.hasGroundContact && CanMoveDirection(structureController, direction);
        if (!canMove)
        {
            Debug.Log($"Объект {GameManager.currentDetail.name} не может двигаться в направлении {direction}");
            return;
        }

        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(MoveOverTime(GameManager.currentDetail.transform, direction));
    }

    /// <summary>
    /// Плавно перемещает объект в указанном направлении на 1 единицу.
    /// </summary>
    private IEnumerator MoveOverTime(Transform target, Vector3 direction)
    {
        Vector3 startPosition = target.position;

        // Округляем конечную позицию сразу, чтобы избежать накопления погрешностей
        Vector3 endPosition = new Vector3(
            Mathf.Round(startPosition.x + direction.x),
            startPosition.y + direction.y,  // Mathf.Round(startPosition.y + direction.y),
            Mathf.Round(startPosition.z + direction.z)
        );

        float moved = 0f;

        while (moved < 1f)
        {
            float step = moveSpeed * Time.deltaTime;
            moved = Mathf.Min(moved + step, 1f); // Не даём выйти за 1.0

            target.position = Vector3.Lerp(startPosition, endPosition, moved); // Теперь Lerp безопасен

            yield return null;
        }

        // В конце гарантируем точное попадание в endPosition
        target.position = endPosition;
    }


    /// <summary>
    /// Проверяет, возможно ли сдвинуть всю конструкцию в указанном направлении.
    /// </summary>
    bool CanMoveDirection(StructureController structureController, Vector3Int direction)
    {
        foreach (var block in structureController.blocks)
        {
            if (!block) continue;

            // Целевая ячейка поля:
            Vector3Int targetPosition = block.GetAlignedPosition() + direction;
            // Та что под ней:
            Vector3Int belowTargetPosition = targetPosition + Vector3Int.down;

            // Если хотя бы одна клетка занята, движение невозможно
            if (Grid.GetCellState(targetPosition) == CellState.Filled ||
                Grid.GetCellState(belowTargetPosition) == CellState.Filled)
            {
                OnCanNotMove?.Invoke();
                return false;
            }
        }

        return true;
    }

}
