using UnityEngine;

public class CameraOrbitController : MonoBehaviour
{
    public Transform target; // ������� ������, ������ �������� ��������� ������
    public float rotationSpeed = 0.2f; // �������� �������� ������
    public float zoomSpeed = 0.5f; // �������� �����������/���������
    public float minDistance = 2f; // ����������� ���������� �� �������
    public float maxDistance = 10f; // ������������ ���������� �� �������

    private Vector2 previousTouch1;
    private Vector2 previousTouch2;
    private float distanceToTarget;

    private void Start()
    {
        if (target == null)
        {
            Debug.LogError("CameraOrbitController: �� �������� ������� ������!");
            return;
        }
        distanceToTarget = Vector3.Distance(transform.position, target.position);
    }

    private void Update()
    {
        if (Input.touchCount == 2) // ���������, ��� ������������� ��� ������
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            // ��������� ������� ������ ����������� ���� �������
            Vector2 delta1 = touch1.deltaPosition;
            Vector2 delta2 = touch2.deltaPosition;

            // ��������� ��������� ���������� ����� �������� ��� ����
            float prevDist = Vector2.Distance(previousTouch1, previousTouch2);
            float currDist = Vector2.Distance(touch1.position, touch2.position);
            float pinchDelta = currDist - prevDist;

            ZoomCamera(pinchDelta);

            // ��������� �������� ���� ������� ��� ��������
            Vector2 averageDelta = (delta1 + delta2) / 2;
            RotateCamera(averageDelta);

            // ��������� ���������� ������� �������
            previousTouch1 = touch1.position;
            previousTouch2 = touch2.position;
        }
    }

    private void ZoomCamera(float pinchDelta)
    {
        float zoomAmount = pinchDelta * zoomSpeed * Time.deltaTime;
        distanceToTarget = Mathf.Clamp(distanceToTarget - zoomAmount, minDistance, maxDistance);
        UpdateCameraPosition();
    }

    private void RotateCamera(Vector2 touchDelta)
    {
        float rotationX = touchDelta.x * rotationSpeed;
        float rotationY = -touchDelta.y * rotationSpeed;

        // ������� ������ ������ �������
        transform.RotateAround(target.position, Vector3.up, rotationX);
        transform.RotateAround(target.position, transform.right, rotationY);

        // ��������� ���������� ������ �� ���� ����� ��������
        distanceToTarget = Vector3.Distance(transform.position, target.position);
    }

    private void UpdateCameraPosition()
    {
        Vector3 direction = (transform.position - target.position).normalized;
        transform.position = target.position + direction * distanceToTarget;
    }
}
