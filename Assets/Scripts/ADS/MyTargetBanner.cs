using System;
using UnityEngine;
using Mycom.Target.Unity.Ads; // ���������� VK Ads SDK
using static Mycom.Target.Unity.Ads.MyTargetView;

/// <summary>
/// ����� ��� ����������� ���������� ������� VK Ads (myTarget) � ������� ����� ���� ������.
/// </summary>
public class MyTargetBanner : MonoBehaviour
{
    private MyTargetView _myTargetView; // ��������� ���������� �������
    private readonly object _syncRoot = new object(); // ������ ������������� �������

    /// <summary>
    /// ID ���������� �����. ��� ����� �������� �� ���� ID, ���������� � �������� myTarget (VK Ads).
    /// </summary>
#if UNITY_ANDROID
    private const uint SLOT_ID = 1795055; // ����� ���� Android slot ID
#elif UNITY_IOS
    private const uint SLOT_ID = 1795055; // ����� ���� iOS slot ID
#endif

    private void Awake()
    {
        if (_myTargetView != null)
        {
            return;
        }

        lock (_syncRoot)
        {
            if (_myTargetView != null)
            {
                return;
            }

            // ������� ��������� ������ ������� 320x50 ��� �������������� �������
            _myTargetView = new MyTargetView(SLOT_ID, AdSize.Size320x50);

            // ������������� �� �������
            _myTargetView.AdClicked += OnAdClicked;
            _myTargetView.AdLoadFailed += OnAdLoadFailed;
            _myTargetView.AdLoadCompleted += OnAdLoadCompleted;
            _myTargetView.AdShown += OnAdShown;

            // ��������� �������
            _myTargetView.Load();
        }
    }

    /// <summary>
    /// ���������� ������� �������� �������� �������.
    /// ������������� ������� � ������� ����� ���� � ��������� �����.
    /// </summary>
    private void OnAdLoadCompleted(object sender, EventArgs eventArgs)
    {
        _myTargetView.X = 0; // ����� ���� ������
        _myTargetView.Y = 0; // ������� ���� ������
        _myTargetView.Start(); // ���������� �������
    }

    /// <summary>
    /// ���������� ������� ����� �� �������.
    /// </summary>
    private void OnAdClicked(object sender, EventArgs eventArgs)
    {
        Debug.Log("������������ ������� �� �������.");
    }

    /// <summary>
    /// ���������� ������� ������ �������� �������.
    /// </summary>
    private void OnAdLoadFailed(object sender, ErrorEventArgs errorEventArgs)
    {
        Debug.LogError($"������ �������� �������: {errorEventArgs.Message}");
    }

    /// <summary>
    /// ���������� ������� ��������� ������ �������.
    /// </summary>
    private void OnAdShown(object sender, EventArgs eventArgs)
    {
        Debug.Log("������� ��������.");
    }

    /// <summary>
    /// ����������� ������� ��� �������� �������.
    /// </summary>
    private void OnDestroy()
    {
        if (_myTargetView == null)
        {
            return;
        }

        lock (_syncRoot)
        {
            if (_myTargetView == null)
            {
                return;
            }

            _myTargetView.Dispose();
            _myTargetView = null;
        }
    }
}
