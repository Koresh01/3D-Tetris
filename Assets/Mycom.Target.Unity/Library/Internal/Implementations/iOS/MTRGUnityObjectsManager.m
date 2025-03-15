#import "MTRGUnityObjectsManager.h"
#import "MTRGUnityTracer.h"

static const uint32_t MTRGUnityUndefinedObjectId = 0;

typedef void (*MTRGCallbackTypeAdId)(uint32_t);
typedef void (*MTRGCallbackTypeOnStandardLoadFailedWithError)(uint32_t, int64_t, const char *);
typedef void (*MTRGCallbackTypeOnInterstitialLoadFailedWithError)(uint32_t, int64_t, const char *);
typedef void (*MTRGCallbackTypeOnRewardedLoadFailedWithError)(uint32_t, int64_t, const char *);
typedef void (*MTRGCallbackTypeOnReward)(uint32_t, const char *);

#pragma mark - interstitial callbacks

MTRGCallbackTypeAdId _MTRGInterstitialAdCallbackOnLoad = NULL;
MTRGCallbackTypeAdId _MTRGInterstitialAdCallbackOnClick = NULL;
MTRGCallbackTypeAdId _MTRGInterstitialAdCallbackOnClose = NULL;
MTRGCallbackTypeAdId _MTRGInterstitialAdCallbackOnVideoComplete = NULL;
MTRGCallbackTypeAdId _MTRGInterstitialAdCallbackOnDisplay = NULL;
MTRGCallbackTypeOnInterstitialLoadFailedWithError _MTRGCallbackTypeOnInterstitialLoadFailedWithError = NULL;

#pragma mark - rewarded callbacks

MTRGCallbackTypeAdId _MTRGRewardedAdCallbackOnLoad = NULL;
MTRGCallbackTypeAdId _MTRGRewardedAdCallbackOnClick = NULL;
MTRGCallbackTypeAdId _MTRGRewardedAdCallbackOnClose = NULL;
MTRGCallbackTypeAdId _MTRGRewardedAdCallbackOnDisplay = NULL;
MTRGCallbackTypeOnReward _MTRGRewardedAdCallbackOnReward = NULL;
MTRGCallbackTypeOnRewardedLoadFailedWithError _MTRGCallbackTypeOnRewardedLoadFailedWithError = NULL;

#pragma mark - standard ads callbacks

MTRGCallbackTypeAdId _MTRGStandardAdCallbackOnAdLoadCompleted = NULL;
MTRGCallbackTypeAdId _MTRGStandardAdCallbackOnAdClicked = NULL;
MTRGCallbackTypeAdId _MTRGStandardAdCallbackOnAdShown = NULL;
MTRGCallbackTypeOnStandardLoadFailedWithError _MTRGCallbackTypeOnStandardLoadFailedWithError = NULL;

@implementation MTRGUnityObjectsManager
{
	NSMutableDictionary<NSNumber *, id> *_registeredObjects;
	uint32_t _activeObjectId;
}

+ (MTRGUnityObjectsManager *)sharedManager
{
	static MTRGUnityObjectsManager *_instance = nil;
	static dispatch_once_t once;
	dispatch_once(&once,
	              ^
	              {
		              _instance = [[MTRGUnityObjectsManager alloc] init];
	              });
	return _instance;
}

- (instancetype)init
{
	self = [super init];
	if (self)
	{
		_registeredObjects = [NSMutableDictionary new];
		_activeObjectId = 0;
	}
	return self;
}

- (uint32_t)numberToObjectId:(NSNumber *)number
{
	return number ? [number unsignedIntValue] : MTRGUnityUndefinedObjectId;
}

- (uint32_t)registerObject:(__strong id)object
{
	if (!object)
	{
		return MTRGUnityUndefinedObjectId;
	}

	@synchronized (self)
	{
		_activeObjectId++;
		uint32_t objectId = _activeObjectId;
		NSNumber *objectIdNum = @((objectId));
		_registeredObjects[objectIdNum] = object;
		return objectId;
	}
}

- (id)findObjectWithId:(uint32_t)objectId
{
	NSNumber *adIdNum = @((objectId));
	if (adIdNum)
	{
		@synchronized (self)
		{
			return _registeredObjects[adIdNum];
		}
	}

	return nil;
}

- (uint32_t)findObjectIdWithObject:(id)object
{
	@synchronized (self)
	{
		for (NSNumber *objectIdTemp in [_registeredObjects allKeys])
		{
			id obj = _registeredObjects[objectIdTemp];
			if (object == obj)
			{
				return [self numberToObjectId:objectIdTemp];
			}
		}
		return MTRGUnityUndefinedObjectId;
	}
}

