using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Custom/DetailRotator (�������� �������� ������)")]
public class DetailRotator : MonoBehaviour
{
    /// <summary>
    /// �������� �������� � �������� � �������.
    /// </summary>
    [SerializeField] private float rotationSpeed = 200f;

    /// <summary>
    /// ������� ��������. ������ ���� (���, ����), ������� ����� ��������� ���������������.
    /// </summary>
    private Queue<(Vector3 axis, float angle)> rotationQueue = new Queue<(Vector3, float)>();

    /// <summary>
    /// ����, �����������, ����������� �� ������ ��������.
    /// </summary>
    private bool isRotating = false;

    // --- ��������� ������ ��� �������� ������ ���� ---
    public void RotateXClockwise() => AddRotation(Vector3.right, 90f);
    public void RotateXCounterClockwise() => AddRotation(Vector3.right, -90f);
    public void RotateYClockwise() => AddRotation(Vector3.up, 90f);
    public void RotateYCounterClockwise() => AddRotation(Vector3.up, -90f);
    public void RotateZClockwise() => AddRotation(Vector3.forward, 90f);
    public void RotateZCounterClockwise() => AddRotation(Vector3.forward, -90f);

    /// <summary>
    /// ��������� �������� � ������� � ��������� ���������, ���� ��� ��� �� �����������.
    /// </summary>
    private void AddRotation(Vector3 worldAxis, float angle)
    {
        // ���������, ���� �� ������� ������ � �� �������� �� ��� �����.
        if (GameManager.currentDetail == null) return;
        if (GameManager.currentDetail.GetComponent<StructureController>().hasGroundContact) return;

        // ��������� �������� � �������
        rotationQueue.Enqueue((worldAxis, angle));

        // ���� �������� �� ����������� - ��������� ��������� �������
        if (!isRotating)
            StartCoroutine(ProcessRotationQueue());
    }

    /// <summary>
    /// ������������ ������� ��������, �������� �� ���������������.
    /// </summary>
    private IEnumerator ProcessRotationQueue()
    {
        isRotating = true; // ������������� ����, ��� ������� ������� ��������

        while (rotationQueue.Count > 0)
        {
            // ����� ��������� �������� �� �������
            var (worldAxis, angle) = rotationQueue.Dequeue();

            // ��������� �������� � ���� ��� ����������
            yield return StartCoroutine(RotateOverTime(GameManager.currentDetail.transform, worldAxis, angle));
        }

        isRotating = false; // ��� �������� ���������, ���������� ����
    }

    /// <summary>
    /// ������ ������������ ������ ������ ��������� ��� �� �������� ����.
    /// </summary>
    private IEnumerator RotateOverTime(Transform target, Vector3 worldAxis, float angle)
    {
        float rotated = 0f; // ������� ����������� ����
        Quaternion startRotation = target.rotation; // �������� ���������� �������

        // ��������� ������� ��� � ��������� ������� ��������� �������
        Vector3 localAxis = target.InverseTransformDirection(worldAxis);

        // ��������� �������� ��������
        Quaternion endRotation = startRotation * Quaternion.AngleAxis(angle, localAxis);

        while (rotated < Mathf.Abs(angle))
        {
            // ���������� ��� �������� � ����������� �� �������� � ������� �����
            float step = rotationSpeed * Time.deltaTime;
            float remaining = Mathf.Abs(angle) - rotated;

            // ���� ��� ��������� ���������� ����������, ������������ ���
            if (step > remaining) step = remaining;

            // ������������ ������ ������������ ��� ��������� ���
            target.Rotate(localAxis, Mathf.Sign(angle) * step, Space.Self);
            rotated += step;

            yield return null; // ���� ��������� ����
        }

        // ����������� ������ ��������� � ������ ����
        target.rotation = endRotation;
    }
}
