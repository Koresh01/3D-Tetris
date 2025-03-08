using UnityEngine;


[AddComponentMenu("Custom/FollowCamera (Сохраняет исходное ориентирование стрелочек)\"")]
class RotationSaver : MonoBehaviour
{
    private Quaternion initialRotation;

    void Start()
    {
        // Сохраняем изначальный поворот
        initialRotation = transform.rotation;
    }

    void Update()
    {
        // Оставляем начальный поворот
        transform.rotation = initialRotation;
    }
}
