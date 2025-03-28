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

    [SerializeField] DissolveHandler dissolveHandler;

    /// <summary>
    /// ������� � ����� ����.
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
    public void ResetGame()
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

    /// <summary>
    /// ������ � ����� GameOver.
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
