#import <MyTargetSDK/MyTargetSDK-Swift.h>
#import <MyTargetSDK/MTRGConfig.h>

struct MTRGConfigProxy
{
	bool isTrackingLocationEnabled;
	unichar *testDevices;
};

void MTRGSetDebugMode(bool value)
{
	MTRGManager.debugMode = value;
}

void MTRGInitSdk()
{
	[MTRGManager initSdk];
}

struct MTRGConfigProxy MTRGGetSdkConfig()
{
	MTRGConfig *config = MTRGManager.sdkConfig;

	struct MTRGConfigProxy result;

	NSString *testDevicesResult = [config.testDevices componentsJoinedByString:@","];

	NSUInteger strLen = [testDevicesResult lengthOfBytesUsingEncoding:NSUnicodeStringEncoding] + 1;

	result.testDevices = malloc(strLen);
	[testDevicesResult getCString:result.testDevices maxLength:strLen encoding:NSUnicodeStringEncoding];

	result.isTrackingLocationEnabled = config.isTrackLocationEnabled;

	return result;
}

void MTRGSetSdkConfig(struct MTRGConfigProxy config)
{
	MTRGConfigBuilder *builder = MTRGConfig.newBuilder;
	[builder withTrackingLocation:config.isTrackingLocationEnabled];
	[builder withTestDevices:[[NSString stringWithFormat:@"%S", config.testDevices] componentsSeparatedByString:@","]];

	MTRGManager.sdkConfig = builder.build;
}