- (void)unregisterObject:(uint32_t)objectId
{
	NSNumber *adIdNum = @((objectId));
	if (adIdNum)
	{
		@synchronized (self)
		{
			[_registeredObjects removeObjectForKey:adIdNum];
		}
	}
}

#pragma mark - MTRGInterstitialAdDelegate

- (void)onLoadWithInterstitialAd:(MTRGInterstitialAd *)interstitialAd
{
	uint32_t objectId = [self findObjectIdWithObject:interstitialAd];
	if (MTRGUnityUndefinedObjectId == objectId)
	{
		return;
	}

	MTRGCallbackTypeAdId callback = _MTRGInterstitialAdCallbackOnLoad;
	if (callback)
	{
		mtrg_unity_tracer_d(@"MTRGUnityProxy: onLoadWithInterstitialAd");

		callback(objectId);
	}
}

- (void)onLoadFailedWithError:(NSError *)error interstitialAd:(MTRGInterstitialAd *)interstitialAd
{
	uint32_t objectId = [self findObjectIdWithObject:interstitialAd];
	if (MTRGUnityUndefinedObjectId == objectId)
	{
		return;
	}
	
	MTRGCallbackTypeOnInterstitialLoadFailedWithError callback = _MTRGCallbackTypeOnInterstitialLoadFailedWithError;
	if (callback)
	{
		mtrg_unity_tracer_d(@"MTRGUnityProxy:  interstitial ad loading failed: %@", error);
		callback(objectId, error.code, error.localizedDescription.UTF8String);
	}
}

- (void)onClickWithInterstitialAd:(MTRGInterstitialAd *)interstitialAd
{
	uint32_t objectId = [self findObjectIdWithObject:interstitialAd];
	if (MTRGUnityUndefinedObjectId == objectId)
	{
		return;
	}

	MTRGCallbackTypeAdId callback = _MTRGInterstitialAdCallbackOnClick;
	if (callback)
	{
		mtrg_unity_tracer_d(@"MTRGUnityProxy: onClickWithInterstitialAd");

		callback(objectId);
	}
}

- (void)onCloseWithInterstitialAd:(MTRGInterstitialAd *)interstitialAd
{
	uint32_t objectId = [self findObjectIdWithObject:interstitialAd];
	if (MTRGUnityUndefinedObjectId == objectId)
	{
		return;
	}

	MTRGCallbackTypeAdId callback = _MTRGInterstitialAdCallbackOnClose;
	if (callback)
	{
		mtrg_unity_tracer_d(@"MTRGUnityProxy: onCloseWithInterstitialAd");

		callback(objectId);
	}
}

- (void)onVideoCompleteWithInterstitialAd:(MTRGInterstitialAd *)interstitialAd
{
	uint32_t objectId = [self findObjectIdWithObject:interstitialAd];
	if (MTRGUnityUndefinedObjectId == objectId)
	{
		return;
	}

	MTRGCallbackTypeAdId callback = _MTRGInterstitialAdCallbackOnVideoComplete;
	if (callback)
	{
		mtrg_unity_tracer_d(@"MTRGUnityProxy: onVideoCompleteWithInterstitialAd");

		callback(objectId);
	}
}

- (void)onDisplayWithInterstitialAd:(MTRGInterstitialAd *)interstitialAd
{
	uint32_t objectId = [self findObjectIdWithObject:interstitialAd];
	if (MTRGUnityUndefinedObjectId == objectId)
	{
		return;
	}

	MTRGCallbackTypeAdId callback = _MTRGInterstitialAdCallbackOnDisplay;
	if (callback)
	{
		mtrg_unity_tracer_d(@"MTRGUnityProxy: onDisplayWithInterstitialAd");

		callback(objectId);
	}
}

#pragma mark - MTRGRewardedAdDelegate

- (void)onLoadWithRewardedAd:(MTRGRewardedAd *)rewardedAd
{
	uint32_t objectId = [self findObjectIdWithObject:rewardedAd];
	if (MTRGUnityUndefinedObjectId == objectId)
	{
		return;
	}
	
	MTRGCallbackTypeAdId callback = _MTRGRewardedAdCallbackOnLoad;
	if (callback)
	{
		mtrg_unity_tracer_d(@"MTRGUnityProxy: onLoadWithRewardedAd");
		
		callback(objectId);
	}
}

- (void)onLoadFailedWithError:(NSError *)error rewardedAd:(MTRGRewardedAd *)rewardedAd
{
	uint32_t objectId = [self findObjectIdWithObject:rewardedAd];
	if (MTRGUnityUndefinedObjectId == objectId)
	{
		return;
	}
	
	MTRGCallbackTypeOnRewardedLoadFailedWithError callback = _MTRGCallbackTypeOnRewardedLoadFailedWithError;
	if (callback)
	{
		mtrg_unity_tracer_d(@"MTRGUnityProxy: rewarded ad loading failed: %@", error);
		callback(objectId, error.code, error.localizedDescription.UTF8String);
	}
}

