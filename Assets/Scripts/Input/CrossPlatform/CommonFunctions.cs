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

    private Transform startedCameraTransform;

    private void OnEnable()
    {
        if (cameraSettings == null)
        {
            Debug.LogError("CameraSettings не установлены в " + gameObject.name);
            return;
        }

        // Запоминаем изначальную позицию и поворот камеры:
        startedCameraTransform = new GameObject("TempCamTransform").transform;
        startedCameraTransform.position = cameraSettings.cameraTransform.position;
        startedCameraTransform.rotation = cameraSettings.cameraTransform.rotation;

        // Перемещаем и поворачиваем камеру к целевой позиции:
        StartCoroutine(MoveToTarget(cameraSettings.cameraTransform.position, cameraSettings.target.position, 20f, 1f));
        StartCoroutine(RotateToTarget(cameraSettings.cameraTransform.rotation,
            Quaternion.LookRotation(cameraSettings.target.position + Vector3.up * GameManager.gridHeight / 3f - cameraSettings.cameraTransform.position), 1f));
    }

    private void OnDisable()
    {
        // Возвращаем камеру в исходное положение и поворот:
        StartCoroutine(MoveToTarget(cameraSettings.cameraTransform.position, startedCameraTransform.position, 0f, 1f));
        StartCoroutine(RotateToTarget(cameraSettings.cameraTransform.rotation, startedCameraTransform.rotation, 1f));

        // Удаляем временный объект, если он существует
        if (startedCameraTransform != null)
            Destroy(startedCameraTransform.gameObject);
    }

    /// <summary>
    /// Вращает камеру к заданному повороту.
    /// </summary>
    IEnumerator RotateToTarget(Quaternion startRotation, Quaternion targetRotation, float duration)
    {
        float progress = 0f;
        while (progress < duration)
        {
            progress += Time.deltaTime;
            float t = progress / duration;
            cameraSettings.cameraTransform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
            yield return null;
        }
        cameraSettings.cameraTransform.rotation = targetRotation;
    }

    /// <summary>
    /// Перемещает камеру к заданной позиции с учетом дистанции.
    /// </summary>
    IEnumerator MoveToTarget(Vector3 startPos, Vector3 targetPos, float distance, float duration)
    {
        Vector3 direction = (startPos - targetPos).normalized;
        Vector3 endPos = targetPos + direction * distance;

        float progress = 0f;
        while (progress < duration)
        {
            progress += Time.deltaTime;
            float t = progress / duration;
            cameraSettings.cameraTransform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }
        cameraSettings.cameraTransform.position = endPos;
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
    public void RotateCamera(Vector2 rotationDelta)
    {
        float pixelDeltaX = rotationDelta.x;
        float pixelDeltaY = rotationDelta.y;

        Transform cameraTransform = cameraSettings.cameraTransform;
        Transform target = cameraSettings.target;

        // Вращение по горизонтали (вокруг оси Y)
        cameraTransform.RotateAround(target.position, Vector3.up, pixelDeltaX * cameraSettings.rotationSpeed);

        // Получаем текущее направление камеры относительно цели
        Vector3 directionToCamera = (cameraTransform.position - target.position).normalized;

        // Вычисляем угол наклона камеры относительно оси Y (в градусах)
        float currentAngle = Mathf.Asin(directionToCamera.y) * Mathf.Rad2Deg;

        // Ограничиваем новый угол
        float clampedAngle = Mathf.Clamp(currentAngle - pixelDeltaY * cameraSettings.rotationSpeed,
                                         cameraSettings.minVerticalAngle, cameraSettings.maxVerticalAngle);

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
        Vector3 direction = (cameraSettings.cameraTransform.position - cameraSettings.target.position).normalized;
        cameraSettings.cameraTransform.position = cameraSettings.target.position + direction * distanceToTarget;
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
