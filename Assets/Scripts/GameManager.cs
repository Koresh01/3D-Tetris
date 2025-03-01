using UnityEngine;

class GameManager : MonoBehaviour
{
    [Tooltip("Размерность поля (N x N)")]
    [Range(1, 10)] // Ограничиваем размер поля в разумных пределах
    public int gridSize = 5; // Размерность поля
    
    
    
    bool pause = false;

}
