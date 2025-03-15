#import "PlatformUtils.h"
#import "MTRGUnityObjectsManager.h"
#import "MTRGUnityTracer.h"

MTRGRewardedAd *mtrg_unity_findRewardedAdWithAdId(uint32_t adId, NSString *logContext)
{
	id object = [MTRGUnityObjectsManager.sharedManager findObjectWithId:adId];
	if (object)
	{
		if ([object isKindOfClass:[MTRGRewardedAd class]])
		{
			return (MTRGRewardedAd *) (object);
		}

		mtrg_unity_tracer_d(@"%@ error, invalid object type for adId = %d", logContext, adId);
	}
	else
	{
		mtrg_unity_tracer_d(@"%@ error, object with adId = %d not found", logContext, adId);
	}
	return nil;
}

uint32_t MTRGRewardedAdCreate(uint32_t slotId)
{
	mtrg_unity_tracer_d(@"MTRGRewardedAdCreate, slotId = %d", slotId);

	MTRGRewardedAd *ad = [[MTRGRewardedAd alloc] initWithSlotId:slotId];

	MTRGUnityObjectsManager *sharedManager = MTRGUnityObjectsManager.sharedManager;
	if (ad)
	{
		ad.delegate = sharedManager;
	}

	uint32_t adId = [sharedManager registerObject:ad];

	mtrg_unity_tracer_d(@"MTRGRewardedAd created, adId = %d", adId);

	return adId;
}

void MTRGRewardedAdDelete(uint32_t adId)
{
	mtrg_unity_tracer_d(@"MTRGRewardedAdDelete, adId = %d", adId);

	[MTRGUnityObjectsManager.sharedManager unregisterObject:adId];

	mtrg_unity_tracer_d(@"MTRGRewardedAd deleted, adId = %d", adId);
}

void MTRGRewardedAdLoad(uint32_t adId)
{
	[NSOperationQueue.mainQueue addOperationWithBlock:^
	{
		mtrg_unity_tracer_d(@"MTRGRewardedAdLoad, adId = %d", adId);

		MTRGRewardedAd *rewardedAd = mtrg_unity_findRewardedAdWithAdId(adId, @"MTRGRewardedAdLoad");
		if (rewardedAd)
		{
			[rewardedAd load];
		}
	}];
}

void MTRGRewardedAdShow(uint32_t adId)
{
	[NSOperationQueue.mainQueue addOperationWithBlock:^
	{
		mtrg_unity_tracer_d(@"MTRGRewardedAdShow, adId = %d", adId);

		UIViewController *controller = mtrg_unity_topViewController();
		if (controller)
		{
			MTRGRewardedAd *rewardedAd = mtrg_unity_findRewardedAdWithAdId(adId, @"MTRGRewardedAdShow");
			if (rewardedAd)
			{
				[rewardedAd showWithController:controller];
			}
		}
		else
		{
			mtrg_unity_tracer_e(@"MTRGRewardedAdShow - UI not found");
		}
	}];
}

void MTRGRewardedAdClose(uint32_t adId)
{
	[NSOperationQueue.mainQueue addOperationWithBlock:^
	{
		mtrg_unity_tracer_d(@"MTRGRewardedAdClose, adId = %d", adId);

		MTRGRewardedAd *rewardedAd = mtrg_unity_findRewardedAdWithAdId(adId, @"MTRGRewardedAdClose");
		if (rewardedAd)
		{
			[rewardedAd close];
		}
	}];
}