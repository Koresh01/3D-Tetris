using UnityEngine;

/// <summary>
/// Скрипт для генерации квадратного поля из кубиков.
/// Кубики размещаются вплотную, зная, что их стандартный размер 1x1x1 метр.
/// </summary>
public class CubeGridGenerator : MonoBehaviour
{
    [Header("Настройки поля")]
    [Tooltip("Префаб кубика, который будет использоваться для генерации поля")]
    public GameObject cubePrefab; // Префаб кубика
    [SerializeField] GameManager gameManager;

    [Tooltip("Размерность поля (N x N)")]
    int gridSize; // Размерность поля

    /// <summary>
    /// Генерирует квадратное поле из кубиков при старте сцены.
    /// </summary>
    void Start()
    {
        gridSize = gameManager.gridSize;
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
        float offset = (gridSize - 1) / 2.0f;

        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                // Вычисляем позицию каждого кубика
                Vector3 position = new Vector3(x - offset, 0, z - offset);

                // Создаем кубик
                Instantiate(cubePrefab, position, Quaternion.identity, transform);
            }
        }
    }
}
