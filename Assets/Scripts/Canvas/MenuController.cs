using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[AddComponentMenu("�������� ����� ����.")]
public class MenuController : MonoBehaviour
{
    [Header("������:")]
    [Tooltip("������ �� ��������� ��������������� ���� ������������.")]
    [SerializeField] GameObject InputHandler;

    [Tooltip("������ ������������ ������ �� ����� �������� � ���� � �������.")]
    [SerializeField] CameraMover CameraMover;

    [Tooltip("��������� ��� �������.")]
    [SerializeField] Transform detailsContainer;

    [Header("���������:")]
    [Tooltip("���������� �����.")]
    [SerializeField] ScoreController ScoreController;

    [Tooltip("������� ������� ������������ ������ �� ����� ����.")]
    [SerializeField] List<GameObject> objects;

    [Tooltip("������������� ������")]
    [SerializeField] List<GameObject> interactiveBtns;

    /// <summary>
    /// ������� � ����� ����.
    /// </summary>
    public void setPlayMode()
    {
        // ������� ������ ������:
        if (GameManager.isPaused)
            GameManager.isPaused = false;   // ������ ���� �� �� �����
        else
            DetailsSpawner.Instance.SpawnNextDetail();  // ���� ���� �� ���� �� �����, � ���� ������� �� ������ "������" => ��� ������ ������ ���� � ���� ���������� ������ ������. � ������ ��� ���� ���������.

        CameraMover.SwitchToGameMode();



        // ���������� ������ ��� �������� �������� ������� �� �����:
        // ��������� ������ ����:
        StartCoroutine(ToggleObjectsWithDelay(objects, true, interactiveBtns, false));
    }

    /// <summary>
    /// ������� � ����� ����.
    /// </summary>
    public void setMenuMode()
    {
        // ������ ���� �� ����:
        GameManager.isPaused = true;

        CameraMover.SwitchToMenuMode();

        // ����� ������ ������� �� �����:
        // �������� ������ ����:
        StartCoroutine(ToggleObjectsWithDelay(objects, false, interactiveBtns, true));

    }

    /// <summary>
    /// ������� ��� ������������ �� ������ ������ ������.
    /// </summary>
    public void ClearGrid()
    {
        GameManager.isPaused = false;   // ������ ���� �� �� �����

        ScoreController.SetScore(0);
        Grid.ClearGrid();

        // ������� ��� ������� ��������:
        for (int i = detailsContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(detailsContainer.GetChild(i).gameObject);
        }
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // ���������� ���� � ���������
#else
            Application.Quit(); // ������� ���������� � �����
#endif
    }

    /// <summary>
    /// ���������� �������� ��� ��������� ������� � ������ � ��������� ��������� ����� �����������.
    /// </summary>
    /// <param name="objects">������ ������� �������� ��� ������������.</param>
    /// <param name="stateObjects">��������� (true - ��������, false - ���������) ��� ������� ��������.</param>
    /// <param name="buttons">������ ������ UI ��� ������������.</param>
    /// <param name="stateButtons">��������� (true - ��������, false - ���������) ��� ������.</param>
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
