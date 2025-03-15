#import <MyTargetSDK/MTRGPrivacy.h>

struct MTRGPrivacyValue
{
    int32_t userConsent;
    int32_t ccpaUserConsent;
    int32_t iabUserConsent;
    uint16_t userAgeRestricted;
};

void MTRGSetUserConsent(bool userConsent)
{
    BOOL value = userConsent == true ? YES : NO;
    [MTRGPrivacy setUserConsent:value];
}

void MTRGSetCcpaUserConsent(bool ccpaUserConsent)
{
    BOOL value = ccpaUserConsent == true ? YES : NO;
    [MTRGPrivacy setCcpaUserConsent:value];
}

void MTRGSetIabUserConsent(bool iabUserConsent)
{
    BOOL value = iabUserConsent == true ? YES : NO;
    [MTRGPrivacy setIABUserConsent:value];
}

void MTRGSetUserAgeRestricted(bool userAgeRestricted)
{
    BOOL value = userAgeRestricted == true ? YES : NO;
    [MTRGPrivacy setUserAgeRestricted:value];
}

struct MTRGPrivacyValue MTRGGetCurrentPrivacy()
{
    struct MTRGPrivacyValue result;
    const MTRGPrivacy* currentValue = MTRGPrivacy.currentPrivacy;

    NSNumber* userConsent = [currentValue userConsent];
    result.userConsent = userConsent == nil ? -1 : userConsent.intValue;

    NSNumber* ccpaUserConsent = [currentValue ccpaUserConsent];
    result.ccpaUserConsent = ccpaUserConsent == nil ? -1 : ccpaUserConsent.intValue;

    NSNumber* iabUserConsent = [currentValue iABUserConsent];
    result.iabUserConsent = iabUserConsent == nil ? -1 : iabUserConsent.intValue;

    result.userAgeRestricted = (uint16_t) ([currentValue userAgeRestricted] == YES);

    return result;
}
