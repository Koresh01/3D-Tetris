using UnityEngine;
using System.Collections;

/// <summary>
/// ��������� ������������ � ��������� ������ ����� ������� ������� � ������� ����.
/// </summary>
[AddComponentMenu("Custom/CameraController (���������� ������)")]
public class CameraMover : MonoBehaviour
{
    [SerializeField, Tooltip("��������� ����� ������������ � ������.")]
    private UserInputSettings userInputSettings;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private Coroutine moveCoroutine;
    private Coroutine rotateCoroutine;

    private void Start()
    {
        // ���������� ��������� ������� � ������� ������
        initialPosition = userInputSettings.cameraTransform.position;
        initialRotation = userInputSettings.cameraTransform.rotation;
    }

    /// <summary>
    /// ����������� ������ � ����� "����".
    /// </summary>
    public void SwitchToGameMode()
    {
        StopActiveCoroutines();

        Vector3 targetPosition = userInputSettings.target.position;
        Quaternion targetRotation = Quaternion.LookRotation(
            userInputSettings.target.position + Vector3.up * GameManager.gridHeight / 3f - userInputSettings.cameraTransform.position
        );

        moveCoroutine = StartCoroutine(MoveToTarget(targetPosition, 20f, 1f));
        rotateCoroutine = StartCoroutine(RotateToTarget(targetRotation, 1f));
    }

    /// <summary>
    /// ����������� ������ � ����� "����".
    /// </summary>
    public void SwitchToMenuMode()
    {
        StopActiveCoroutines();

        moveCoroutine = StartCoroutine(MoveToTarget(initialPosition, 0f, 1f));
        rotateCoroutine = StartCoroutine(RotateToTarget(initialRotation, 1f));
    }

    /// <summary>
    /// ��������� ������� �������� �������� � �������� ������, ���� ��� ��������.
    /// </summary>
    private void StopActiveCoroutines()
    {
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        if (rotateCoroutine != null) StopCoroutine(rotateCoroutine);
    }

    /// <summary>
    /// ��������� �������� ������ � ��������� ��������.
    /// </summary>
    private IEnumerator RotateToTarget(Quaternion targetRotation, float duration)
    {
        Quaternion startRotation = userInputSettings.cameraTransform.rotation;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            userInputSettings.cameraTransform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
            yield return null;
        }
        userInputSettings.cameraTransform.rotation = targetRotation;
    }

    /// <summary>
    /// ��������� ����������� ������ � �������� �������.
    /// </summary>
    /// <param name="duration">������������ ��������.</param>
    private IEnumerator MoveToTarget(Vector3 targetPos, float distance, float duration)
    {
        Vector3 startPos = userInputSettings.cameraTransform.position;
        Vector3 direction = (startPos - targetPos).normalized;
        Vector3 endPos = targetPos + direction * distance;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            userInputSettings.cameraTransform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }
        userInputSettings.cameraTransform.position = endPos;
    }
}
