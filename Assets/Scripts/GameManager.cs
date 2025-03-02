using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Настройки поля:")]

    [Tooltip("Ширина (N x N) игровой области")]
    [Range(1, 9)]
    public int gridWidth = 5;

    [Tooltip("Высота игровой области")]
    [Range(1, 20)]
    public int gridHeight = 15;


    [Header("Ещё настройки:")]
    bool pause = false;

    [Header("Текущее состояние игры:")]
    bool readyToCreateDetail = false;

}