- (void)onClickWithRewardedAd:(MTRGRewardedAd *)rewardedAd
{
	uint32_t objectId = [self findObjectIdWithObject:rewardedAd];
	if (MTRGUnityUndefinedObjectId == objectId)
	{
		return;
	}
	
	MTRGCallbackTypeAdId callback = _MTRGRewardedAdCallbackOnClick;
	if (callback)
	{
		mtrg_unity_tracer_d(@"MTRGUnityProxy: onClickWithRewardedAd");
		
		callback(objectId);
	}
}

- (void)onCloseWithRewardedAd:(MTRGRewardedAd *)rewardedAd
{
	uint32_t objectId = [self findObjectIdWithObject:rewardedAd];
	if (MTRGUnityUndefinedObjectId == objectId)
	{
		return;
	}
	
	MTRGCallbackTypeAdId callback = _MTRGRewardedAdCallbackOnClose;
	if (callback)
	{
		mtrg_unity_tracer_d(@"MTRGUnityProxy: onCloseWithRewardedAd");
		
		callback(objectId);
	}
}

- (void)onReward:(MTRGReward *)reward rewardedAd:(MTRGRewardedAd *)rewardedAd
{
	uint32_t objectId = [self findObjectIdWithObject:rewardedAd];
	if (MTRGUnityUndefinedObjectId == objectId)
	{
		return;
	}
	
	MTRGCallbackTypeOnReward callback = _MTRGRewardedAdCallbackOnReward;
	if (callback)
	{
		mtrg_unity_tracer_d(@"MTRGUnityProxy: onVideoCompleteWithRewardedAd");
		
		NSString *type = reward.type;
		callback(objectId, (type ? type : @"").UTF8String);
	}
}

- (void)onDisplayWithRewardedAd:(MTRGRewardedAd *)rewardedAd
{
	uint32_t objectId = [self findObjectIdWithObject:rewardedAd];
	if (MTRGUnityUndefinedObjectId == objectId)
	{
		return;
	}
	
	MTRGCallbackTypeAdId callback = _MTRGRewardedAdCallbackOnDisplay;
	if (callback)
	{
		mtrg_unity_tracer_d(@"MTRGUnityProxy: onDisplayWithRewardedAd");
		
		callback(objectId);
	}
}

#pragma mark -- MTRGAdViewDelegate

- (void)onLoadWithAdView:(MTRGAdView *)adView
{
	uint32_t objectId = [self findObjectIdWithObject:adView];
	if (MTRGUnityUndefinedObjectId == objectId)
	{
		return;
	}
	
	MTRGCallbackTypeAdId callback = _MTRGStandardAdCallbackOnAdLoadCompleted;
	if (callback)
	{
		mtrg_unity_tracer_d(@"MTRGUnityProxy: onLoadWithAdView");
		
		callback(objectId);
	}
}

- (void)onLoadFailedWithError:(NSError *)error adView:(MTRGAdView *)adView
{
	uint32_t objectId = [self findObjectIdWithObject:adView];
	if (MTRGUnityUndefinedObjectId == objectId)
	{
		return;
	}
	
	MTRGCallbackTypeOnStandardLoadFailedWithError callback = _MTRGCallbackTypeOnStandardLoadFailedWithError;
	if (callback)
	{
		mtrg_unity_tracer_d(@"MTRGUnityProxy:  standard ad loading failed: %@", error);
		callback(objectId, error.code, error.localizedDescription.UTF8String);
	}
}

- (void)onAdClickWithAdView:(MTRGAdView *)adView
{
	uint32_t objectId = [self findObjectIdWithObject:adView];
	if (MTRGUnityUndefinedObjectId == objectId)
	{
		return;
	}
	
	MTRGCallbackTypeAdId callback = _MTRGStandardAdCallbackOnAdClicked;
	if (callback)
	{
		mtrg_unity_tracer_d(@"MTRGUnityProxy: onAdClickWithAdView");
		
		callback(objectId);
	}
}

- (void)onAdShowWithAdView:(MTRGAdView *)adView
{
	uint32_t objectId = [self findObjectIdWithObject:adView];
	if (MTRGUnityUndefinedObjectId == objectId)
	{
		return;
	}
	
	MTRGCallbackTypeAdId callback = _MTRGStandardAdCallbackOnAdShown;
	if (callback)
	{
		mtrg_unity_tracer_d(@"MTRGUnityProxy: onAdShowWithAdView");
		
		callback(objectId);
	}
}

@end

#pragma mark -- standard ads callbacks setters

