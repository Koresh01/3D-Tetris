using System;
using UnityEngine;
using Mycom.Target.Unity.Ads; // Подключаем VK Ads SDK
using static Mycom.Target.Unity.Ads.MyTargetView;

public class MyFullScreenBlock : MonoBehaviour
{
    private InterstitialAd _interstitialAd;

    private InterstitialAd CreateInterstitialAd()
    {
        UInt32 slotId = 0;
#if UNITY_ANDROID
        slotId = 1798817; // Замените на ваш реальный slotId для Android
#elif UNITY_IOS
        slotId = 1798817; // Замените на ваш реальный slotId для iOS
#endif

        return new InterstitialAd(slotId);
    }

    private void Start()
    {
        InitAd();
    }

    private void InitAd()
    {
        _interstitialAd = CreateInterstitialAd();

        _interstitialAd.AdLoadCompleted += OnLoadCompleted;
        _interstitialAd.AdDismissed += OnAdDismissed;
        _interstitialAd.AdDisplayed += OnAdDisplayed;
        _interstitialAd.AdVideoCompleted += OnAdVideoCompleted;
        _interstitialAd.AdClicked += OnAdClicked;
        _interstitialAd.AdLoadFailed += OnAdLoadFailed;

        _interstitialAd.Load();
    }

    private void OnLoadCompleted(object sender, EventArgs e)
    {
        Debug.Log("Interstitial Ad Loaded");
        _interstitialAd.Show(); // Показ рекламы на отдельной странице
    }

    private void OnAdDismissed(object sender, EventArgs e)
    {
        Debug.Log("Interstitial Ad Dismissed");
    }

    private void OnAdDisplayed(object sender, EventArgs e)
    {
        Debug.Log("Interstitial Ad Displayed");
    }

    private void OnAdVideoCompleted(object sender, EventArgs e)
    {
        Debug.Log("Ad Video Completed");
    }

    private void OnAdClicked(object sender, EventArgs e)
    {
        Debug.Log("Ad Clicked");
    }

    private void OnAdLoadFailed(object sender, ErrorEventArgs e)
    {
        Debug.LogError("Interstitial Ad Load Failed: " + e.Message);
    }
}
