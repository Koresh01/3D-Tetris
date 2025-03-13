using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Менеджер паузы игры.")]
public class MenuController : MonoBehaviour
{
    [SerializeField] MobileInput mobileInput;
    [SerializeField] ComputerInput computerInput;

    [Tooltip("Контейнер для деталей")]
    [SerializeField] Transform detailsContainer;

    [Tooltip("Объекты которые отображаются только во время игры:")]
    [SerializeField] List<GameObject> objects;

    [Tooltip("Интерактивные кнопки")]
    [SerializeField] List<GameObject> interactiveBtns;

    /// <summary>
    /// Переход в режим игры.
    /// </summary>
    public void setPlayMode()
    {
        mobileInput.enabled = true;
        computerInput.enabled = true;

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
        mobileInput.enabled = false;
        computerInput.enabled = false;

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
        ScoreController.score = 0;
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
