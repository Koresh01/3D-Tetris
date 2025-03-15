namespace Mycom.Target.Unity.Common
{
    using System;
    using Internal;

    public static class MyTargetManager
    {
        public static Boolean DebugMode
        {
            set => MyTargetManagerProxy.DebugMode = value;
        }

        public static MyTargetConfig Config
        {
            get => MyTargetManagerProxy.Config;
            set => MyTargetManagerProxy.Config = value;
        }

        public static void InitSdk() => MyTargetManagerProxy.InitSdk();
    }
}