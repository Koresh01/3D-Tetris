using UnityEngine;

/// <summary>
/// ������ ������������� ������� �������� FPS � ����.
/// </summary>
public class FPScontroller : MonoBehaviour
{
    [SerializeField] private int targetFPS = 120;

    private void Awake()
    {
        // ��������� ������������ �������������(���� �������� ��� ���� ���������� FPS)
        QualitySettings.vSyncCount = 0;

        // ������������� �������� FPS:
        Application.targetFrameRate = targetFPS;
    }


}
