using System;
using UnityEngine;
using YandexMobileAds;
using YandexMobileAds.Base;

public class YandexMobileAdsBannerDemoScript : MonoBehaviour
{
    [Header("Настройки баннера")]
    
    [SerializeField] private AdPosition adPosition = AdPosition.TopCenter;
    [SerializeField] private string adUnitId = "demo-banner-yandex"; // ⚠️ Замени на свой ID

    [Header("Размеры баннера:")]
    [SerializeField] private BannerSizeType bannerSize = BannerSizeType.Banner320x50;
    [Header("Высота баннера в px (Используется если выбран InlineAdaptive)")]
    [SerializeField, Range(30, 400)] int adaptiveHeightPx = 100;

    private Banner banner;

    private void Awake()
    {
        RequestBanner();
    }

    private void RequestBanner()
    {
        MobileAds.SetAgeRestrictedUser(true);

        if (banner != null)
            banner.Destroy();

        // Преобразуем выбранный enum в конкретный размер
        BannerAdSize bannerAdSize = GetBannerAdSize(bannerSize);
        banner = new Banner(adUnitId, bannerAdSize, adPosition);

        banner.OnAdLoaded += HandleAdLoaded;
        banner.OnAdFailedToLoad += HandleAdFailedToLoad;

        banner.LoadAd(CreateAdRequest());
        Debug.Log($"Yandex Banner: запрошен баннер ({bannerSize})");
    }

    private AdRequest CreateAdRequest() => new AdRequest.Builder().Build();

    private void HandleAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("Yandex Banner: баннер загружен");
        banner.Show();
    }

    private void HandleAdFailedToLoad(object sender, AdFailureEventArgs args)
    {
        Debug.LogWarning("Yandex Banner: ошибка загрузки — " + args.Message);
    }

    private BannerAdSize GetBannerAdSize(BannerSizeType type)
    {
        switch (type)
        {
            case BannerSizeType.Banner320x50:
                return BannerAdSize.FixedSize(320, 50);
            case BannerSizeType.Banner300x250:
                return BannerAdSize.FixedSize(300, 250);
            case BannerSizeType.Banner728x90:
                return BannerAdSize.FixedSize(728, 90);
            case BannerSizeType.InlineAdaptive: // Максимум по ширине с заданной высотой в пикселях
                int widthDp = GetScreenWidthDp();
                int heightDp = ScreenUtils.ConvertPixelsToDp(adaptiveHeightPx);
                return BannerAdSize.InlineSize(widthDp, heightDp);  
            case BannerSizeType.StickyAdaptive:
                return BannerAdSize.StickySize(GetScreenWidthDp()); // вариант который советует yandex, но высота баннера у StickySize определяется АВТОМАТИЧЕСКИ внутри yandex sdk.
            default:
                return BannerAdSize.FixedSize(320, 50);
        }
    }

    private int GetScreenWidthDp()
    {
        int screenWidth = (int)Screen.safeArea.width;
        return ScreenUtils.ConvertPixelsToDp(screenWidth);
    }
}

/// <summary>
/// Возможные размеры баннеров (удобно выбирать в Unity Inspector)
/// </summary>
public enum BannerSizeType
{
    Banner320x50,
    Banner300x250,
    Banner728x90,
    InlineAdaptive,
    StickyAdaptive
}
