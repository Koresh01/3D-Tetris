using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Менеджер паузы игры.")]
public class MenuController : MonoBehaviour
{
    [Header("Камера:")]
    [Tooltip("Объект со скриптами обрабатывающими ввод пользователя.")]
    [SerializeField] GameObject InputHandler;

    [Tooltip("Скрипт перемещающий камеру во время перехода в меню и обратно.")]
    [SerializeField] CameraMover CameraMover;

    [Tooltip("Контейнер для деталей.")]
    [SerializeField] Transform detailsContainer;

    [Header("Остальное:")]
    [Tooltip("Контроллер очков.")]
    [SerializeField] ScoreController ScoreController;

    [Tooltip("Объекты которые отображаются только во время игры.")]
    [SerializeField] List<GameObject> objects;

    [Tooltip("Интерактивные кнопки")]
    [SerializeField] List<GameObject> interactiveBtns;

    /// <summary>
    /// Переход в режим игры.
    /// </summary>
    public void setPlayMode()
    {
        // Спавнит первую деталь:
        if (GameManager.isPaused)
            GameManager.isPaused = false;   // теперь игра не на паузе
        else
            DetailsSpawner.Instance.SpawnNextDetail();  // Если игра не была на паузе, а было нажатие на кнопку "играть" => Это первый запуск игры и надо заспавнить первую деталь. А дальше они сами спавнятся.

        

        InputHandler.SetActive(true);
        CameraMover.SwitchToGameMode();
        
        

        // Отображаем нужные для игрового процесса объекты на сцене:
        foreach (var obj in objects)
        {
            obj.SetActive(true);
        }

        // Выключаем кнопки меню:
        foreach (var btn in interactiveBtns)
        {
            btn.SetActive(false);
        }
    }

    /// <summary>
    /// Переход в режим меню.
    /// </summary>
    public void setMenuMode()
    {
        // Ставим игру на стоп:
        GameManager.isPaused = true;

        InputHandler.SetActive(false);
        CameraMover.SwitchToMenuMode();

        // Тушим лишние объекты на сцене:
        foreach (var obj in objects)
        {
            obj.SetActive(false);
        }

        // Включаем кнопки меню:
        foreach (var btn in interactiveBtns)
        {
            btn.SetActive(true);
        }
    }

    /// <summary>
    /// Удаляет все существующие на данный момент кубики.
    /// </summary>
    public void ClearGrid()
    {
        GameManager.isPaused = false;   // теперь игра не на паузе

        ScoreController.SetScore(0);
        Grid.ClearGrid();

        // Удаляем еще летящие детальки:
        for (int i = detailsContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(detailsContainer.GetChild(i).gameObject);
        }
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Остановить игру в редакторе
#else
            Application.Quit(); // Закрыть приложение в билде
#endif
    }


}
