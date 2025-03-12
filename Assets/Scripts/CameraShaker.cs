using UnityEngine;
using System.Collections;

[AddComponentMenu("Custom/CameraShaker (������ ������)")]
public class CameraShaker : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    [Tooltip("������, ��������� �������� ������:")]
    [SerializeField] private DetailRotator detailRotator;
    [Tooltip("������, ��������� �������� ������:")]
    [SerializeField] private DetailMover detailMover;

    [Header("��������� ������")]
    [SerializeField] private float shakeDuration = 0.3f;  // ������������ ������
    [SerializeField] private float shakeMagnitude = 0.2f; // ��������� ������

    private Coroutine shakeCoroutine;
    private Vector3 originalPosition;

    private void OnEnable()
    {
        detailMover.OnCanNotMove += ShakeCamera;
        detailRotator.OnCanNotRotate += ShakeCamera;
    }

    private void OnDisable()
    {
        detailMover.OnCanNotMove -= ShakeCamera;
        detailRotator.OnCanNotRotate -= ShakeCamera;
    }

    /// <summary>
    /// ������ ������.
    /// </summary>
    private void ShakeCamera()
    {
        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);

        shakeCoroutine = StartCoroutine(ShakeRoutine());
    }

    private IEnumerator ShakeRoutine()
    {
        originalPosition = _camera.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            elapsed += Time.deltaTime;

            float x = (Mathf.PerlinNoise(Time.time * 10f, 0f) - 0.5f) * shakeMagnitude;
            float y = (Mathf.PerlinNoise(0f, Time.time * 10f) - 0.5f) * shakeMagnitude;

            _camera.transform.localPosition = originalPosition + new Vector3(x, y, 0f);
            yield return null;
        }

        _camera.transform.localPosition = originalPosition;
    }
}
