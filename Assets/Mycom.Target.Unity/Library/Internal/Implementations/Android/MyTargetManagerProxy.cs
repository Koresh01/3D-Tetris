#if UNITY_ANDROID
namespace Mycom.Target.Unity.Internal
{
    using System;
    using Common;
    using UnityEngine;
    using Implementations.Android;

    internal static partial class MyTargetManagerProxy
    {
        const String ManagerClassName = "com.my.target.common.MyTargetManager";

        static partial void SetDebugModeImpl(Boolean debugMode)
        {
            using var javaClass = new AndroidJavaClass(ManagerClassName);

            javaClass.CallStatic("setDebugMode", debugMode);
        }

        static partial void SetSdkConfigImpl(MyTargetConfig config)
        {
            using var builder = new AndroidJavaObject("com.my.target.common.MyTargetConfig$Builder");

            var javaConfig = builder.Call<AndroidJavaObject>("withTrackingEnvironment", config.IsTrackingEnvironmentEnabled)
                                    .Call<AndroidJavaObject>("withTrackingLocation", config.IsTrackingLocationEnabled)
                                    .Call<AndroidJavaObject>("withTestDevices", PlatformHelper.CreateJavaStringArray(config.TestDevices))
                                    .Call<AndroidJavaObject>("build");

            using var javaClass = new AndroidJavaClass(ManagerClassName);

            javaClass.CallStatic("setSdkConfig", javaConfig);
        }

        static partial void GetSdkConfigImpl(MyTargetConfig.Builder builder)
        {
            using var javaClass = new AndroidJavaClass(ManagerClassName);

            var javaConfig = javaClass.CallStatic<AndroidJavaObject>("getSdkConfig");

            String[] testDevices = PlatformHelper.CreateStringArray(javaConfig.Get<AndroidJavaObject>("testDevices"));

            builder.WithTrackingEnvironment(javaConfig.Get<Boolean>("isTrackingEnvironmentEnabled"))
                   .WithTrackingLocation(javaConfig.Get<Boolean>("isTrackingLocationEnabled"))
                   .WithTestDevices(testDevices);
        }

        static partial void InitSdkImpl()
        {
            using var javaClass = new AndroidJavaClass(ManagerClassName);

            javaClass.CallStatic("initSdk", PlatformHelper.ApplicationContext);
        }
    }
}
#endif