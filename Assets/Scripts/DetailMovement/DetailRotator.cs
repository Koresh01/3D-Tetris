using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Custom/DetailRotator (Вращение падающей детали)")]
public class DetailRotator : MonoBehaviour
{
    /// <summary>
    /// Скорость вращения в градусах в секунду.
    /// </summary>
    [SerializeField] private float rotationSpeed = 200f;

    /// <summary>
    /// Очередь вращений. Хранит пары (ось, угол), которые нужно выполнить последовательно.
    /// </summary>
    private Queue<(Vector3 axis, float angle)> rotationQueue = new Queue<(Vector3, float)>();

    /// <summary>
    /// Флаг, указывающий, выполняется ли сейчас вращение.
    /// </summary>
    private bool isRotating = false;

    // --- Публичные методы для вращения вокруг осей ---
    public void RotateXClockwise() => AddRotation(Vector3.right, 90f);
    public void RotateXCounterClockwise() => AddRotation(Vector3.right, -90f);
    public void RotateYClockwise() => AddRotation(Vector3.up, 90f);
    public void RotateYCounterClockwise() => AddRotation(Vector3.up, -90f);
    public void RotateZClockwise() => AddRotation(Vector3.forward, 90f);
    public void RotateZCounterClockwise() => AddRotation(Vector3.forward, -90f);

    /// <summary>
    /// Добавляет вращение в очередь и запускает обработку, если она еще не выполняется.
    /// </summary>
    private void AddRotation(Vector3 worldAxis, float angle)
    {
        // Проверяем, есть ли текущая деталь и не касается ли она земли.
        if (GameManager.currentDetail == null) return;
        if (GameManager.currentDetail.GetComponent<StructureController>().hasGroundContact) return;

        // Добавляем вращение в очередь
        rotationQueue.Enqueue((worldAxis, angle));

        // Если вращение не выполняется - запускаем обработку очереди
        if (!isRotating)
            StartCoroutine(ProcessRotationQueue());
    }

    /// <summary>
    /// Обрабатывает очередь вращений, выполняя их последовательно.
    /// </summary>
    private IEnumerator ProcessRotationQueue()
    {
        isRotating = true; // Устанавливаем флаг, что начался процесс вращения

        while (rotationQueue.Count > 0)
        {
            // Берем следующее вращение из очереди
            var (worldAxis, angle) = rotationQueue.Dequeue();

            // Выполняем вращение и ждем его завершения
            yield return StartCoroutine(RotateOverTime(GameManager.currentDetail.transform, worldAxis, angle));
        }

        isRotating = false; // Все вращения выполнены, сбрасываем флаг
    }

    /// <summary>
    /// Плавно поворачивает объект вокруг указанной оси на заданный угол.
    /// </summary>
    private IEnumerator RotateOverTime(Transform target, Vector3 worldAxis, float angle)
    {
        float rotated = 0f; // Счетчик пройденного угла
        Quaternion startRotation = target.rotation; // Исходная ориентация объекта

        // Переводим мировую ось в локальную систему координат объекта
        Vector3 localAxis = target.InverseTransformDirection(worldAxis);

        // Вычисляем конечное вращение
        Quaternion endRotation = startRotation * Quaternion.AngleAxis(angle, localAxis);

        while (rotated < Mathf.Abs(angle))
        {
            // Определяем шаг вращения в зависимости от скорости и времени кадра
            float step = rotationSpeed * Time.deltaTime;
            float remaining = Mathf.Abs(angle) - rotated;

            // Если шаг превышает оставшееся расстояние, корректируем его
            if (step > remaining) step = remaining;

            // Поворачиваем объект относительно его локальной оси
            target.Rotate(localAxis, Mathf.Sign(angle) * step, Space.Self);
            rotated += step;

            yield return null; // Ждем следующий кадр
        }

        // Гарантируем точное попадание в нужный угол
        target.rotation = endRotation;
    }
}
