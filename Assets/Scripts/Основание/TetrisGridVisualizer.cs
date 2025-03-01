#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

/// <summary>
/// Отрисовывает границы игрового пространства, с помощью линий.
/// </summary>
public class TetrisGridVisualizer : MonoBehaviour
{
    [SerializeField] GameManager gameManager;

    [Tooltip("Ширина игровой области")]
    public int gridWidth;

    [Tooltip("Высота игровой области")]
    public int gridHeight;

    [Tooltip("Цвет линий")]
    [SerializeField] Color lineColor = Color.green; // Цвет линий


    private void OnValidate()
    {
        if (gameManager != null)
        {
            gridWidth = gameManager.gridWidth;
            gridHeight = gameManager.gridHeight;
        }
    }

    private void OnDrawGizmos()
    {
        Handles.color = lineColor;

        // Нижняя граница
        DrawRectangle(Vector3.up/2, gridWidth);

        // Верхняя граница
        DrawRectangle(Vector3.up * gridHeight, gridWidth);

        // Вертикальные линии
        DrawVerticalLines(gridWidth, gridHeight);
    }

    /// <summary>
    /// Рисует прямоугольник (основание или верхнюю границу)
    /// </summary>
    private void DrawRectangle(Vector3 position, float width)
    {
        Vector3 bottomLeft = position + new Vector3(-width / 2, 0, -width / 2);
        Vector3 bottomRight = position + new Vector3(width / 2, 0, -width / 2);
        Vector3 topLeft = position + new Vector3(-width / 2, 0, width / 2);
        Vector3 topRight = position + new Vector3(width / 2, 0, width / 2);

        Handles.DrawLine(bottomLeft, bottomRight);
        Handles.DrawLine(bottomRight, topRight);
        Handles.DrawLine(topRight, topLeft);
        Handles.DrawLine(topLeft, bottomLeft);
    }

    /// <summary>
    /// Рисует вертикальные линии по углам игрового поля с учетом подъема на yOffset
    /// </summary>
    private void DrawVerticalLines(float width, float gridHeight)
    {
        Vector3 bottomLeft = new Vector3(-width / 2, 0, -width / 2);
        Vector3 bottomRight = new Vector3(width / 2, 0, -width / 2);
        Vector3 topLeft = new Vector3(-width / 2, 0, width / 2);
        Vector3 topRight = new Vector3(width / 2, 0, width / 2);

        Vector3 up = Vector3.up * gridHeight;

        Handles.DrawLine(bottomLeft, bottomLeft + up);
        Handles.DrawLine(bottomRight, bottomRight + up);
        Handles.DrawLine(topLeft, topLeft + up);
        Handles.DrawLine(topRight, topRight + up);
    }
}
#endif
