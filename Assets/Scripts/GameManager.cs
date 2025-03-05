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






    [Header("Ссылки на скрипты:")]
    [SerializeField] private DetailsSpawner detailsSpawner;





    [Header("Ещё настройки:")]
    [SerializeField] private bool pause = false;





    [Header("Текущее состояние игры:")]
    [SerializeField] private bool _readyToCreateDetail = false;


    [Tooltip("Текущая деталь:")]
    [SerializeField] private GameObject _currentDetail;




    private void Awake()
    {
        gridWidth = _gridWidth;
        gridHeight = _gridHeight;


        Grid.InitializeGrid();
    }

    private void Update()
    {
        if (_readyToCreateDetail)
        {
            _currentDetail = detailsSpawner.SpawnDetail();
            _readyToCreateDetail = false;
        }
    }
}
