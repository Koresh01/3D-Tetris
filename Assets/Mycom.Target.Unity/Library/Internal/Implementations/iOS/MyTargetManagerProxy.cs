#if UNITY_IOS
namespace Mycom.Target.Unity.Internal
{
    using System;
    using System.Runtime.InteropServices;
    using Common;
    
    internal static partial class MyTargetManagerProxy
    {
        [DllImport("__Internal")]
        private static extern void MTRGSetDebugMode(bool value);

        [DllImport("__Internal")]
        private static extern void MTRGInitSdk();

        [DllImport("__Internal")]
        private static extern MTRGConfigProxy MTRGGetSdkConfig();

        [DllImport("__Internal")]
        private static extern void MTRGSetSdkConfig(MTRGConfigProxy config);

        static partial void SetDebugModeImpl(Boolean debugMode) => MTRGSetDebugMode(debugMode);

        static partial void SetSdkConfigImpl(MyTargetConfig config) => MTRGSetSdkConfig(new MTRGConfigProxy
        {
            isTrackingEnvironmentEnabled = config.IsTrackingEnvironmentEnabled,
            testDevices = String.Join(",", config.TestDevices)
        });

        static partial void GetSdkConfigImpl(MyTargetConfig.Builder builder)
        {
            var config = MTRGGetSdkConfig();

            builder.WithTestDevices(config.testDevices.Split(","))
                   .WithTrackingLocation(false)
                   .WithTrackingEnvironment(config.isTrackingEnvironmentEnabled);
        }

        static partial void InitSdkImpl() => MTRGInitSdk();

        [StructLayout(LayoutKind.Sequential)]
        struct MTRGConfigProxy
        {
            public bool isTrackingEnvironmentEnabled;

            [MarshalAs(UnmanagedType.LPWStr)]
            public string testDevices;
        }

    }
}
#endif