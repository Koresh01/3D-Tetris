#if UNITY_ANDROID
namespace Mycom.Target.Unity.Internal
{
    using System;
    using UnityEngine;

    internal static partial class MyTargetPrivacyProxy
    {
        private const String ClassName = "com.my.target.common.MyTargetPrivacy";

        static partial void SetUserConsent(Boolean userConsent)
        {
            using var javaClass = new AndroidJavaClass(ClassName);

            javaClass.CallStatic("setUserConsent", userConsent);
        }

        static partial void SetUserAgeRestricted(Boolean userAgeRestricted)
        {
            using var javaClass = new AndroidJavaClass(ClassName);

            javaClass.CallStatic("setUserAgeRestricted", userAgeRestricted);
        }

        static partial void SetCcpaUserConsent(Boolean ccpaUserConsent)
        {
            using var javaClass = new AndroidJavaClass(ClassName);

            javaClass.CallStatic("setCcpaUserConsent", ccpaUserConsent);
        }

        static partial void SetIabUserConsent(Boolean iabUserConsent)
        {
            using var javaClass = new AndroidJavaClass(ClassName);

            javaClass.CallStatic("setIabUserConsent", iabUserConsent);
        }

        static partial void GetCurrentPrivacy(ref MyTargetPrivacyValue value)
        {
            using var javaClass = new AndroidJavaClass(ClassName);
            using var currentPrivacyJava = javaClass.CallStatic<AndroidJavaObject>("currentPrivacy");
            using (var userConsentJava = currentPrivacyJava.Get<AndroidJavaObject>("userConsent"))
            {
                if (userConsentJava != null)
                {
                    value.userConsent = userConsentJava.Call<Boolean>("booleanValue");
                }
            }
            using (var ccpaUserConsentJava = currentPrivacyJava.Get<AndroidJavaObject>("ccpaUserConsent"))
            {
                if (ccpaUserConsentJava != null)
                {
                    value.ccpaUserConsent = ccpaUserConsentJava.Call<Boolean>("booleanValue");
                }
            }
            using (var iabUserConsentJava = currentPrivacyJava.Get<AndroidJavaObject>("iabUserConsent"))
            {
                if (iabUserConsentJava != null)
                {
                    value.iabUserConsent = iabUserConsentJava.Call<Boolean>("booleanValue");
                }
            }
            value.userAgeRestricted = currentPrivacyJava.Get<Boolean>("userAgeRestricted");
        }
    }
}

#endif