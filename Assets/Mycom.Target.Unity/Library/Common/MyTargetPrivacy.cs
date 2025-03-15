namespace Mycom.Target.Unity.Common
{
    using System;
    using Internal;

    public sealed class MyTargetPrivacy
    {
        public static Boolean UserConsent
        {
            set { MyTargetPrivacyProxy.UserConsent = value; }
        }

        public static Boolean CcpaUserConsent
        {
            set { MyTargetPrivacyProxy.CcpaUserConsent = value; }
        }

        public static Boolean IabUserConsent
        {
            set { MyTargetPrivacyProxy.IabUserConsent = value; }
        }

        public static Boolean UserAgeRestricted
        {
            set { MyTargetPrivacyProxy.UserAgeRestricted = value; }
        }

        public static MyTargetPrivacy CurrentPrivacy
        {
            get
            {
                var currentValue = MyTargetPrivacyProxy.CurrentPrivacy;
                return new MyTargetPrivacy(currentValue.userConsent,
                                           currentValue.ccpaUserConsent,
                                           currentValue.iabUserConsent,
                                           currentValue.userAgeRestricted);
            }
        }

        public readonly Boolean? userConsent;
        public readonly Boolean? ccpaUserConsent;
        public readonly Boolean? iabUserConsent;
        public readonly Boolean userAgeRestricted;
        public readonly Boolean isConsent;

        private MyTargetPrivacy(Boolean? userConsent,
                                Boolean? ccpaUserConsent,
                                Boolean? iabUserConsent,
                                Boolean userAgeRestricted)
        {
            this.userConsent = userConsent;
            this.ccpaUserConsent = ccpaUserConsent;
            this.iabUserConsent = iabUserConsent;
            this.userAgeRestricted = userAgeRestricted;
            this.isConsent = userConsent != false && ccpaUserConsent != false && iabUserConsent != false;
        }
    }
}