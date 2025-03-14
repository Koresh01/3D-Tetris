using UnityEngine;

/// <summary>
/// Обрабатывает ввод с мобильных устройств: вращение и зум камеры жестами.
/// </summary>
[AddComponentMenu("Custom/MobileInput (Обработка ввода с телефона)")]
public class MobileInput : CommonFunctions
{
    private Vector2 previousSingleTouchPos;
    private Vector2 previousTouch1;
    private Vector2 previousTouch2;
    private bool wasUsingTwoFingers = false;

    protected override void HandleInput()
    {
        int touchCount = Input.touchCount;

        if (touchCount == 1)
        {
            HandleSingleTouch(Input.GetTouch(0));
        }
        else if (touchCount == 2)
        {
            HandleTwoFingerTouch(Input.GetTouch(0), Input.GetTouch(1));
        }
    }

    /// <summary>
    /// Обрабатывает перемещение одним пальцем (вращение камеры).
    /// </summary>
    private void HandleSingleTouch(Touch touch)
    {
        if (wasUsingTwoFingers)
        {
            // Сброс предыдущей позиции, чтобы избежать резких скачков после двух пальцев.
            previousSingleTouchPos = touch.position;
            wasUsingTwoFingers = false;
        }

        if (touch.phase == TouchPhase.Began)
        {
            previousSingleTouchPos = touch.position;
        }
        else if (touch.phase == TouchPhase.Moved)
        {
            Vector2 delta = touch.position - previousSingleTouchPos;
            RotateCamera(delta);
            previousSingleTouchPos = touch.position;
        }
    }

    /// <summary>
    /// Обрабатывает жесты двумя пальцами (зум и вращение).
    /// </summary>
    private void HandleTwoFingerTouch(Touch touch1, Touch touch2)
    {
        if (!wasUsingTwoFingers)
        {
            // При первом касании двумя пальцами запоминаем позиции.
            previousTouch1 = touch1.position;
            previousTouch2 = touch2.position;
            wasUsingTwoFingers = true;
        }

        if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
        {
            float prevDistance = Vector2.Distance(previousTouch1, previousTouch2);
            float currDistance = Vector2.Distance(touch1.position, touch2.position);
            float zoomDelta = (currDistance - prevDistance) / (userInputSettings.zoomStep * 1.5f);
            ZoomCamera(zoomDelta);

            Vector2 averageDelta = (touch1.deltaPosition + touch2.deltaPosition) * 0.5f;
            RotateCamera(averageDelta / 2.2f);

            previousTouch1 = touch1.position;
            previousTouch2 = touch2.position;
        }
    }
}
