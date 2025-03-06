using UnityEngine;

public class ComputerInput : CameraController
{
    protected override void HandleInput()
    {
        if (Input.GetMouseButton(1))
        {
            float rotationX = Input.GetAxis("Mouse X") * rotationSpeed;
            float rotationY = Input.GetAxis("Mouse Y") * rotationSpeed;

            RotateCamera(new Vector2(rotationX, rotationY));
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            ZoomCamera(scroll * 10f);
        }
    }
}
