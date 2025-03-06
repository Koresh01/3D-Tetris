using System.Collections;
using UnityEngine;

[AddComponentMenu("Custom/DetailRotator (�������� �������� ������)")]
public class DetailRotator : MonoBehaviour
{
    /// <summary>
    /// �������� �������� (�������� � �������).
    /// </summary>
    [SerializeField] private float rotationSpeed = 200f;

    private Coroutine rotationCoroutine;

    /// <summary>
    /// ������� ������� ������ ������ ������� ��� X.
    /// </summary>
    public void RotateX()
    {
        Rotate(Vector3.right);
    }

    /// <summary>
    /// ������� ������� ������ ������ ������� ��� Y.
    /// </summary>
    public void RotateY()
    {
        Rotate(Vector3.up);
    }

    /// <summary>
    /// ������� ������� ������ ������ ������� ��� Z.
    /// </summary>
    public void RotateZ()
    {
        Rotate(Vector3.forward);
    }

    /// <summary>
    /// ��������� �������� �������� ������ �� 90 ��������.
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
    /// ������ ������������ ������ ������ **������� ���** �� ��������� ���������� ��������.
    /// </summary>
    private IEnumerator RotateOverTime(Transform target, Vector3 worldAxis, float angle)
    {
        float rotated = 0f;
        Quaternion startRotation = target.rotation;

        // ����������� ������� ��� � ��������� ������� ��������� �������
        Vector3 localAxis = target.InverseTransformDirection(worldAxis);

        Quaternion endRotation = startRotation * Quaternion.AngleAxis(angle, localAxis);

        while (rotated < angle)
        {
            float step = rotationSpeed * Time.deltaTime;
            rotated += step;
            target.rotation = Quaternion.Slerp(startRotation, endRotation, rotated / angle);
            yield return null;
        }

        target.rotation = endRotation; // ����������, ��� ���� ����� 90�
    }
}
