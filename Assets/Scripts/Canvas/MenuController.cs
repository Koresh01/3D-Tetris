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
    [Tooltip("������� ������� ������������ ������ �� ����� ����.")]
    [SerializeField] List<GameObject> objects;

    [Tooltip("������������� ������")]
    [SerializeField] List<GameObject> interactiveBtns;

    /// <summary>
    /// ������� � ����� ����.
    /// </summary>
    public void setPlayMode()
    {
        InputHandler.SetActive(true);
        CameraMover.setPlayMode();

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
        InputHandler.SetActive(false);
        CameraMover.setMenuMode();

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
        ScoreController.score = 0;
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
