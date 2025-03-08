using UnityEngine;
using UnityEngine.Events;
using System.Collections;

[AddComponentMenu("Custom/ClickEventInvoker (����������� ������� �� �������� �������� �������)")]
public class ClickEventInvoker : MonoBehaviour
{
    [SerializeField] private UnityEvent onClick;

    [Header("��������� ��������")]
    [SerializeField] private float scaleFactor = 1.2f; // ��������� ��������� ������
    [SerializeField] private float scaleSpeed = 5f;   // �������� ��������

    private Vector3 originalScale; // �������� ������ �������
    private Coroutine scaleCoroutine; // ������ �� ��������

    private void Start()
    {
        originalScale = transform.localScale; // ��������� ��������� ������
    }

    private void Update()
    {
#if UNITY_ANDROID
        if (Input.GetMouseButtonDown(0))
        {
            CheckClick(Input.mousePosition);
        }
#else
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                CheckClick(touch.position);
            }
        }
# endif
    }

    private void CheckClick(Vector2 screenPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        RaycastHit[] hits = Physics.RaycastAll(ray); // �������� ��� �����������

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject == gameObject)
            {
                if (scaleCoroutine != null)
                    StopCoroutine(scaleCoroutine);

                scaleCoroutine = StartCoroutine(AnimateScale());
                onClick?.Invoke();
                return; // �������� ������� � �������
            }
        }
    }

    /// <summary>
    /// ������� ��������� ���������� � ���������� ������� ��� �������.
    /// </summary>
    private IEnumerator AnimateScale()
    {
        Vector3 targetScale = originalScale * scaleFactor; // ����� ������ (�����������)

        // ����������
        yield return ScaleObject(targetScale);

        // ������� ����������� � ��������� �������
        yield return ScaleObject(originalScale);
    }

    /// <summary>
    /// ������ �������� ������ ������� �� �������� ��������.
    /// </summary>
    private IEnumerator ScaleObject(Vector3 targetScale)
    {
        while (Vector3.Distance(transform.localScale, targetScale) > 0.01f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, scaleSpeed * Time.deltaTime);
            yield return null;
        }

        transform.localScale = targetScale; // ����������� ������ ��������� � ������
    }
}
