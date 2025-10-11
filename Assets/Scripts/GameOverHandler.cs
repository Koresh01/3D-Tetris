using UnityEngine;

/// <summary>
/// Смотрит когда игрок проиграл.
/// </summary>
public class GameOverHandler : MonoBehaviour 
{
    [Header("Зависимости:")]
    [SerializeField] MenuController menuController;

    [Header("Условия окончания игры:")]
    [SerializeField] float Ybound;

    private void Update()
    {
        HandleGridCanSpawn();
        HandleActiveDetailZpos();
    }

    /// <summary>
    /// Отслеживает конец игры, когда всё поле занято детальками.
    /// </summary>
    void HandleGridCanSpawn()
    {
        if (!Grid.CanSpawn())
            menuController.SetGameOverMode();
    }

    /// <summary>
    /// Отслеживает ситуацию, когда падающая деталь вообще не попала на поле.
    /// </summary>
    void HandleActiveDetailZpos()
    {
        GameObject currentDetail = GameManager.currentDetail;
        if (currentDetail == null) return;

        float yPos = currentDetail.transform.position.y;
        if (yPos < Ybound)
            menuController.SetGameOverMode();
    }
}
