using UnityEngine;

public class MobileInput : CameraController
{
    private Vector2 previousTouch1;
    private Vector2 previousTouch2;

    protected override void HandleInput()
    {

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
