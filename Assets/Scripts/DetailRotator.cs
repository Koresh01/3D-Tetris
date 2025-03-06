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
    /// Вращает текущую деталь вокруг мировой оси X.
    /// </summary>
    public void RotateX()
    {
        Rotate(Vector3.right);
    }

    /// <summary>
    /// Вращает текущую деталь вокруг мировой оси Y.
    /// </summary>
    public void RotateY()
    {
        Rotate(Vector3.up);
    }

    /// <summary>
    /// Вращает текущую деталь вокруг мировой оси Z.
    /// </summary>
    public void RotateZ()
    {
        Rotate(Vector3.forward);
    }

    /// <summary>
    /// Запускает корутину вращения детали на 90 градусов.
    /// </summary>
    private void Rotate(Vector3 worldAxis)
    {
        if (GameManager.currentDetail == null) return;

        bool hasGroundContact = GameManager.currentDetail.GetComponent<StructureController>().hasGroundContact;
        if (hasGroundContact) return;

        if (rotationCoroutine != null)
            StopCoroutine(rotationCoroutine);

        rotationCoroutine = StartCoroutine(RotateOverTime(GameManager.currentDetail.transform, worldAxis, 90f));
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

        while (rotated < angle)
        {
            float step = rotationSpeed * Time.deltaTime;
            rotated += step;
            target.rotation = Quaternion.Slerp(startRotation, endRotation, rotated / angle);
            yield return null;
        }

        target.rotation = endRotation; // Убеждаемся, что угол точно 90°
    }
}
