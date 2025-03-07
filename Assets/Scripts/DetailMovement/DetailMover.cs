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



}
