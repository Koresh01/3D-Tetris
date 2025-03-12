using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartStopManager : MonoBehaviour
{
    public CanvasGroup menuPanel; // Для плавного исчезновения UI
    public GameObject inputSystem;
    public float fadeDuration = 1f; // Длительность анимации исчезновения

    public void StartPlaying()
    {
        StartCoroutine(fadeCanvasGroup());
        // Включаем ввод от пользователя.
        inputSystem.SetActive(true);
    }

    public void StopPlaying()
    {
        StartCoroutine(showCanvasGroup());
        // Выключаем ввод от пользователя.
        inputSystem.SetActive(false);
    }

    // Плавное исчезновение UI и загрузка сцены
    private IEnumerator fadeCanvasGroup()
    {
        // Плавное исчезновение UI (от 1 к 0)
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            menuPanel.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration); // Плавное исчезновение (от 1 к 0)
            yield return null;
        }
        // Устанавливаем окончательную прозрачность
        menuPanel.alpha = 0f;

        // Выключаем всю панель
        menuPanel.gameObject.SetActive(false);
    }

    // Плавное исчезновение UI и загрузка сцены
    private IEnumerator showCanvasGroup()
    {
        // Включаем всю панель
        menuPanel.gameObject.SetActive(true);

        // Плавное исчезновение UI (от 1 к 0)
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            menuPanel.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration); // Плавное исчезновение (от 1 к 0)
            yield return null;
        }
        // Устанавливаем окончательную прозрачность
        menuPanel.alpha = 1f;
    }
}
