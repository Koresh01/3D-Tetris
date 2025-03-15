#import "PlatformUtils.h"
#import "MTRGUnityObjectsManager.h"
#import "MTRGUnityTracer.h"
#import <MyTargetSDK/MTRGInterstitialAd.h>

MTRGInterstitialAd *mtrg_unity_findInterstitialAdWithAdId(uint32_t adId, NSString *logContext)
{
    id object = [[MTRGUnityObjectsManager sharedManager] findObjectWithId:adId];
    if (object)
    {
        if ([object isKindOfClass:[MTRGInterstitialAd class]])
        {
            return (MTRGInterstitialAd *) (object);
        }
        else
        {
            mtrg_unity_tracer_d(@"%@ error, invalid object type for adId = %d", logContext, adId);
        }
    }
    else
    {
        mtrg_unity_tracer_d(@"%@ error, object with adId = %d not found", logContext, adId);
    }
    return nil;
}

uint32_t MTRGInterstitialAdCreate(uint32_t slotId)
{
    mtrg_unity_tracer_d(@"MTRGInterstitialAdCreate, slotId = %d", slotId);

    MTRGInterstitialAd *ad = [[MTRGInterstitialAd alloc] initWithSlotId:slotId];
    if (ad)
    {
        ad.delegate = [MTRGUnityObjectsManager sharedManager];
    }

    uint32_t adId = [[MTRGUnityObjectsManager sharedManager] registerObject:ad];

    mtrg_unity_tracer_d(@"MTRGInterstitialAd created, adId = %d", adId);

    return adId;
}

void MTRGInterstitialAdDelete(uint32_t adId)
{
    mtrg_unity_tracer_d(@"MTRGInterstitialAdDelete, adId = %d", adId);

    [[MTRGUnityObjectsManager sharedManager] unregisterObject:adId];

    mtrg_unity_tracer_d(@"MTRGInterstitialAd deleted, adId = %d", adId);
}

void MTRGInterstitialAdLoad(uint32_t adId)
{
    [[NSOperationQueue mainQueue] addOperationWithBlock:^
    {
        mtrg_unity_tracer_d(@"MTRGInterstitialAdLoad, adId = %d", adId);

        MTRGInterstitialAd *interstitialAd = mtrg_unity_findInterstitialAdWithAdId(adId, @"MTRGInterstitialAdLoad");
        if (interstitialAd)
        {
            [interstitialAd load];
        }
    }];
}

void MTRGInterstitialAdShow(uint32_t adId)
{
    [[NSOperationQueue mainQueue] addOperationWithBlock:^
    {
        mtrg_unity_tracer_d(@"MTRGInterstitialAdShow, adId = %d", adId);

        UIViewController *controller = mtrg_unity_topViewController();
        if (controller)
        {
            MTRGInterstitialAd *interstitialAd = mtrg_unity_findInterstitialAdWithAdId(adId, @"MTRGInterstitialAdShow");
            if (interstitialAd)
            {
                [interstitialAd showWithController:controller];
            }
        }
        else
        {
            mtrg_unity_tracer_e(@"MTRGInterstitialAdShow - UI not found");
        }
    }];
}

void MTRGInterstitialAdClose(uint32_t adId)
{
    [[NSOperationQueue mainQueue] addOperationWithBlock:^
    {
        mtrg_unity_tracer_d(@"MTRGInterstitialAdClose, adId = %d", adId);

        MTRGInterstitialAd *interstitialAd = mtrg_unity_findInterstitialAdWithAdId(adId, @"MTRGInterstitialAdClose");
        if (interstitialAd)
        {
            [interstitialAd close];
        }
    }];
}