using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu("Custom/DetailRotator (Вращение падающей детали)")]
public class DetailRotator : MonoBehaviour
{
    [Tooltip("Событие невозможности вращения")]
    public UnityAction OnCanNotRotate;

    /// <summary>
    /// Скорость вращения в градусах в секунду.
    /// </summary>
    [SerializeField] private float rotationSpeed = 200f;

    /// <summary>
    /// Флаг, указывающий, выполняется ли сейчас вращение.
    /// </summary>
    private bool isRotating = false;

    // --- Публичные методы для вращения вокруг осей ---
    public void RotateXClockwise() => TryRotate(Vector3.right, 90f);
    public void RotateXCounterClockwise() => TryRotate(Vector3.right, -90f);
    public void RotateYClockwise() => TryRotate(Vector3.up, 90f);
    public void RotateYCounterClockwise() => TryRotate(Vector3.up, -90f);
    public void RotateZClockwise() => TryRotate(Vector3.forward, 90f);
    public void RotateZCounterClockwise() => TryRotate(Vector3.forward, -90f);

    /// <summary>
    /// Проверяет возможность вращения и запускает анимацию, если текущее вращение завершено.
    /// </summary>
    private void TryRotate(Vector3 worldAxis, float angle)
    {
        if (isRotating || GameManager.currentDetail == null) return;

        var structureController = GameManager.currentDetail.GetComponent<StructureController>();

        if (structureController.hasGroundContact || !CanRotate(structureController, worldAxis, angle))
            return;

        StartCoroutine(RotateOverTime(GameManager.currentDetail.transform, worldAxis, angle));
    }

    /// <summary>
    /// Плавно поворачивает объект вокруг указанной оси на заданный угол.
    /// </summary>
    private IEnumerator RotateOverTime(Transform target, Vector3 worldAxis, float angle)
    {
        isRotating = true; // Устанавливаем флаг, что началось вращение

        float rotated = 0f;
        Quaternion startRotation = target.rotation;
        Vector3 localAxis = target.InverseTransformDirection(worldAxis);
        Quaternion endRotation = startRotation * Quaternion.AngleAxis(angle, localAxis);

        while (rotated < Mathf.Abs(angle))
        {
            float step = rotationSpeed * Time.deltaTime;
            float remaining = Mathf.Abs(angle) - rotated;

            if (step > remaining) step = remaining;

            target.Rotate(localAxis, Mathf.Sign(angle) * step, Space.Self);
            rotated += step;

            yield return null;
        }

        target.rotation = endRotation;
        isRotating = false; // Сбрасываем флаг по завершении вращения
    }

    /// <summary>
    /// Проверяет, возможно ли повернуть всю конструкцию на заданный угол вокруг указанной оси.
    /// </summary>
    private bool CanRotate(StructureController structureController, Vector3 worldAxis, float angle)
    {
        Transform detailTransform = structureController.transform;
        Quaternion rotation = Quaternion.AngleAxis(angle, worldAxis);

        foreach (var block in structureController.blocks)
        {
            if (!block) continue;

            Vector3Int currentPos = block.GetAlignedPosition();
            Vector3 rotatedPos = rotation * (currentPos - detailTransform.position) + detailTransform.position;
            
            // Позиция кубика как если бы он повернулся:
            Vector3Int targetPos = Vector3Int.RoundToInt(rotatedPos);
            Vector3Int belowPos = targetPos + Vector3Int.down;

            if (Grid.GetCellState(targetPos) == CellState.Filled || Grid.GetCellState(belowPos) == CellState.Filled)
            {
                OnCanNotRotate?.Invoke();
                return false;
            }
        }

        return true;
    }
}
