using UnityEngine;
using System.Collections;

[AddComponentMenu("Custom/CameraMover ( онтроллер перемещени€ камеры)")]
public class CameraMover : MonoBehaviour
{
    [SerializeField, Tooltip("Ќастройки камеры, чтобы избежать дублировани€ параметров.")]
    protected UserInputSettings userInputSettings;

    [SerializeField, Tooltip("ѕервоначальное положение камеры.")]
    private Transform startedCameraTransform;

    private void Start()
    {
        // «апоминаем изначальную позицию и поворот камеры:
        startedCameraTransform = new GameObject("TempCamTransform").transform;
        startedCameraTransform.position = userInputSettings.cameraTransform.position;
        startedCameraTransform.rotation = userInputSettings.cameraTransform.rotation;
    }

    /// <summary>
    /// ѕередвижение камеры в режим "игры"
    /// </summary>
    public void setPlayMode()
    {
        // ѕеремещаем и поворачиваем камеру к целевой позиции:
        StartCoroutine(MoveToTarget(userInputSettings.cameraTransform.position, userInputSettings.target.position, 20f, 1f));
        StartCoroutine(RotateToTarget(userInputSettings.cameraTransform.rotation,
        Quaternion.LookRotation(userInputSettings.target.position + Vector3.up * GameManager.gridHeight / 3f - userInputSettings.cameraTransform.position), 1f));
    }

    /// <summary>
    /// ѕередвижение камеры в режим "меню"
    /// </summary>
    public void setMenuMode()
    {
        StartCoroutine(MoveToTarget(userInputSettings.cameraTransform.position, startedCameraTransform.position, 0f, 1f));
        StartCoroutine(RotateToTarget(userInputSettings.cameraTransform.rotation, startedCameraTransform.rotation, 1f));
    }

    /// <summary>
    /// ¬ращает камеру к заданному повороту.
    /// </summary>
    IEnumerator RotateToTarget(Quaternion startRotation, Quaternion targetRotation, float duration)
    {
        float progress = 0f;
        while (progress < duration)
        {
            progress += Time.deltaTime;
            float t = progress / duration;
            userInputSettings.cameraTransform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
            yield return null;
        }
        userInputSettings.cameraTransform.rotation = targetRotation;
    }

    /// <summary>
    /// ѕеремещает камеру к заданной позиции с учетом дистанции.
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
            userInputSettings.cameraTransform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }
        userInputSettings.cameraTransform.position = endPos;
    }
}
