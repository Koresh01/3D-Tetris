using UnityEngine;

[AddComponentMenu("Custom/GameManager (Главный скрипт с настройками всей игры.)")]
public class GameManager : MonoBehaviour
{
    [Header("Настройки поля:")]

    [Range(1, 9)]
    [Tooltip("Ширина (N x N) игровой области")]
    [SerializeField] private int _gridWidth = 5;

    [Range(1, 20)]
    [Tooltip("Высота игровой области")]
    [SerializeField] private int _gridHeight = 15;

    
    public static int gridWidth { get; private set; }
    public static int gridHeight { get; private set; }

    [Header("Ещё настройки:")]
    private bool pause = false;

    [Header("Текущее состояние игры:")]
    private bool _readyToCreateDetail = false;


    

    private void Awake()
    {
        gridWidth = _gridWidth;
        gridHeight = _gridHeight;


        Grid.InitializeGrid();
    }
}
