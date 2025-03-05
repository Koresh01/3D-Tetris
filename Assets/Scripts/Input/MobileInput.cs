using UnityEngine;

public class MobileInput : CameraController
{
    // Поля для камеры и объекта, вокруг которого происходит вращение
    [SerializeField] private RectTransform scrollBarRect; // Ссылка на RectTransform полоски
    private Vector2 previousTouchPosition;

    // Ввод двумя пальцами:
    private Vector2 previousTouch1;
    private Vector2 previousTouch2;

    protected override void HandleInput()
    {
        if (Input.touchCount == 1) // Обрабатываем только один палец
        {
            Touch touch = Input.GetTouch(0);

            // Преобразуем экранные координаты касания в пространство канваса
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(scrollBarRect, touch.position, camera, out localPoint);

            // Проверяем, попадает ли локальная точка в область полоски
            if (scrollBarRect.rect.Contains(localPoint))
            {
                // Если движение пальца происходит по полоске
                if (touch.phase == TouchPhase.Moved)
                {
                    Vector2 deltaPosition = touch.position - previousTouchPosition;

                    // Изменяем угол вращения в зависимости от движения пальца
                    float currentAngle = deltaPosition.x * rotationSpeed * Time.deltaTime;

                    // Обновляем вращение камеры вокруг target
                    RotateCamera(new Vector2(currentAngle, 0));

                    // Обновляем предыдущую позицию касания
                    previousTouchPosition = touch.position;
                }
                else if (touch.phase == TouchPhase.Began)
                {
                    // Запоминаем начальную позицию касания
                    previousTouchPosition = touch.position;
                }
            }
        }

        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            // Проверяем, двигались ли пальцы
            if (touch1.phase == TouchPhase.Moved && touch2.phase == TouchPhase.Moved)
            {
                float prevDist = Vector2.Distance(previousTouch1, previousTouch2);
                float currDist = Vector2.Distance(touch1.position, touch2.position);
                float pinchDelta = currDist - prevDist;

                ZoomCamera(pinchDelta);
            }

            Vector2 averageDelta = (touch1.deltaPosition + touch2.deltaPosition) / 2;
            RotateCamera(averageDelta);

            previousTouch1 = touch1.position;
            previousTouch2 = touch2.position;
        }
    }


}
