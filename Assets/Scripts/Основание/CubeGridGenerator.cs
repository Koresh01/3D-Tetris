using UnityEngine;

/// <summary>
/// Скрипт для генерации квадратного поля из кубиков.
/// Кубики размещаются вплотную, зная, что их стандартный размер 1x1x1 метр.
/// </summary>
public class CubeGridGenerator : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    GameObject cubePrefab;

    [Tooltip("Ширина игровой области")]
    int gridWidth;

    [Tooltip("Высота игровой области")]
    int gridHeight;

    /// <summary>
    /// Генерирует квадратное поле из кубиков при старте сцены.
    /// </summary>
    void Start()
    {
        gridWidth = gameManager.gridWidth;
        cubePrefab = gameManager.cubePrefab;
        gridHeight = gameManager.gridHeight;
        GenerateGrid();
    }

    /// <summary>
    /// Метод для генерации квадратного поля из кубиков.
    /// </summary>
    void GenerateGrid()
    {
        if (cubePrefab == null)
        {
            Debug.LogError("Не назначен префаб кубика!");
            return;
        }

        // Вычисляем начальную точку так, чтобы поле было центрировано относительно (0, 0, 0)
        float offset = (gridWidth - 1) / 2.0f;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridWidth; z++)
            {
                // Вычисляем позицию каждого кубика
                Vector3 position = new Vector3(x - offset, 0, z - offset);

                // Создаем кубик
                Instantiate(cubePrefab, position, Quaternion.identity, transform);
            }
        }
    }
}
