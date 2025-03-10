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






    [Header("Ссылки на скрипты:")]
    [SerializeField] private DetailsSpawner detailsSpawner;





    [Header("Ещё настройки:")]
    [SerializeField] private bool pause = false;





    [Header("Текущее состояние игры.")]
    [SerializeField] private bool _readyToCreateDetail = false;


    [Tooltip("Текущая деталь.")]
    public static GameObject currentDetail;




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
            detailsSpawner.SpawnDetail();
            _readyToCreateDetail = false;
        }
    }

    

    /// <summary>
    /// Удаляет слой игрового поля.
    /// </summary>
    public static void DestroyLayer(int layerInx)
    {
        for (int x = 0; x < gridWidth; x++)    // grid width не видит
        {
            for (int z = 0; z < gridWidth; z++)
            {
                Vector3Int CellPosition = new Vector3Int(x, layerInx, z);
                CellState state = Grid.GetCellState(CellPosition);
                GameObject block = Grid.GetCellGameObject(CellPosition);

                if (state == CellState.Filled)
                {
                    Grid.SetCellState(CellPosition, null, CellState.Free);
                    Destroy(block);
                }

                // Отрисовка состояния Grid на данный момент.
                if (CellsVizualizer.Instance != null)
                {
                    CellsVizualizer.Instance.UpdateCellMaterial(CellPosition);
                }
            }
        }
    }
}
