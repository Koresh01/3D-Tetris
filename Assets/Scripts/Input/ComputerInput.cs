using UnityEngine;

public class ComputerInput : CameraController
{
    public float minVerticalAngle = -30f;
    public float maxVerticalAngle = 60f;
    private float verticalAngle = 0f;

    protected override void HandleInput()
    {
        if (Input.GetMouseButton(1))
        {
            float rotationX = Input.GetAxis("Mouse X") * rotationSpeed;
            float rotationY = Input.GetAxis("Mouse Y") * rotationSpeed;

            float newVerticalAngle = Mathf.Clamp(verticalAngle + rotationY, minVerticalAngle, maxVerticalAngle);
            float deltaAngle = newVerticalAngle - verticalAngle;
            verticalAngle = newVerticalAngle;

            RotateCamera(new Vector2(rotationX, deltaAngle));
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            ZoomCamera(scroll * 10f);
        }
    }
}
