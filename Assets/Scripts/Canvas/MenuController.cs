using System.Collections.Generic;
using System.Collections;
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

    [SerializeField] DissolveHandler dissolveHandler;

    /// <summary>
    /// Переход в режим игры.
    /// </summary>
    public void setPlayMode()
    {
        StartCoroutine(dissolveHandler.ShowObject());

        if (GameManager.currentDetail == null)
        {
            DetailsSpawner.Instance.SpawnNextDetail();
        }
        GameManager.isPaused = false;



        CameraMover.SwitchToGameMode();



        // Отображаем нужные для игрового процесса объекты на сцене:
        // Выключаем кнопки меню:
        StartCoroutine(ToggleObjectsWithDelay(objects, true, interactiveBtns, false));
    }

    /// <summary>
    /// Переход в режим меню.
    /// </summary>
    public void setMenuMode()
    {
        // Ставим игру на стоп:
        GameManager.isPaused = true;

        CameraMover.SwitchToMenuMode();

        // Тушим лишние объекты на сцене:
        // Включаем кнопки меню:
        StartCoroutine(ToggleObjectsWithDelay(objects, false, interactiveBtns, true));

    }

    /// <summary>
    /// Удаляет все существующие на данный момент кубики.
    /// </summary>
    public void ResetGame()
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

    /// <summary>
    /// Перход в режим GameOver.
    /// </summary>
    public void SetGameOverMode()
    {
        StartCoroutine(dissolveHandler.HideObject());
        ResetGame();
        setMenuMode();
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Остановить игру в редакторе
#else
            Application.Quit(); // Закрыть приложение в билде
#endif
    }

    /// <summary>
    /// Постепенно включает или выключает объекты и кнопки с небольшой задержкой между изменениями.
    /// </summary>
    /// <param name="objects">Список игровых объектов для переключения.</param>
    /// <param name="stateObjects">Состояние (true - включить, false - выключить) для игровых объектов.</param>
    /// <param name="buttons">Список кнопок UI для переключения.</param>
    /// <param name="stateButtons">Состояние (true - включить, false - выключить) для кнопок.</param>
    IEnumerator ToggleObjectsWithDelay(List<GameObject> objects, bool stateObjects, List<GameObject> buttons, bool stateButtons)
    {
        foreach (var obj in objects)
        {
            obj.SetActive(stateObjects);
            yield return new WaitForSecondsRealtime(0.025f);
        }

        foreach (var btn in buttons)
        {
            btn.SetActive(stateButtons);
            yield return new WaitForSecondsRealtime(0.025f);
        }
    }

}
