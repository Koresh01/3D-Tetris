using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Custom/AngleBar (Отвечает за работу полосы прокрутки.)")]
public class AngleBar : MonoBehaviour
{
    private Vector2 previousTouchPosition; // Позиция касания на предыдущем кадре

    [SerializeField, Tooltip("Основная камера, управляемая этим скриптом.")]
    private Camera camera;

    [SerializeField, Tooltip("Скрипт управления камерой.")]
    private CameraController cameraController;

    [SerializeField, Tooltip("Область, фиксирующая касания.")]
    private RectTransform scrollArea;

    [SerializeField, Tooltip("Область изображения полосы.")]
    private RectTransform imageRect;

    [SerializeField, Tooltip("Ширина изображения в пикселях.")]
    private float imageWidth;

    private void Start()
    {
        imageWidth = imageRect.rect.width; // Запоминаем ширину изображения
    }

    private void Update()
    {
        HandleTouchInput();
    }

    /// <summary>
    /// Обрабатывает ввод от пользователя при касании экрана.
    /// </summary>
    private void HandleTouchInput()
    {
        if (Input.touchCount != 1) return; // Обрабатываем только одно касание

        Touch touch = Input.GetTouch(0);

        if (IsTouchWithinScrollArea(touch))
        {
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    previousTouchPosition = touch.position; // Запоминаем начальную позицию касания
                    break;
                case TouchPhase.Moved:
                    float pixelDeltaX = touch.position.x - previousTouchPosition.x; // Смещение по X в пикселях

                    // Основное действие:
                    cameraController.RotateCamera(new Vector2(pixelDeltaX, 0)); // Вращаем камеру

                    previousTouchPosition = touch.position; // Обновляем предыдущую позицию
                    break;
            }
        }
    }

    /// <summary>
    /// Проверяет, находится ли касание в пределах области полосы прокрутки.
    /// </summary>
    /// <param name="touch">Текущее касание.</param>
    /// <returns>True, если касание внутри области, иначе False.</returns>
    private bool IsTouchWithinScrollArea(Touch touch)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(scrollArea, touch.position, camera, out Vector2 localPoint))
        {
            return scrollArea.rect.Contains(localPoint);
        }
        return false;
    }

    /// <summary>
    /// Обновляет позицию полоски на основе смещения по оси X.
    /// </summary>
    /// <param name="pixelDeltaX">Смещение по X в пикселях.</param>
    public void UpdateAngleBarPosition(float pixelDeltaX)
    {
        float newPosX = imageRect.anchoredPosition.x + pixelDeltaX;

        // Зацикливаем движение, если полоска выходит за границы
        if (newPosX > imageWidth / 4f)
        {
            newPosX -= imageWidth / 4f;
        }
        else if (newPosX < -imageWidth / 4f)
        {
            newPosX += imageWidth / 4f;
        }

        imageRect.anchoredPosition = new Vector2(newPosX, imageRect.anchoredPosition.y);
    }
}
