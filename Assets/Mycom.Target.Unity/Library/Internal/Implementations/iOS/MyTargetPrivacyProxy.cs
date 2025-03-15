#if UNITY_IOS

using System;
using System.Runtime.InteropServices;

namespace Mycom.Target.Unity.Internal
{
    internal static partial class MyTargetPrivacyProxy
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct MTRGPrivacyValue
        {
            public Int32 userConsent;
            public Int32 ccpaUserConsent;
            public Int32 iabUserConsent;
            public UInt16 userAgeRestricted;
        }

        [DllImport("__Internal")]
        private static extern void MTRGSetUserConsent(Boolean userConsent);

        [DllImport("__Internal")]
        private static extern void MTRGSetCcpaUserConsent(Boolean ccpaUserConsent);

        [DllImport("__Internal")]
        private static extern void MTRGSetIabUserConsent(Boolean iabUserConsent);

        [DllImport("__Internal")]
        private static extern void MTRGSetUserAgeRestricted(Boolean userAgeRestricted);

        [DllImport("__Internal")]
        private static extern MTRGPrivacyValue MTRGGetCurrentPrivacy();

        static partial void SetUserConsent(Boolean userConsent)
        {
            MTRGSetUserConsent(userConsent);
        }

        static partial void SetCcpaUserConsent(Boolean ccpaUserConsent)
        {
            MTRGSetCcpaUserConsent(ccpaUserConsent);
        }

        static partial void SetIabUserConsent(Boolean iabUserConsent)
        {
            MTRGSetIabUserConsent(iabUserConsent);
        }

        static partial void SetUserAgeRestricted(Boolean userAgeRestricted)
        {
            MTRGSetUserAgeRestricted(userAgeRestricted);
        }

        static partial void GetCurrentPrivacy(ref MyTargetPrivacyValue value)
        {
            var nativeValue = MTRGGetCurrentPrivacy();
            value.userConsent = nativeValue.userConsent < 0 ? (Boolean?)null : nativeValue.userConsent == 1;
            value.ccpaUserConsent = nativeValue.ccpaUserConsent < 0 ? (Boolean?)null : nativeValue.ccpaUserConsent == 1;
            value.iabUserConsent = nativeValue.iabUserConsent < 0 ? (Boolean?)null : nativeValue.iabUserConsent == 1;
            value.userAgeRestricted = nativeValue.userAgeRestricted > 0;
        }
    }
}

#endif