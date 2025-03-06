using UnityEngine;

/// <summary>
/// Абстрактный контроллер камеры, содержащий основные методы для управления камерой,
/// такие как вращение, масштабирование и позиционирование.
/// Этот класс следует расширять для реализации управления на разных устройствах.
/// </summary>
[AddComponentMenu("Custom/CommonFunctions (Хранит все необходимые методы, для реализации управления камерой)")]
public abstract class CommonFunctions : MonoBehaviour
{
    /// <summary>
    /// Настройки камеры, включающие ссылки на основные объекты и параметры.
    /// Используется для избежания дублирования параметров в разных скриптах.
    /// </summary>
    [SerializeField, Tooltip("Настройки камеры, чтобы избежать дублирования параметров.")]
    protected InputSettings cameraSettings;

    /// <summary>
    /// Текущее расстояние от камеры до целевого объекта.
    /// Используется при масштабировании (зуме) камеры.
    /// </summary>
    protected float distanceToTarget;

    /// <summary>
    /// Текущий угол наклона камеры по вертикали.
    /// Используется для ограничения вертикального вращения.
    /// </summary>
    private float currentVerticalAngle = 0f;

    /// <summary>
    /// Инициализация параметров камеры при старте.
    /// Устанавливает камеру в начальное положение и рассчитывает дистанцию до цели.
    /// </summary>
    protected virtual void Start()
    {
        if (cameraSettings == null)
        {
            Debug.LogError("CameraSettings не установлены в " + gameObject.name);
            return;
        }
        // Определяем куда будет смотреть камера:
        Vector3 lookToTransform = cameraSettings.target.position + Vector3.up * GameManager.gridHeight / 3f;
        cameraSettings.camera.transform.LookAt(lookToTransform);
        distanceToTarget = Vector3.Distance(cameraSettings.camera.transform.position, cameraSettings.target.position);
    }

    /// <summary>
    /// Изменяет расстояние между камерой и целевым объектом (зум).
    /// </summary>
    /// <param name="pinchDelta">Разница в расстоянии между пальцами (для сенсорного ввода) 
    /// или величина прокрутки колесика мыши.</param>
    protected void ZoomCamera(float pinchDelta)
    {
        float zoomAmount = pinchDelta * cameraSettings.zoomStep * Time.deltaTime;
        distanceToTarget = Mathf.Clamp(distanceToTarget - zoomAmount, cameraSettings.minDistance, cameraSettings.maxDistance);
        UpdateCameraPosition();
    }

    /// <summary>
    /// Вращает камеру вокруг целевого объекта с учетом ограничений по вертикальному углу.
    /// </summary>
    /// <param name="rotationDelta">Вектор изменения вращения камеры (например, смещение мыши или жеста на сенсорном экране).</param>
    public void RotateCamera(Vector2 rotationDelta)
    {
        float pixelDeltaX = rotationDelta.x;
        float pixelDeltaY = rotationDelta.y;

        Camera cam = cameraSettings.camera;
        Transform target = cameraSettings.target;

        // Вращение по горизонтали (вокруг оси Y)
        cam.transform.RotateAround(target.position, Vector3.up, pixelDeltaX * cameraSettings.rotationSpeed);

        // Ограничение вертикального вращения
        float newVerticalAngle = Mathf.Clamp(currentVerticalAngle - pixelDeltaY * cameraSettings.rotationSpeed,
                                             cameraSettings.minVerticalAngle, cameraSettings.maxVerticalAngle);

        float deltaAngle = newVerticalAngle - currentVerticalAngle;
        currentVerticalAngle = newVerticalAngle;

        // Вращение по вертикали (вокруг оси X)
        cam.transform.RotateAround(target.position, cam.transform.right, deltaAngle);

        // Обновление дистанции до целевого объекта
        distanceToTarget = Vector3.Distance(cam.transform.position, target.position);

        // Обновляем шкалу угла поворота
        cameraSettings.angleBar.UpdateAngleBarPosition(pixelDeltaX);
    }


    /// <summary>
    /// Обновляет позицию камеры, сохраняя корректное расстояние до целевого объекта.
    /// </summary>
    protected void UpdateCameraPosition()
    {
        Vector3 direction = (cameraSettings.camera.transform.position - cameraSettings.target.position).normalized;
        cameraSettings.camera.transform.position = cameraSettings.target.position + direction * distanceToTarget;
    }

    /// <summary>
    /// Абстрактный метод для обработки пользовательского ввода.
    /// Должен быть реализован в дочерних классах для различных платформ (мобильные устройства, ПК и т. д.).
    /// </summary>
    protected abstract void HandleInput();

    /// <summary>
    /// Вызывает обработку пользовательского ввода на каждом кадре.
    /// </summary>
    private void Update()
    {
        HandleInput();
    }
}
