using UnityEngine;

class ComputerInput : MonoBehaviour
{
    [SerializeField] Camera camera;

    public Transform target; // Целевой объект, вокруг которого вращается камера
    public float rotationSpeed = 0.2f; // Скорость вращения камеры
    public float zoomSpeed = 0.5f; // Скорость приближения/отдаления
    public float minDistance = 2f; // Минимальное расстояние до объекта
    public float maxDistance = 10f; // Максимальное расстояние до объекта
    public float minVerticalAngle = -30f; // Минимальный угол вертикального вращения
    public float maxVerticalAngle = 60f;  // Максимальный угол вертикального вращения

    private float verticalAngle = 0f; // Текущий угол вертикального вращения
    private float distanceToTarget;

    private void Start()
    {
        if (target == null)
        {
            Debug.LogError("CameraOrbitController: Не назначен целевой объект!");
            return;
        }
        distanceToTarget = Vector3.Distance(camera.transform.position, target.position);
    }

    private void Update()
    {
        if (Input.GetMouseButton(1)) // Правая кнопка мыши
        {
            float rotationX = Input.GetAxis("Mouse X") * rotationSpeed;
            float rotationY = -Input.GetAxis("Mouse Y") * rotationSpeed;

            // Ограничиваем вертикальное вращение
            float newVerticalAngle = Mathf.Clamp(verticalAngle + rotationY, minVerticalAngle, maxVerticalAngle);
            float deltaAngle = newVerticalAngle - verticalAngle;
            verticalAngle = newVerticalAngle;

            // Вращаем камеру
            camera.transform.RotateAround(target.position, Vector3.up, rotationX);
            camera.transform.RotateAround(target.position, camera.transform.right, deltaAngle);

            // Обновляем расстояние камеры до цели после вращения
            distanceToTarget = Vector3.Distance(camera.transform.position, target.position);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            ZoomCamera(scroll * 10f); // Ускоряем эффект колеса мыши
        }
    }

    private void ZoomCamera(float pinchDelta)
    {
        float zoomAmount = pinchDelta * zoomSpeed * Time.deltaTime;
        distanceToTarget = Mathf.Clamp(distanceToTarget - zoomAmount, minDistance, maxDistance);
        UpdateCameraPosition();
    }

    private void UpdateCameraPosition()
    {
        Vector3 direction = (camera.transform.position - target.position).normalized;
        camera.transform.position = target.position + direction * distanceToTarget;
    }
}
