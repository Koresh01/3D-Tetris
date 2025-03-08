using UnityEngine;
using UnityEngine.Events;
using System.Collections;

[AddComponentMenu("Custom/ClickEventInvoker (Отлавливает нажатия на колайдер текущего объекта)")]
public class ClickEventInvoker : MonoBehaviour
{
    [SerializeField] private UnityEvent onClick;

    [Header("Настройки анимации")]
    [SerializeField] private float scaleFactor = 1.2f; // Насколько увеличить объект
    [SerializeField] private float scaleSpeed = 5f;   // Скорость анимации

    private Vector3 originalScale; // Исходный размер объекта
    private Coroutine scaleCoroutine; // Ссылка на корутину

    private void Start()
    {
        originalScale = transform.localScale; // Сохраняем начальный размер
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
        RaycastHit[] hits = Physics.RaycastAll(ray); // Получаем все пересечения

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject == gameObject)
            {
                if (scaleCoroutine != null)
                    StopCoroutine(scaleCoroutine);

                scaleCoroutine = StartCoroutine(AnimateScale());
                onClick?.Invoke();
                return; // Вызываем событие и выходим
            }
        }
    }

    /// <summary>
    /// Корутин анимирует увеличение и уменьшение объекта при нажатии.
    /// </summary>
    private IEnumerator AnimateScale()
    {
        Vector3 targetScale = originalScale * scaleFactor; // Новый размер (увеличенный)

        // Увеличение
        yield return ScaleObject(targetScale);

        // Плавное возвращение к исходному размеру
        yield return ScaleObject(originalScale);
    }

    /// <summary>
    /// Плавно изменяет размер объекта до целевого значения.
    /// </summary>
    private IEnumerator ScaleObject(Vector3 targetScale)
    {
        while (Vector3.Distance(transform.localScale, targetScale) > 0.01f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, scaleSpeed * Time.deltaTime);
            yield return null;
        }

        transform.localScale = targetScale; // Гарантируем точное попадание в размер
    }
}
