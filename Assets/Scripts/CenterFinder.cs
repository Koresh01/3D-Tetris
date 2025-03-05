using UnityEngine;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// Находит центр игрового поля.
/// </summary>
[AddComponentMenu("Custom/Center Finder (Находит центр игрового поля)")]
class CenterFinder : MonoBehaviour
{
    private void Start()
    {
        // FindCenter(); 
        // т.к. он вызывается из DetailsSpawner
    }

    /// <summary>
    /// Ищет центр игрового поля.
    /// </summary>
    public void FindCenter()
    {
        // Определяем куда будет смотреть камера в самом начале:
        int gridWidth = GameManager.gridWidth;
        //int gridHeight = gameManager.gridHeight;
        Vector3 start = new Vector3(-0.5f, 0.0f, -0.5f);
        transform.position = start + new Vector3(gridWidth / 2f, 0, gridWidth / 2f);
    }
}
