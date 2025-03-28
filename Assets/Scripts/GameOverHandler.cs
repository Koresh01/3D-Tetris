using UnityEngine;

/// <summary>
/// Смотрит когда игрок проиграл.
/// </summary>
public class GameOverHandler : MonoBehaviour 
{
    [Header("Зависимости:")]
    [SerializeField] MenuController menuController;

    private void Update()
    {
        if (!Grid.CanSpawn())
            menuController.SetGameOverMode();
    }
}
