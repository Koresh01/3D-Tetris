using UnityEngine;

[AddComponentMenu("Custom/ComputerInput (Обработка ввода с компьютера)")]
public class ComputerInput : CommonFunctions
{
    private Vector2 previousMousePosition;

    protected override void HandleInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            previousMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(1))
        {
            Vector2 currentMousePosition = Input.mousePosition;
            Vector2 pixelDeltaX = currentMousePosition - previousMousePosition;

            RotateCamera(pixelDeltaX);
            previousMousePosition = currentMousePosition;
        }

        float scroll = Input.mouseScrollDelta.y;
        if (scroll != 0f)
        {
            ZoomCamera(scroll);
        }
    }
}
