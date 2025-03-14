using System.Collections;
using UnityEngine;

/// <summary>
/// Абстрактный контроллер камеры, содержащий основные методы для управления камерой,
/// такие как вращение, масштабирование и позиционирование.
/// Этот класс следует расширять для реализации управления на разных устройствах.
/// </summary>
[AddComponentMenu("Custom/CommonFunctions (Хранит все необходимые методы, для реализации управления камерой)")]
public abstract class CommonFunctions : MonoBehaviour
{
    [SerializeField, Tooltip("Настройки камеры, чтобы избежать дублирования параметров.")]
    protected UserInputSettings userInputSettings;

    /// <summary>
    /// Текущее расстояние от камеры до целевого объекта.
    /// Используется при масштабировании (зуме) камеры.
    /// </summary>
    protected float distanceToTarget;
    /// <summary>
    /// Изменяет расстояние между камерой и целевым объектом (зум).
    /// </summary>
    /// <param name="pinchDelta">Разница в расстоянии между пальцами (для сенсорного ввода) 
    /// или величина прокрутки колесика мыши.</param>
    protected void ZoomCamera(float pinchDelta)
    {
        float zoomAmount = pinchDelta * userInputSettings.zoomStep * Time.deltaTime;
        distanceToTarget = Mathf.Clamp(distanceToTarget - zoomAmount, userInputSettings.minDistance, userInputSettings.maxDistance);
        UpdateCameraPosition();
    }

    /// <summary>
    /// Вращает камеру вокруг целевого объекта с учетом ограничений по вертикальному углу.
    /// </summary>
    public void RotateCamera(Vector2 rotationDelta)
    {
        float pixelDeltaX = rotationDelta.x;
        float pixelDeltaY = rotationDelta.y;

        Transform cameraTransform = userInputSettings.cameraTransform;
        Transform target = userInputSettings.target;

        // Вращение по горизонтали (вокруг оси Y)
        cameraTransform.RotateAround(target.position, Vector3.up, pixelDeltaX * userInputSettings.rotationSpeed);

        // Получаем текущее направление камеры относительно цели
        Vector3 directionToCamera = (cameraTransform.position - target.position).normalized;

        // Вычисляем угол наклона камеры относительно оси Y (в градусах)
        float currentAngle = Mathf.Asin(directionToCamera.y) * Mathf.Rad2Deg;

        // Ограничиваем новый угол
        float clampedAngle = Mathf.Clamp(currentAngle - pixelDeltaY * userInputSettings.rotationSpeed,
                                         userInputSettings.minVerticalAngle, userInputSettings.maxVerticalAngle);

        // Вычисляем разницу углов
        float deltaAngle = clampedAngle - currentAngle;

        // Вращаем камеру вокруг оси X
        cameraTransform.RotateAround(target.position, cameraTransform.right, deltaAngle);

        // Обновляем дистанцию до целевого объекта
        distanceToTarget = Vector3.Distance(cameraTransform.position, target.position);
    }

    /// <summary>
    /// Обновляет позицию камеры, сохраняя корректное расстояние до целевого объекта.
    /// </summary>
    protected void UpdateCameraPosition()
    {
        Vector3 direction = (userInputSettings.cameraTransform.position - userInputSettings.target.position).normalized;
        userInputSettings.cameraTransform.position = userInputSettings.target.position + direction * distanceToTarget;
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
