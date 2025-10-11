using System;
using UnityEngine;
using YandexMobileAds;
using YandexMobileAds.Base;

public class YandexMobileAdsBannerDemoScript : MonoBehaviour
{
    private Banner banner;

    private void Awake()
    {
        RequestBanner();
    }

    private void RequestBanner()
    {
        // Устанавливаем ограничение COPPA (для пользователей младше 13 лет)
        MobileAds.SetAgeRestrictedUser(true);

        // Твой реальный идентификатор рекламного блока
        string adUnitId = "demo-banner-yandex"; // ⚠️ Замени на свой ID

        // Уничтожаем предыдущий баннер, если есть
        if (banner != null)
        {
            banner.Destroy();
        }

        // Размер баннера — адаптивная ширина, высота определяется SDK
        BannerAdSize bannerSize = BannerAdSize.StickySize(GetScreenWidthDp());

        // Создаём баннер и размещаем его вверху по центру
        banner = new Banner(adUnitId, bannerSize, AdPosition.TopCenter);

        // Подписываемся только на нужные события
        banner.OnAdLoaded += HandleAdLoaded;
        banner.OnAdFailedToLoad += HandleAdFailedToLoad;

        // Загружаем баннер
        banner.LoadAd(CreateAdRequest());

        Debug.Log("Yandex Banner: запрошен баннер");
    }

    private int GetScreenWidthDp()
    {
        int screenWidth = (int)Screen.safeArea.width;
        return ScreenUtils.ConvertPixelsToDp(screenWidth);
    }

    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder().Build();
    }

    private void HandleAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("Yandex Banner: баннер загружен");
        banner.Show();
    }

    private void HandleAdFailedToLoad(object sender, AdFailureEventArgs args)
    {
        Debug.LogWarning("Yandex Banner: ошибка загрузки — " + args.Message);
    }
}
