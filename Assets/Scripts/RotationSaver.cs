using UnityEngine;

/// <summary>
/// Сохраняет заданное мировое направление объекта, игнорируя вращение родителя.
/// </summary>
[AddComponentMenu("Custom/RotationSaver (Сохраняет заданное ориентирование)")]
class RotationSaver : MonoBehaviour
{
    [Tooltip("Глобальный поворот, который объект должен сохранять.")]
    [SerializeField] private Quaternion worldRotation = Quaternion.identity; // Значение по умолчанию (без вращения)

    void Update()
    {
        // Применяем заданный мировой поворот, чтобы объект не вращался вместе с родителем
        transform.rotation = worldRotation;
    }
}
