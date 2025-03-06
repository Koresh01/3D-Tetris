using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Custom/AngleBar (�������� �� ������ ������ ���������.)")]
public class AngleBar : MonoBehaviour
{
    [SerializeField, Tooltip("�������� �����������.")] private RectTransform imageRectTransform;
    [SerializeField, Tooltip("������� ������ ���������.")] private RectTransform canvasRect;
    [SerializeField, Tooltip("��������� �������� �������� ������ ���������.")] private float rotationSpeed = 2f;

    [SerializeField, Tooltip("������� ���� ��������.")] private float currentRotation = 0f;
    private float maxRotation = 360f; // ������������ ���� (360 ��������)
    private float minRotation = 0f;   // ����������� ���� (0 ��������)

    /// <summary>
    /// ���������� ������� angle bar �� ������ ���� �������� ������.
    /// </summary>
    public void UpdateAngleBarPosition(float rotationDelta)
    {
        // ��������� ������� ���� ��������
        currentRotation += rotationDelta* rotationSpeed;

        // ������������ ����������� ���� (����������� ��� �� 360 ��������)
        if (currentRotation > maxRotation)
        {
            currentRotation -= maxRotation;
        }
        else if (currentRotation < minRotation)
        {
            currentRotation += maxRotation;
        }

        // ����������� ���� � �������� � �������� ��� ��������
        float normalizedRotation = currentRotation / maxRotation;
        float angleBarWidth = imageRectTransform.rect.width;
        float newPosX = normalizedRotation * canvasRect.rect.width;

        // ������������� ������� Image � ����������� �� ������������ ��������
        imageRectTransform.anchoredPosition = new Vector2(newPosX, imageRectTransform.anchoredPosition.y);

        // ��������� angle bar, ���� �� ������� �� ������� Canvas
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
