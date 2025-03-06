using System.Collections;
using UnityEngine;

[AddComponentMenu("Custom/DetailMover (Перемещение падающей детали)")]
public class DetailMover : MonoBehaviour
{
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
        Move(Vector3.forward);
    }

    /// <summary>
    /// Смещает текущую деталь назад (по оси Z).
    /// </summary>
    public void MoveBackward()
    {
        Move(Vector3.back);
    }

    /// <summary>
    /// Смещает текущую деталь влево (по оси X).
    /// </summary>
    public void MoveLeft()
    {
        Move(Vector3.left);
    }

    /// <summary>
    /// Смещает текущую деталь вправо (по оси X).
    /// </summary>
    public void MoveRight()
    {
        Move(Vector3.right);
    }

    /// <summary>
    /// Запускает корутину перемещения детали на 1 единицу в указанном направлении.
    /// </summary>
    private void Move(Vector3 direction)
    {
        if (GameManager.currentDetail == null) return;

        bool hasGroundContact = GameManager.currentDetail.GetComponent<StructureController>().hasGroundContact;
        if (hasGroundContact) return;

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
        Vector3 endPosition = startPosition + direction;
        float moved = 0f;

        while (moved < 1f)
        {
            float step = moveSpeed * Time.deltaTime;
            moved += step;
            target.position = Vector3.Lerp(startPosition, endPosition, moved);
            yield return null;
        }

        target.position = endPosition; // Убеждаемся, что позиция точно на целых координатах
    }
}
