using UnityEngine;

/// <summary>
/// Абстрактный контроллер камеры, обеспечивающий базовые механики вращения, масштабирования и позиционирования.
/// Должен быть унаследован для реализации специфичных методов управления для разных платформ.
/// </summary>
public abstract class CameraController : MonoBehaviour
{
    [SerializeField, Tooltip("Основная камера, управляемая этим скриптом.")]
    protected Camera camera;

    [SerializeField, Tooltip("Ссылка на GameManager, используемый для получения параметров игрового поля.")]
    protected GameManager gameManager;

    [Tooltip("Целевой объект, вокруг которого вращается камера.")]
    public Transform target;

    [Tooltip("Скорость вращения камеры вокруг целевого объекта.")]
    public float rotationSpeed = 0.2f;

    [Tooltip("Скорость приближения/отдаления камеры.")]
    public float zoomSpeed = 0.5f;

    [Tooltip("Минимальное расстояние до целевого объекта.")]
    public float minDistance = 2f;

    [Tooltip("Максимальное расстояние до целевого объекта.")]
    public float maxDistance = 10f;

    /// <summary>
    /// Текущее расстояние от камеры до целевого объекта.
    /// </summary>
    protected float distanceToTarget;

    /// <summary>
    /// Инициализация камеры. Вычисляет начальное положение целевого объекта и устанавливает дистанцию камеры.
    /// </summary>
    protected virtual void Start()
    {
        // Определяем куда будет смотреть камера в самом начале:
        Vector3 start = new Vector3(-0.5f, 0, -0.5f);
        int gridWidth = gameManager.gridWidth;
        target.transform.position = start + new Vector3(gridWidth / 2f, 0, gridWidth / 2f);

        // Заставляем камеру смотреть на эту точку:
        camera.transform.LookAt(target.transform);

        // Устанавливаем начальное расстояние камеры до целевого объекта.
        distanceToTarget = Vector3.Distance(camera.transform.position, target.position);
    }

    /// <summary>
    /// Масштабирует камеру, приближая или отдаляя её от целевого объекта.
    /// </summary>
    /// <param name="pinchDelta">Разница в дистанции ввода (например, скролл мыши или жест сжатия).</param>
    protected void ZoomCamera(float pinchDelta)
    {
        float zoomAmount = pinchDelta * zoomSpeed * Time.deltaTime;
        distanceToTarget = Mathf.Clamp(distanceToTarget - zoomAmount, minDistance, maxDistance);
        UpdateCameraPosition();
    }

    /// <summary>
    /// Вращает камеру вокруг целевого объекта.
    /// </summary>
    /// <param name="rotationDelta">Изменение угла вращения (обычно на основе пользовательского ввода).</param>
    protected void RotateCamera(Vector2 rotationDelta)
    {
        // Вращение по горизонтали (вокруг оси Y)
        camera.transform.RotateAround(target.position, Vector3.up, rotationDelta.x * rotationSpeed);

        // Вращение по вертикали (вокруг оси X)
        camera.transform.RotateAround(target.position, camera.transform.right, -rotationDelta.y * rotationSpeed);

        // Обновление дистанции до целевого объекта после вращения
        distanceToTarget = Vector3.Distance(camera.transform.position, target.position);
    }

    /// <summary>
    /// Обновляет позицию камеры, сохраняя корректное расстояние до целевого объекта.
    /// </summary>
    protected void UpdateCameraPosition()
    {
        // Направление от целевого объекта к камере
        Vector3 direction = (camera.transform.position - target.position).normalized;

        // Устанавливаем новую позицию камеры, сохраняя расстояние
        camera.transform.position = target.position + direction * distanceToTarget;
    }

    /// <summary>
    /// Абстрактный метод для обработки пользовательского ввода, реализуется в наследниках.
    /// </summary>
    protected abstract void HandleInput();

    /// <summary>
    /// Вызывается каждый кадр. Обрабатывает пользовательский ввод.
    /// </summary>
    private void Update()
    {
        HandleInput(); // Вызов метода обработки ввода, реализованного в наследниках.
    }
}
