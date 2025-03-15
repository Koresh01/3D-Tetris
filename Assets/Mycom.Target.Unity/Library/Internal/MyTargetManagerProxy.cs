namespace Mycom.Target.Unity.Internal
{
    using System;
    using Common;

    internal static partial class MyTargetManagerProxy
    {
        public static Boolean DebugMode
        {
            set
            {
                try
                {
                    SetDebugModeImpl(value);
                }
                catch (Exception ex)
                {
                    MyTargetLogger.Log(ex.ToString());
                }
            }
        }

        public static MyTargetConfig Config
        {
            get
            {
                var builder = new MyTargetConfig.Builder();

                try
                {
                    GetSdkConfigImpl(builder);
                }
                catch (Exception ex)
                {
                    MyTargetLogger.Log(ex.ToString());
                }

                return builder.Build();
            }
            set
            {
                try
                {
                    SetSdkConfigImpl(value);
                }
                catch (Exception ex)
                {
                    MyTargetLogger.Log(ex.ToString());
                }
            }
        }

        public static void InitSdk()
        {
            try
            {
                InitSdkImpl();
            }
            catch (Exception ex)
            {
                MyTargetLogger.Log(ex.ToString());
            }
        }

        static partial void SetDebugModeImpl(Boolean debugMode);

        static partial void SetSdkConfigImpl(MyTargetConfig config);

        static partial void GetSdkConfigImpl(MyTargetConfig.Builder builder);

        static partial void InitSdkImpl();
    }

#if !UNITY_IOS && !UNITY_ANDROID
    internal static partial class MyTargetManagerProxy
    {
        static partial void SetDebugModeImpl(Boolean debugMode) { }

        static partial void SetSdkConfigImpl(MyTargetConfig config) { }

        static partial void GetSdkConfigImpl(MyTargetConfig.Builder builder) { }

        static partial void InitSdkImpl() { }

    }
#endif

}