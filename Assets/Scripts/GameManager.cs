using System.Collections.Generic;
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

    [Tooltip("Состояние паузы игры.")]
    public static bool isPaused = false;

    [Tooltip("Текущая деталь.")]
    public static GameObject currentDetail;

    [Tooltip("Текущая скорость детали (по умолчанию 1f).")]
    public static float currentDetailSpeed = 1f;


    private void Awake()
    {
        gridWidth = _gridWidth;
        gridHeight = _gridHeight;


        Grid.InitializeGrid();
    }

    private void Update()
    {
        // Если игра на стопе то останавливаем спавн деталей:
        if (isPaused) return;

        // Логика создания следующей детали:
        if (currentDetail == null) return;
        StructureController curDet = currentDetail.GetComponent<StructureController>();
        
        if (curDet.HasGroundContact(out BlockController touchingBlock))
        {
            SoundManager.Instance.PlayPlaceBlock();
            // Спавним первую детальку
            DetailsSpawner.Instance.SpawnNextDetail();
            // Отключаем ускоренное падение.
            FastFall_Off();
        }
    }

    #region DetailFalling 
    public void FastFall_On()
    {
        currentDetailSpeed = 10f;
    }

    public void FastFall_Off()
    {
        currentDetailSpeed = 1f;
    }
    #endregion
}
