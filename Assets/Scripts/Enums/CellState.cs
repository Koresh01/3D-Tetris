using UnityEngine;
/// <summary>
/// Возможные состояния клетки в игровом поле.
/// </summary>
public enum CellState
{
    [Tooltip("Клетка свободна")]
    Free,
    [Tooltip("Клетка занята")]
    Filled
}