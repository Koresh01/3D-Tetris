using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Менеджер паузы игры.")]
public class MenuController : MonoBehaviour
{
    [SerializeField] MobileInput mobileInput;
    [SerializeField] ComputerInput computerInput;

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
}
