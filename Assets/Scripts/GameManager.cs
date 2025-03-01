using UnityEngine;

class GameManager : MonoBehaviour
{
    [Header("Настройки поля:")]
    [Tooltip("Префаб кубика, который будет использоваться для генерации поля")]
    public GameObject cubePrefab; // Префаб кубика

    [Tooltip("Ширина (N x N) игровой области")]
    [Range(1, 10)] // Ограничиваем размер поля в разумных пределах
    public int gridWidth = 5; // Размерность поля

    [Tooltip("Высота игровой области")]
    [Range(1, 20)] // Ограничиваем размер поля в разумных пределах
    public int gridHeight = 15; // Размерность поля


    [Header("Ещё настройки:")]
    bool pause = false;

}
