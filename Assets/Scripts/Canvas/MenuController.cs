using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("�������� ����� ����.")]
public class MenuController : MonoBehaviour
{
    [SerializeField] MobileInput mobileInput;
    [SerializeField] ComputerInput computerInput;

    [Tooltip("��������� ��� �������")]
    [SerializeField] Transform detailsContainer;

    [Tooltip("������� ������� ������������ ������ �� ����� ����:")]
    [SerializeField] List<GameObject> objects;

    [Tooltip("������������� ������")]
    [SerializeField] List<GameObject> interactiveBtns;

    /// <summary>
    /// ������� � ����� ����.
    /// </summary>
    public void setPlayMode()
    {
        mobileInput.enabled = true;
        computerInput.enabled = true;

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
        mobileInput.enabled = false;
        computerInput.enabled = false;

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
