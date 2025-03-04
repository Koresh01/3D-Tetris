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
        /*for (int h = 0; h < gridHeight; h++) {
            DrawRectangle(h);
        }*/
        DrawRectangle(-0.5f);
        DrawRectangle(gridHeight-0.5f);

        DrawLines(gridHeight);

    }

    /// <summary>
    /// Рисует прямоугольник (основание или верхнюю границу)
    /// </summary>
    private void DrawRectangle(float h)
    {
        Vector3 leftBottom  = start + new Vector3(0, h, 0);
        Vector3 rightBottom = start + new Vector3(gridWidth, h, 0);
        Vector3 rightUp     = start + new Vector3(gridWidth, h, gridWidth);
        Vector3 leftUp      = start + new Vector3(0, h, gridWidth);


        Handles.DrawLine(leftBottom, rightBottom);
        Handles.DrawLine(rightBottom, rightUp);
        Handles.DrawLine(rightUp, leftUp);
        Handles.DrawLine(leftUp, leftBottom);
    }

    /// <summary>
    /// Рисует вертикальные линии.
    /// </summary>
    private void DrawLines(int h)
    {
        Vector3 l1      = start + new Vector3(0, -0.5f, 0);
        Vector3 l1Up    = start + new Vector3(0, h-0.5f, 0);

        Vector3 l2      = start + new Vector3(gridWidth, -0.5f, 0);
        Vector3 l2Up    = start + new Vector3(gridWidth, h-0.5f, 0);

        Vector3 l3      = start + new Vector3(0, -0.5f, gridWidth);
        Vector3 l3Up    = start + new Vector3(0, h-0.5f, gridWidth);

        Vector3 l4      = start + new Vector3(gridWidth, -0.5f, gridWidth);
        Vector3 l4Up    = start + new Vector3(gridWidth, h-0.5f, gridWidth);




        Handles.DrawLine(l1, l1Up);
        Handles.DrawLine(l2, l2Up);
        Handles.DrawLine(l3, l3Up);
        Handles.DrawLine(l4, l4Up);
    }
}
#endif
