using UnityEngine;

[AddComponentMenu("Custom/MobileInput (Обработка ввода с телефона)")]
public class MobileInput : CommonFunctions
{
    private Vector2 previousTouchPos;
    private Vector2 currentTouchPos;

    private Vector2 previousTouch1;
    private Vector2 previousTouch2;

    protected override void HandleInput()
    {
        // Обработчик одного пальца.
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                previousTouchPos = touch.position; // Устанавливаем стартовую позицию
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                Vector2 pixelDeltaX = touch.position - previousTouchPos;
                RotateCamera(pixelDeltaX);
                previousTouchPos = touch.position; // Обновляем позицию для следующего кадра
            }
        }

        // Обработчик двух пальцев.
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            if (touch1.phase == TouchPhase.Moved && touch2.phase == TouchPhase.Moved)
            {
                float prevDist = Vector2.Distance(previousTouch1, previousTouch2);
                float currDist = Vector2.Distance(touch1.position, touch2.position);
                float pixelDeltaX = currDist - prevDist;

                ZoomCamera(pixelDeltaX/(userInputSettings.zoomStep*1.5f));
            }

            if (touch1.phase == TouchPhase.Moved && touch2.phase == TouchPhase.Moved)
            {
                Vector2 averageDelta = (touch1.deltaPosition + touch2.deltaPosition);
                RotateCamera(averageDelta/2.2f);
            }

            previousTouch1 = touch1.position;
            previousTouch2 = touch2.position;
        }
    }
}
