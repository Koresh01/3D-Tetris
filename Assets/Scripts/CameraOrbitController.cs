using UnityEngine;

public class CameraOrbitController : MonoBehaviour
{
    public Transform target; // Целевой объект, вокруг которого вращается камера
    public float rotationSpeed = 0.2f; // Скорость вращения камеры
    public float zoomSpeed = 0.5f; // Скорость приближения/отдаления
    public float minDistance = 2f; // Минимальное расстояние до объекта
    public float maxDistance = 10f; // Максимальное расстояние до объекта

    private Vector2 previousTouch1;
    private Vector2 previousTouch2;
    private float distanceToTarget;

    private void Start()
    {
        if (target == null)
        {
            Debug.LogError("CameraOrbitController: Не назначен целевой объект!");
            return;
        }
        distanceToTarget = Vector3.Distance(transform.position, target.position);
    }

    private void Update()
    {
        if (Input.touchCount == 2) // Проверяем, что задействованы два пальца
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            // Вычисляем средний вектор перемещения двух пальцев
            Vector2 delta1 = touch1.deltaPosition;
            Vector2 delta2 = touch2.deltaPosition;

            // Проверяем изменение расстояния между пальцами для зума
            float prevDist = Vector2.Distance(previousTouch1, previousTouch2);
            float currDist = Vector2.Distance(touch1.position, touch2.position);
            float pinchDelta = currDist - prevDist;

            ZoomCamera(pinchDelta);

            // Проверяем движение двух пальцев для вращения
            Vector2 averageDelta = (delta1 + delta2) / 2;
            RotateCamera(averageDelta);

            // Сохраняем предыдущие позиции пальцев
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

        // Вращаем камеру вокруг объекта
        transform.RotateAround(target.position, Vector3.up, rotationX);
        transform.RotateAround(target.position, transform.right, rotationY);

        // Обновляем расстояние камеры до цели после вращения
        distanceToTarget = Vector3.Distance(transform.position, target.position);
    }

    private void UpdateCameraPosition()
    {
        Vector3 direction = (transform.position - target.position).normalized;
        transform.position = target.position + direction * distanceToTarget;
    }
}
