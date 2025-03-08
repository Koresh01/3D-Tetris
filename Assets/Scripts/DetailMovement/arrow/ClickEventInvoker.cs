using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu("Custom/ClickEventInvoker (Обработчик нажатий)")]
public class ClickEventInvoker : MonoBehaviour
{
    [SerializeField] private UnityEvent onClick;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckClick(Input.mousePosition);
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                CheckClick(touch.position);
            }
        }
    }

    private void CheckClick(Vector2 screenPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        RaycastHit[] hits = Physics.RaycastAll(ray); // Получаем все пересечения

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject == gameObject)
            {
                onClick?.Invoke();
                return; // Вызываем событие и выходим
            }
        }
    }
}
