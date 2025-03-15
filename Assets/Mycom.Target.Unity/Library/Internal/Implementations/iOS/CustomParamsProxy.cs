#if UNITY_IOS
namespace Mycom.Target.Unity.Internal.Implementations.iOS
{
    using System;
    using System.Runtime.InteropServices;
    using System.Linq;
    using Ads;
    using Interfaces;

    internal class CustomParamsProxy : ICustomParamsProxy
    {
        [DllImport("__Internal")]
        private static extern void MTRGCustomParamsSetAge(UInt32 adId, UInt32 value);

        [DllImport("__Internal")]
        private static extern void MTRGCustomParamsResetAge(UInt32 adId);

        [DllImport("__Internal")]
        private static extern void MTRGCustomParamsSetEmail(UInt32 adId, String value);

        [DllImport("__Internal")]
        private static extern void MTRGCustomParamsSetGender(UInt32 adId, Int32 value);

        [DllImport("__Internal")]
        private static extern void MTRGCustomParamsSetLanguage(UInt32 adId, String value);

        [DllImport("__Internal")]
        private static extern void MTRGCustomParamsSetIcqIds(UInt32 adId, String value);

        [DllImport("__Internal")]
        private static extern void MTRGCustomParamsSetMrgsAppId(UInt32 adId, String value);

        [DllImport("__Internal")]
        private static extern void MTRGCustomParamsSetMrgsId(UInt32 adId, String value);

        [DllImport("__Internal")]
        private static extern void MTRGCustomParamsSetMrgsUserId(UInt32 adId, String value);

        [DllImport("__Internal")]
        private static extern void MTRGCustomParamsSetOkIds(UInt32 adId, String value);

        [DllImport("__Internal")]
        private static extern void MTRGCustomParamsSetVkIds(UInt32 adId, String value);

        [DllImport("__Internal")]
        private static extern void MTRGCustomParamsSetCustomUserIds(UInt32 adId, String value);

        [DllImport("__Internal")]
        private static extern void MTRGCustomParamsSetCustomParam(UInt32 adId, String key, String value);

        private readonly UInt32 _adId;

        public CustomParamsProxy(UInt32 adId)
        {
            _adId = adId;

            MTRGCustomParamsSetCustomParam(_adId, "framework", "1");
        }

        void ICustomParamsProxy.SetAge(UInt32? value)
        {
            if (value != null)
            {
                MTRGCustomParamsSetAge(_adId, value.Value);
            }
            else
            {
                MTRGCustomParamsResetAge(_adId);
            }
        }

        void ICustomParamsProxy.SetEmails(String[] value) => MTRGCustomParamsSetEmail(_adId, value == null ? null : String.Join(",", value));

        void ICustomParamsProxy.SetGender(GenderEnum value) => MTRGCustomParamsSetGender(_adId, (Int32)value);

        void ICustomParamsProxy.SetLang(String value) => MTRGCustomParamsSetLanguage(_adId, value);

        void ICustomParamsProxy.SetIcqIds(UInt32[] value) => MTRGCustomParamsSetIcqIds(_adId, value == null ? null : String.Join(",", value.Select(v => Convert.ToString(v)).ToArray()));

        void ICustomParamsProxy.SetMrgsAppId(String value) => MTRGCustomParamsSetMrgsAppId(_adId, value);

        void ICustomParamsProxy.SetMrgsId(String value) => MTRGCustomParamsSetMrgsId(_adId, value);

        void ICustomParamsProxy.SetMrgsUserId(String value) => MTRGCustomParamsSetMrgsUserId(_adId, value);

        void ICustomParamsProxy.SetOkIds(String[] value) => MTRGCustomParamsSetOkIds(_adId, value == null ? null : String.Join(",", value));

        void ICustomParamsProxy.SetVkIds(String[] value) => MTRGCustomParamsSetVkIds(_adId, value == null ? null : String.Join(",", value));

        void ICustomParamsProxy.SetCustomUserIds(string[] value) => MTRGCustomParamsSetCustomUserIds(_adId, value == null ? null : String.Join(",", value));

        void ICustomParamsProxy.SetCustomParam(String key, String value) => MTRGCustomParamsSetCustomParam(_adId, key, value);

        void IDisposable.Dispose() { }
    }
}

#endif