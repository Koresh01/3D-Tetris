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

    [Tooltip("Цвет линий.")]
    [SerializeField] Color lineColor = Color.blue;

    [Tooltip("Точка отссчета")]
    private Vector3 start;

    private void OnValidate()
    {
        start = new Vector3(- 0.5f, 0, - 0.5f);   // где целесообразнее это делать?

        if (gameManager != null)
        {
            gridWidth = gameManager.gridWidth;
            gridHeight = gameManager.gridHeight;
        }
    }

    private void OnDrawGizmos()
    {
        Handles.color = lineColor;

        // Слои по которым будут расставлены кубики
        for (int h = 0; h < gridHeight; h++) {
            DrawRectangle(h);
        }

    }

    /// <summary>
    /// Рисует прямоугольник (основание или верхнюю границу)
    /// </summary>
    private void DrawRectangle(int h)
    {
        Vector3 leftBottom = start  + new Vector3(0, h, 0);
        Vector3 rightBottom = start + new Vector3(gridWidth, h, 0);
        Vector3 rightUp = start     + new Vector3(gridWidth, h, gridWidth);
        Vector3 leftUp = start      + new Vector3(0, h, gridWidth);


        Handles.DrawLine(leftBottom, rightBottom);
        Handles.DrawLine(rightBottom, rightUp);
        Handles.DrawLine(rightUp, leftUp);
        Handles.DrawLine(leftUp, leftBottom);
    }
}
#endif