void MTRGStandardAdSetCallbackOnAdLoadCompleted(MTRGCallbackTypeAdId callback)
{
	mtrg_unity_tracer_d(@"MTRGStandardAdSetCallbackOnAdLoadCompleted");
	_MTRGStandardAdCallbackOnAdLoadCompleted = callback;
}

void MTRGStandardAdSetCallbackOnAdLoadFailed(MTRGCallbackTypeOnStandardLoadFailedWithError callback)
{
	mtrg_unity_tracer_d(@"MTRGStandardAdSetCallbackOnAdLoadFailed");
	_MTRGCallbackTypeOnStandardLoadFailedWithError = callback;
}

void MTRGStandardAdSetCallbackOnAdClicked(MTRGCallbackTypeAdId callback)
{
	mtrg_unity_tracer_d(@"MTRGStandardAdSetCallbackOnAdClicked");
	_MTRGStandardAdCallbackOnAdClicked = callback;
}

void MTRGStandardAdSetCallbackOnAdShown(MTRGCallbackTypeAdId callback)
{
	mtrg_unity_tracer_d(@"MTRGStandardAdSetCallbackOnAdShown");
	_MTRGStandardAdCallbackOnAdShown = callback;
}

#pragma mark - interstitial callbacks setters

void MTRGInterstitialAdSetCallbackOnLoad(MTRGCallbackTypeAdId callback)
{
	mtrg_unity_tracer_d(@"MTRGInterstitialAdSetCallbackOnLoad");
	_MTRGInterstitialAdCallbackOnLoad = callback;
}

void MTRGInterstitialAdSetCallbackOnAdLoadFailed(MTRGCallbackTypeOnInterstitialLoadFailedWithError callback)
{
	mtrg_unity_tracer_d(@"MTRGInterstitialAdSetCallbackOnNoAdWithReason");
	_MTRGCallbackTypeOnInterstitialLoadFailedWithError = callback;
}

void MTRGInterstitialAdSetCallbackOnClick(MTRGCallbackTypeAdId callback)
{
	mtrg_unity_tracer_d(@"MTRGInterstitialAdSetCallbackOnClick");
	_MTRGInterstitialAdCallbackOnClick = callback;
}

void MTRGInterstitialAdSetCallbackOnClose(MTRGCallbackTypeAdId callback)
{
	mtrg_unity_tracer_d(@"MTRGInterstitialAdSetCallbackOnClose");
	_MTRGInterstitialAdCallbackOnClose = callback;
}

void MTRGInterstitialAdSetCallbackOnVideoComplete(MTRGCallbackTypeAdId callback)
{
	mtrg_unity_tracer_d(@"MTRGInterstitialAdSetCallbackOnVideoComplete");
	_MTRGInterstitialAdCallbackOnVideoComplete = callback;
}

void MTRGInterstitialAdSetCallbackOnDisplay(MTRGCallbackTypeAdId callback)
{
	mtrg_unity_tracer_d(@"MTRGInterstitialAdSetCallbackOnDisplay");
	_MTRGInterstitialAdCallbackOnDisplay = callback;
}

#pragma mark - rewarded callbacks setters

void MTRGRewardedAdSetCallbackOnLoad(MTRGCallbackTypeAdId callback)
{
	mtrg_unity_tracer_d(@"MTRGRewardedAdSetCallbackOnLoad");
	_MTRGRewardedAdCallbackOnLoad = callback;
}

void MTRGRewardedAdSetCallbackOnAdLoadFailed(MTRGCallbackTypeOnRewardedLoadFailedWithError callback)
{
	mtrg_unity_tracer_d(@"MTRGRewardedAdSetCallbackOnNoAdWithReason");
	_MTRGCallbackTypeOnRewardedLoadFailedWithError = callback;
}

void MTRGRewardedAdSetCallbackOnClick(MTRGCallbackTypeAdId callback)
{
	mtrg_unity_tracer_d(@"MTRGRewardedAdSetCallbackOnClick");
	_MTRGRewardedAdCallbackOnClick = callback;
}

void MTRGRewardedAdSetCallbackOnClose(MTRGCallbackTypeAdId callback)
{
	mtrg_unity_tracer_d(@"MTRGRewardedAdSetCallbackOnClose");
	_MTRGRewardedAdCallbackOnClose = callback;
}

void MTRGRewardedAdSetCallbackOnReward(MTRGCallbackTypeOnReward callback)
{
	mtrg_unity_tracer_d(@"MTRGRewardedAdSetCallbackOnVideoComplete");
	_MTRGRewardedAdCallbackOnReward = callback;
}

void MTRGRewardedAdSetCallbackOnDisplay(MTRGCallbackTypeAdId callback)
{
	mtrg_unity_tracer_d(@"MTRGRewardedAdSetCallbackOnDisplay");
	_MTRGRewardedAdCallbackOnDisplay = callback;
}
