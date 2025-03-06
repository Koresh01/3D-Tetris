using UnityEngine;

/// <summary>
/// Абстрактный контроллер камеры, обеспечивающий базовые механики вращения, масштабирования и позиционирования.
/// Должен быть унаследован для реализации специфичных методов управления для разных платформ.
/// </summary>
public abstract class CameraController : MonoBehaviour
{
    [SerializeField, Tooltip("Основная камера, управляемая этим скриптом.")]
    protected Camera camera;

    [SerializeField, Tooltip("Шкала градусов поворота камеры.")]
    protected AngleBar angleBar;

    [SerializeField, Tooltip("Ссылка на GameManager, используемый для получения параметров игрового поля.")]
    protected GameManager gameManager;

    [Tooltip("Целевой объект, вокруг которого вращается камера.")]
    public Transform target;

    [Tooltip("Скорость вращения камеры вокруг целевого объекта.")]
    public float rotationSpeed = 4f;

    [Tooltip("Скорость приближения/отдаления камеры.")]
    public float zoomSpeed = 0.5f;

    [Tooltip("Минимальное расстояние до целевого объекта.")]
    public float minDistance = 2f;

    [Tooltip("Максимальное расстояние до целевого объекта.")]
    public float maxDistance = 10f;

    [Tooltip("Минимальный угол наклона камеры (по вертикали).")]
    public float minVerticalAngle = -30f;

    [Tooltip("Максимальный угол наклона камеры (по вертикали).")]
    public float maxVerticalAngle = 60f;

    /// <summary>
    /// Текущее расстояние от камеры до целевого объекта.
    /// </summary>
    protected float distanceToTarget;

    /// <summary>
    /// Текущий вертикальный угол поворота камеры.
    /// </summary>
    private float currentVerticalAngle = 0f;

    /// <summary>
    /// Инициализация камеры.
    /// </summary>
    protected virtual void Start()
    {
        camera.transform.LookAt(target.transform);
        distanceToTarget = Vector3.Distance(camera.transform.position, target.position);
    }

    /// <summary>
    /// Масштабирует камеру, приближая или отдаляя её от целевого объекта.
    /// </summary>
    protected void ZoomCamera(float pinchDelta)
    {
        float zoomAmount = pinchDelta * zoomSpeed * Time.deltaTime;
        distanceToTarget = Mathf.Clamp(distanceToTarget - zoomAmount, minDistance, maxDistance);
        UpdateCameraPosition();
    }

    /// <summary>
    /// Вращает камеру вокруг целевого объекта с ограничением вертикального угла.
    /// </summary>
    public void RotateCamera(Vector2 rotationDelta)
    {
        float pixelDeltaX = rotationDelta.x;
        float pixelDeltaY = rotationDelta.y;

        // Вращение по горизонтали (вокруг оси Y)
        camera.transform.RotateAround(target.position, Vector3.up, pixelDeltaX * rotationSpeed);

        // Ограничение вертикального вращения
        float newVerticalAngle = Mathf.Clamp(currentVerticalAngle - pixelDeltaY, minVerticalAngle, maxVerticalAngle);
        float deltaAngle = newVerticalAngle - currentVerticalAngle;
        currentVerticalAngle = newVerticalAngle;

        // Вращение по вертикали (вокруг оси X)
        camera.transform.RotateAround(target.position, camera.transform.right, deltaAngle * rotationSpeed);

        // Обновление дистанции до целевого объекта
        distanceToTarget = Vector3.Distance(camera.transform.position, target.position);

        // Обновляем шкалу угла поворота
        angleBar.UpdateAngleBarPosition(pixelDeltaX);
    }

    /// <summary>
    /// Обновляет позицию камеры, сохраняя корректное расстояние до целевого объекта.
    /// </summary>
    protected void UpdateCameraPosition()
    {
        Vector3 direction = (camera.transform.position - target.position).normalized;
        camera.transform.position = target.position + direction * distanceToTarget;
    }

    /// <summary>
    /// Абстрактный метод для обработки пользовательского ввода.
    /// </summary>
    protected abstract void HandleInput();

    private void Update()
    {
        HandleInput();
    }
}
