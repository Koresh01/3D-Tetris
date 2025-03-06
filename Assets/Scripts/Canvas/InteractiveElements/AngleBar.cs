using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Custom/AngleBar (ќтвечает за работу полосы прокрутки.)")]
public class AngleBar : MonoBehaviour
{
    [SerializeField, Tooltip("√абариты изображени€.")] private RectTransform imageRectTransform;
    [SerializeField, Tooltip("ќбласть полосы прокрутки.")] private RectTransform canvasRect;
    [SerializeField, Tooltip("ћножитель скорости вращени€ полосы прокрутки.")] private float rotationSpeed = 2f;

    [SerializeField, Tooltip("“екущий угол вращени€.")] private float currentRotation = 0f;
    private float maxRotation = 360f; // ћаксимальный угол (360 градусов)
    private float minRotation = 0f;   // ћинимальный угол (0 градусов)

    /// <summary>
    /// ќбновление позиции angle bar на основе угла вращени€ камеры.
    /// </summary>
    public void UpdateAngleBarPosition(float rotationDelta)
    {
        // ќбновл€ем текущий угол вращени€
        currentRotation += rotationDelta* rotationSpeed;

        // ќбеспечиваем цикличность угла (зацикливаем его на 360 градусов)
        if (currentRotation > maxRotation)
        {
            currentRotation -= maxRotation;
        }
        else if (currentRotation < minRotation)
        {
            currentRotation += maxRotation;
        }

        // ѕреобразуем угол в значение в пиксел€х дл€ смещени€
        float normalizedRotation = currentRotation / maxRotation;
        float angleBarWidth = imageRectTransform.rect.width;
        float newPosX = normalizedRotation * canvasRect.rect.width;

        // ”станавливаем позицию Image в зависимости от вычисленного значени€
        imageRectTransform.anchoredPosition = new Vector2(newPosX, imageRectTransform.anchoredPosition.y);

        // ƒублируем angle bar, если он выходит за границы Canvas
        if (imageRectTransform.anchoredPosition.x > canvasRect.rect.width)
        {
            imageRectTransform.anchoredPosition = new Vector2(newPosX - canvasRect.rect.width, imageRectTransform.anchoredPosition.y);
        }
        else if (imageRectTransform.anchoredPosition.x < 0)
        {
            imageRectTransform.anchoredPosition = new Vector2(newPosX + canvasRect.rect.width, imageRectTransform.anchoredPosition.y);
        }
    }
}
