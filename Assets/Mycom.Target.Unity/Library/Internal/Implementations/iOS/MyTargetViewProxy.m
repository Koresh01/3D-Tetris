#import "PlatformUtils.h"
#import "MTRGUnityObjectsManager.h"
#import "MTRGUnityTracer.h"
#import <MyTargetSDK/MTRGAdView.h>
#import <MyTargetSDK/MTRGAdSize.h>

MTRGAdView *mtrg_unity_findStandardAdWithAdId(uint32_t adId, NSString *logContext)
{
    id object = [[MTRGUnityObjectsManager sharedManager] findObjectWithId:adId];
    if (object)
    {
        if ([object isKindOfClass:[MTRGAdView class]])
        {
            return (MTRGAdView *) (object);
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

uint32_t MTRGStandardAdCreate(uint32_t slotId, bool isRefreshAd, uint32_t adSize)
{
    MTRGAdSize *adViewSize;
    CGRect adViewFrame;
    switch (adSize)
    {
        case 1:
            adViewSize = [MTRGAdSize adSize300x250];
            adViewFrame = CGRectMake(0, 0, 300, 250);
            break;
        case 2:
            adViewSize = [MTRGAdSize adSize728x90];
            adViewFrame = CGRectMake(0, 0, 728, 90);
            break;
        default:
            adViewSize = [MTRGAdSize adSize320x50];
            adViewFrame = CGRectMake(0, 0, 320, 50);
            break;
    }

    mtrg_unity_tracer_d(@"MTRGStandardAdCreate, slotId = %d", slotId);

    MTRGAdView *adView = [MTRGAdView adViewWithSlotId:slotId shouldRefreshAd:isRefreshAd];
    if (adView)
    {
        adView.frame = adViewFrame;
        adView.adSize = adViewSize;
        adView.delegate = [MTRGUnityObjectsManager sharedManager];
        adView.viewController = mtrg_unity_topViewController();
    }

    uint32_t adId = [[MTRGUnityObjectsManager sharedManager] registerObject:adView];

    mtrg_unity_tracer_d(@"MTRGStandardAdCreate created, adId = %d", adId);

    return adId;
}

void MTRGStandardAdDelete(uint32_t adId)
{
    [[NSOperationQueue mainQueue] addOperationWithBlock:^
    {
        mtrg_unity_tracer_d(@"MTRGStandardAdDelete, adId = %d", adId);

        MTRGAdView *adView = mtrg_unity_findStandardAdWithAdId(adId, @"MTRGStandardAdStart");
        if (adView)
        {
            [adView removeFromSuperview];
        }

        [[MTRGUnityObjectsManager sharedManager] unregisterObject:adId];

        mtrg_unity_tracer_d(@"MTRGStandardAdDelete deleted, adId = %d", adId);
    }];
}

void MTRGStandardAdLoad(uint32_t adId)
{
    [[NSOperationQueue mainQueue] addOperationWithBlock:^
    {
        mtrg_unity_tracer_d(@"MTRGStandardAdLoad, adId = %d", adId);

        MTRGAdView *adView = mtrg_unity_findStandardAdWithAdId(adId, @"MTRGStandardAdLoad");
        if (adView)
        {
            [adView load];
        }
    }];
}

void MTRGStandardAdStart(uint32_t adId)
{
    [[NSOperationQueue mainQueue] addOperationWithBlock:^
    {
        mtrg_unity_tracer_d(@"MTRGStandardAdStart, adId = %d", adId);

        MTRGAdView *adView = mtrg_unity_findStandardAdWithAdId(adId, @"MTRGStandardAdStart");
        if (adView)
        {
            UIViewController *controller = mtrg_unity_topViewController();
            if (controller && controller.view)
            {
                [controller.view addSubview:adView];
            }
            else
            {
                mtrg_unity_tracer_e(@"MTRGStandardAdStart - UI not found");
            }
        }
    }];
}

void MTRGStandardAdStop(uint32_t adId)
{
    [[NSOperationQueue mainQueue] addOperationWithBlock:^
    {
        mtrg_unity_tracer_d(@"MTRGStandardAdStop, adId = %d", adId);
        MTRGAdView *adView = mtrg_unity_findStandardAdWithAdId(adId, @"MTRGStandardAdStop");
        if (adView)
        {
            [adView removeFromSuperview];
        }
    }];
}

void MTRGStandardAdSetX(uint32_t adId, uint32_t x)
{
    [[NSOperationQueue mainQueue] addOperationWithBlock:^
    {
        mtrg_unity_tracer_d(@"MTRGStandardAdSetX, adId = %d", adId);
        MTRGAdView *adView = mtrg_unity_findStandardAdWithAdId(adId, @"MTRGStandardAdSetX");
        if (adView)
        {
            CGRect frame = adView.frame;
            frame.origin.x = x;
            adView.frame = frame;
        }
    }];
}

void MTRGStandardAdSetY(uint32_t adId, uint32_t y)
{
    [[NSOperationQueue mainQueue] addOperationWithBlock:^
    {
        mtrg_unity_tracer_d(@"MTRGStandardAdSetY, adId = %d", adId);
        MTRGAdView *adView = mtrg_unity_findStandardAdWithAdId(adId, @"MTRGStandardAdSetY");
        if (adView)
        {
            CGRect frame = adView.frame;
            frame.origin.y = y;
            adView.frame = frame;
        }
    }];
}

void MTRGStandardAdSetWidth(uint32_t adId, uint32_t width)
{
    [[NSOperationQueue mainQueue] addOperationWithBlock:^
    {
        mtrg_unity_tracer_d(@"MTRGStandardAdSetWidth, adId = %d", adId);
        MTRGAdView *adView = mtrg_unity_findStandardAdWithAdId(adId, @"MTRGStandardAdSetWidth");
        if (adView)
        {
            CGRect frame = adView.frame;
            frame.size.width = width;
            adView.frame = frame;
        }
    }];
}

void MTRGStandardAdSetHeight(uint32_t adId, uint32_t height)
{
    [[NSOperationQueue mainQueue] addOperationWithBlock:^
    {
        mtrg_unity_tracer_d(@"MTRGStandardAdSetHeight, adId = %d", adId);
        MTRGAdView *adView = mtrg_unity_findStandardAdWithAdId(adId, @"MTRGStandardAdSetHeight");
        if (adView)
        {
            CGRect frame = adView.frame;
            frame.size.height = height;
            adView.frame = frame;
        }
    }];
}