using System.Collections.Generic;
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

        

        InputHandler.SetActive(true);
        CameraMover.SwitchToGameMode();
        
        

        // ���������� ������ ��� �������� �������� ������� �� �����:
        foreach (var obj in objects)
        {
            obj.SetActive(true);
        }

        // ��������� ������ ����:
        foreach (var btn in interactiveBtns)
        {
            btn.SetActive(false);
        }
    }

    /// <summary>
    /// ������� � ����� ����.
    /// </summary>
    public void setMenuMode()
    {
        // ������ ���� �� ����:
        GameManager.isPaused = true;

        InputHandler.SetActive(false);
        CameraMover.SwitchToMenuMode();

        // ����� ������ ������� �� �����:
        foreach (var obj in objects)
        {
            obj.SetActive(false);
        }

        // �������� ������ ����:
        foreach (var btn in interactiveBtns)
        {
            btn.SetActive(true);
        }
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


}
