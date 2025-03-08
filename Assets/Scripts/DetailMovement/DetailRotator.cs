using System.Collections;
using UnityEngine;

[AddComponentMenu("Custom/DetailRotator (Вращение падающей детали)")]
public class DetailRotator : MonoBehaviour
{
    /// <summary>
    /// Скорость вращения (градусов в секунду).
    /// </summary>
    [SerializeField] private float rotationSpeed = 200f;

    private Coroutine rotationCoroutine;

    /// <summary>
    /// Вращает текущую деталь вокруг мировой оси X по часовой стрелке (90°).
    /// </summary>
    public void RotateXClockwise()
    {
        Rotate(Vector3.right, 90f);
    }

    /// <summary>
    /// Вращает текущую деталь вокруг мировой оси X против часовой стрелки (-90°).
    /// </summary>
    public void RotateXCounterClockwise()
    {
        Rotate(Vector3.right, -90f);
    }

    /// <summary>
    /// Вращает текущую деталь вокруг мировой оси Y по часовой стрелке (90°).
    /// </summary>
    public void RotateYClockwise()
    {
        Rotate(Vector3.up, 90f);
    }

    /// <summary>
    /// Вращает текущую деталь вокруг мировой оси Y против часовой стрелки (-90°).
    /// </summary>
    public void RotateYCounterClockwise()
    {
        Rotate(Vector3.up, -90f);
    }

    /// <summary>
    /// Вращает текущую деталь вокруг мировой оси Z по часовой стрелке (90°).
    /// </summary>
    public void RotateZClockwise()
    {
        Rotate(Vector3.forward, 90f);
    }

    /// <summary>
    /// Вращает текущую деталь вокруг мировой оси Z против часовой стрелки (-90°).
    /// </summary>
    public void RotateZCounterClockwise()
    {
        Rotate(Vector3.forward, -90f);
    }

    /// <summary>
    /// Запускает корутину вращения детали на заданный угол вокруг указанной оси.
    /// </summary>
    private void Rotate(Vector3 worldAxis, float angle)
    {
        if (GameManager.currentDetail == null) return;

        bool hasGroundContact = GameManager.currentDetail.GetComponent<StructureController>().hasGroundContact;
        if (hasGroundContact) return;

        if (rotationCoroutine != null)
            StopCoroutine(rotationCoroutine);

        rotationCoroutine = StartCoroutine(RotateOverTime(GameManager.currentDetail.transform, worldAxis, angle));
    }

    /// <summary>
    /// Плавно поворачивает объект вокруг **мировой оси** на указанное количество градусов.
    /// </summary>
    private IEnumerator RotateOverTime(Transform target, Vector3 worldAxis, float angle)
    {
        float rotated = 0f;
        Quaternion startRotation = target.rotation;

        // Преобразуем мировую ось в локальную систему координат объекта
        Vector3 localAxis = target.InverseTransformDirection(worldAxis);

        Quaternion endRotation = startRotation * Quaternion.AngleAxis(angle, localAxis);

        while (rotated < Mathf.Abs(angle))
        {
            float step = rotationSpeed * Time.deltaTime;
            rotated += step;
            target.rotation = Quaternion.Slerp(startRotation, endRotation, rotated / Mathf.Abs(angle));
            yield return null;
        }

        target.rotation = endRotation; // Гарантируем точное попадание в нужный угол
    }
}
