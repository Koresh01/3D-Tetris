namespace Mycom.Target.Unity.Internal
{
    using System;

    internal static partial class MyTargetPrivacyProxy
    {
        internal static Boolean UserConsent
        {
            set
            {
                try
                {
                    SetUserConsent(value);
                }
                catch (Exception ex)
                {
                    MyTargetLogger.Log(ex.ToString());
                }
            }
        }

        internal static Boolean CcpaUserConsent
        {
            set
            {
                try
                {
                    SetCcpaUserConsent(value);
                }
                catch (Exception ex)
                {
                    MyTargetLogger.Log(ex.ToString());
                }
            }
        }

        internal static Boolean IabUserConsent
        {
            set
            {
                try
                {
                    SetIabUserConsent(value);
                }
                catch (Exception ex)
                {
                    MyTargetLogger.Log(ex.ToString());
                }
            }
        }

        internal static Boolean UserAgeRestricted
        {
            set
            {
                try
                {
                    SetUserAgeRestricted(value);
                }
                catch (Exception ex)
                {
                    MyTargetLogger.Log(ex.ToString());
                }
            }
        }

        internal static MyTargetPrivacyValue CurrentPrivacy
        {
            get
            {
                MyTargetPrivacyValue result = new MyTargetPrivacyValue();
                try
                {
                    GetCurrentPrivacy(ref result);
                }
                catch (Exception ex)
                {
                    MyTargetLogger.Log(ex.ToString());
                }
                return result;
            }
        }

        static partial void SetUserConsent(Boolean userConsent);

        static partial void SetCcpaUserConsent(Boolean ccpaUserConsent);

        static partial void SetIabUserConsent(Boolean iabUserConsent);

        static partial void SetUserAgeRestricted(Boolean userAgeRestricted);

        static partial void GetCurrentPrivacy(ref MyTargetPrivacyValue value);

        internal struct MyTargetPrivacyValue
        {
            internal Boolean? userConsent;
            internal Boolean? ccpaUserConsent;
            internal Boolean? iabUserConsent;
            internal Boolean userAgeRestricted;
        }
    }

#if !UNITY_IOS && !UNITY_ANDROID
    internal static partial class MyTargetPrivacyProxy
    {
        static partial void SetUserConsent(Boolean userConsent) { }

        static partial void SetCcpaUserConsent(Boolean ccpaUserConsent) { }

        static partial void SetIabUserConsent(Boolean iabUserConsent) { }

        static partial void SetUserAgeRestricted(Boolean userAgeRestricted) { }

        static partial void GetCurrentPrivacy(ref MyTargetPrivacyValue value) { }
    }
#endif

}