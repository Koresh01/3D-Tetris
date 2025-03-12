using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartStopManager : MonoBehaviour
{
    public CanvasGroup menuPanel; // ��� �������� ������������ UI
    public GameObject inputSystem;
    public float fadeDuration = 1f; // ������������ �������� ������������

    public void StartPlaying()
    {
        StartCoroutine(fadeCanvasGroup());
        // �������� ���� �� ������������.
        inputSystem.SetActive(true);
    }

    public void StopPlaying()
    {
        StartCoroutine(showCanvasGroup());
        // ��������� ���� �� ������������.
        inputSystem.SetActive(false);
    }

    // ������� ������������ UI � �������� �����
    private IEnumerator fadeCanvasGroup()
    {
        // ������� ������������ UI (�� 1 � 0)
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            menuPanel.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration); // ������� ������������ (�� 1 � 0)
            yield return null;
        }
        // ������������� ������������� ������������
        menuPanel.alpha = 0f;

        // ��������� ��� ������
        menuPanel.gameObject.SetActive(false);
    }

    // ������� ������������ UI � �������� �����
    private IEnumerator showCanvasGroup()
    {
        // �������� ��� ������
        menuPanel.gameObject.SetActive(true);

        // ������� ������������ UI (�� 1 � 0)
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            menuPanel.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration); // ������� ������������ (�� 1 � 0)
            yield return null;
        }
        // ������������� ������������� ������������
        menuPanel.alpha = 1f;
    }
}
