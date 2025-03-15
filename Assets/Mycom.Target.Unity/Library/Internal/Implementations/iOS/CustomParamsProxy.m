#import "MTRGUnityObjectsManager.h"
#import "MTRGUnityTracer.h"
#import <MyTargetSDK/MTRGCustomParams.h>

MTRGCustomParams *mtrg_unity_customParamsWithAdId(uint32_t adId)
{
    id object = [[MTRGUnityObjectsManager sharedManager] findObjectWithId:adId];
    if (object)
    {
        if ([object isKindOfClass:[MTRGInterstitialAd class]])
        {
            return ((MTRGInterstitialAd *) object).customParams;
        }

        if ([object isKindOfClass:[MTRGAdView class]])
        {
            return ((MTRGAdView *) object).customParams;
        }
    }

    return nil;
}

void MTRGCustomParamsSetAge(uint32_t adId, uint32_t value)
{
    MTRGCustomParams *customParams = mtrg_unity_customParamsWithAdId(adId);
    if (customParams)
    {
        customParams.age = @(value);
    }
    else
    {
        mtrg_unity_tracer_e(@"MTRGCustomParamsSetAge: custom params not found for adId = %d", adId);
    }
}

void MTRGCustomParamsResetAge(uint32_t adId)
{
    MTRGCustomParams *customParams = mtrg_unity_customParamsWithAdId(adId);
    if (customParams)
    {
        customParams.age = nil;
    }
    else
    {
        mtrg_unity_tracer_e(@"MTRGCustomParamsResetAge: custom params not found for adId = %d", adId);
    }
}

void MTRGCustomParamsSetEmail(uint16_t adId, const char *value)
{
    MTRGCustomParams *customParams = mtrg_unity_customParamsWithAdId(adId);
    if (customParams)
    {
        customParams.email = value ? [NSString stringWithUTF8String:value] : nil;
    }
    else
    {
        mtrg_unity_tracer_e(@"MTRGCustomParamsSetEmail: custom params not found for adId = %d", adId);
    }
}

void MTRGCustomParamsSetGender(uint32_t adId, int32_t value)
{
    MTRGCustomParams *customParams = mtrg_unity_customParamsWithAdId(adId);
    if (customParams)
    {
        switch (value)
        {
            case 0:
                customParams.gender = MTRGGenderUnknown;
                break;
            case 1:
                customParams.gender = MTRGGenderMale;
                break;
            case 2:
                customParams.gender = MTRGGenderFemale;
                break;
            default:
                customParams.gender = MTRGGenderUnspecified;
                break;
        }
    }
    else
    {
        mtrg_unity_tracer_e(@"MTRGCustomParamsSetGender: custom params not found for adId = %d", adId);
    }
}

void MTRGCustomParamsSetIcqIds(uint32_t adId, const char *value)
{
    MTRGCustomParams *customParams = mtrg_unity_customParamsWithAdId(adId);
    if (customParams)
    {
        customParams.icqId = value ? [NSString stringWithUTF8String:value] : nil;
    }
    else
    {
        mtrg_unity_tracer_e(@"MTRGCustomParamsSetIcqIds: custom params not found for adId = %d", adId);
    }
}

void MTRGCustomParamsSetLanguage(uint32_t adId, const char *value)
{
    MTRGCustomParams *customParams = mtrg_unity_customParamsWithAdId(adId);
    if (customParams)
    {
        customParams.language = value ? [NSString stringWithUTF8String:value] : nil;
    }
    else
    {
        mtrg_unity_tracer_e(@"MTRGCustomParamsSetLanguage: custom params not found for adId = %d", adId);
    }
}

void MTRGCustomParamsSetMrgsAppId(uint32_t adId, const char *value)
{
    MTRGCustomParams *customParams = mtrg_unity_customParamsWithAdId(adId);
    if (customParams)
    {
        customParams.mrgsAppId = value ? [NSString stringWithUTF8String:value] : nil;
    }
    else
    {
        mtrg_unity_tracer_e(@"MTRGCustomParamsSetMrgsAppId: custom params not found for adId = %d", adId);
    }
}

void MTRGCustomParamsSetMrgsId(uint32_t adId, const char *value)
{
    MTRGCustomParams *customParams = mtrg_unity_customParamsWithAdId(adId);
    if (customParams)
    {
        customParams.mrgsDeviceId = value ? [NSString stringWithUTF8String:value] : nil;
    }
    else
    {
        mtrg_unity_tracer_e(@"MTRGCustomParamsSetMrgsId: custom params not found for adId = %d", adId);
    }
}

void MTRGCustomParamsSetMrgsUserId(uint32_t adId, const char *value)
{
    MTRGCustomParams *customParams = mtrg_unity_customParamsWithAdId(adId);
    if (customParams)
    {
        customParams.mrgsUserId = value ? [NSString stringWithUTF8String:value] : nil;
    }
    else
    {
        mtrg_unity_tracer_e(@"MTRGCustomParamsSetMrgsUserId: custom params not found for adId = %d", adId);
    }
}

void MTRGCustomParamsSetOkIds(uint32_t adId, const char *value)
{
    MTRGCustomParams *customParams = mtrg_unity_customParamsWithAdId(adId);
    if (customParams)
    {
        customParams.okId = value ? [NSString stringWithUTF8String:value] : nil;
    }
    else
    {
        mtrg_unity_tracer_e(@"MTRGCustomParamsSetOkIds: custom params not found for adId = %d", adId);
    }
}

void MTRGCustomParamsSetVkIds(uint32_t adId, const char *value)
{
    MTRGCustomParams *customParams = mtrg_unity_customParamsWithAdId(adId);
    if (customParams)
    {
        customParams.vkId = value ? [NSString stringWithUTF8String:value] : nil;
    }
    else
    {
        mtrg_unity_tracer_e(@"MTRGCustomParamsSetVkIds: custom params not found for adId = %d", adId);
    }
}

void MTRGCustomParamsSetCustomUserIds(uint32_t adId, const char *value)
{
    MTRGCustomParams *customParams = mtrg_unity_customParamsWithAdId(adId);
    if (customParams)
    {
        customParams.customUserId = value ? [NSString stringWithUTF8String:value] : nil;
    }
    else
    {
        mtrg_unity_tracer_e(@"MTRGCustomParamsSetCustomUserIds: custom params not found for adId = %d", adId);
    }
}

void MTRGCustomParamsSetCustomParam(uint32_t adId, const char *key, const char *value)
{
    if (key)
    {
        NSString *keyStr = [NSString stringWithUTF8String:key];
        if (keyStr)
        {
            MTRGCustomParams *customParams = mtrg_unity_customParamsWithAdId(adId);
            if (customParams)
            {
                NSString *valStr = value ? [NSString stringWithUTF8String:value] : nil;
                [customParams setCustomParam:valStr forKey:keyStr];
            }
        }
    }
    else
    {
        mtrg_unity_tracer_e(@"MTRGCustomParamsSetCustomParam: custom params not found for adId = %d", adId);
    }
}