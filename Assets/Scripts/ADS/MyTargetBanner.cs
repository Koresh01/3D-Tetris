using System;
using UnityEngine;
using Mycom.Target.Unity.Ads; // Подключаем VK Ads SDK
using static Mycom.Target.Unity.Ads.MyTargetView;

/// <summary>
/// Класс для отображения рекламного баннера VK Ads (myTarget) в верхнем левом углу экрана.
/// </summary>
public class MyTargetBanner : MonoBehaviour
{
    private MyTargetView _myTargetView; // Экземпляр рекламного баннера
    private readonly object _syncRoot = new object(); // Объект синхронизации потоков

    /// <summary>
    /// ID рекламного слота. Его нужно заменить на свой ID, полученный в кабинете myTarget (VK Ads).
    /// </summary>
#if UNITY_ANDROID
    private const uint SLOT_ID = 1795055; // Укажи свой Android slot ID
#elif UNITY_IOS
    private const uint SLOT_ID = 1795055; // Укажи свой iOS slot ID
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

            // Создаем рекламный баннер формата 320x50 без автоматической ротации
            _myTargetView = new MyTargetView(SLOT_ID, AdSize.Size320x50);

            // Подписываемся на события
            _myTargetView.AdClicked += OnAdClicked;
            _myTargetView.AdLoadFailed += OnAdLoadFailed;
            _myTargetView.AdLoadCompleted += OnAdLoadCompleted;
            _myTargetView.AdShown += OnAdShown;

            // Загружаем рекламу
            _myTargetView.Load();
        }
    }

    /// <summary>
    /// Обработчик события успешной загрузки рекламы.
    /// Устанавливает позицию в верхнем левом углу и запускает показ.
    /// </summary>
    private void OnAdLoadCompleted(object sender, EventArgs eventArgs)
    {
        _myTargetView.X = 0; // Левый край экрана
        _myTargetView.Y = 0; // Верхний край экрана
        _myTargetView.Start(); // Показываем рекламу
    }

    /// <summary>
    /// Обработчик события клика по рекламе.
    /// </summary>
    private void OnAdClicked(object sender, EventArgs eventArgs)
    {
        Debug.Log("Пользователь кликнул на рекламу.");
    }

    /// <summary>
    /// Обработчик события ошибки загрузки рекламы.
    /// </summary>
    private void OnAdLoadFailed(object sender, ErrorEventArgs errorEventArgs)
    {
        Debug.LogError($"Ошибка загрузки рекламы: {errorEventArgs.Message}");
    }

    /// <summary>
    /// Обработчик события успешного показа рекламы.
    /// </summary>
    private void OnAdShown(object sender, EventArgs eventArgs)
    {
        Debug.Log("Реклама показана.");
    }

    /// <summary>
    /// Освобождаем ресурсы при удалении объекта.
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
