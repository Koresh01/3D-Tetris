#import <MyTargetSDK/MTRGInterstitialAd.h>
#import <MyTargetSDK/MTRGRewardedAd.h>
#import <MyTargetSDK/MTRGReward.h>
#import <MyTargetSDK/MTRGAdView.h>
#import <Foundation/Foundation.h>

@interface MTRGUnityObjectsManager : NSObject <MTRGInterstitialAdDelegate, MTRGRewardedAdDelegate, MTRGAdViewDelegate>

+ (MTRGUnityObjectsManager *)sharedManager;

- (instancetype)init;

- (uint32_t)numberToObjectId:(NSNumber *)number;

- (uint32_t)registerObject:(__strong id)object;

- (id)findObjectWithId:(uint32_t)objectId;

- (uint32_t)findObjectIdWithObject:(id)object;

- (void)unregisterObject:(uint32_t)objectId;

@end