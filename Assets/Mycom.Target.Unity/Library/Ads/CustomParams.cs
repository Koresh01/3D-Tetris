using System;
using System.Collections.Generic;
using System.Linq;
using Mycom.Target.Unity.Internal.Interfaces;

namespace Mycom.Target.Unity.Ads
{
    public sealed class CustomParams : IDisposable
    {
        private const String FrameworkSDKKey = "framework";
        private const String FrameworkSDKValue = "1";

        private readonly IDictionary<String, String> _customParams = new Dictionary<String, String>();

        private UInt32? _age;
        private ICustomParamsProxy _customParamsProxy;
        private String[] _emails;
        private GenderEnum _gender = GenderEnum.Unspecified;
        private UInt32[] _icqIds;
        private String _lang;
        private String _mrgsAppId;
        private String _mrgsId;
        private String _mrgsUserId;
        private String[] _okIds;
        private String[] _vkIds;
        private String[] _customUserIds;

        public CustomParams() => SetCustomParam(FrameworkSDKKey, FrameworkSDKValue);

        public UInt32? Age
        {
            get => _age;
            set
            {
                _age = value;

                _customParamsProxy?.SetAge(value);
            }
        }

        public String Email
        {
            get => _emails?.FirstOrDefault();
            set
            {
                _emails = value == null ? null : new[] { value };

                _customParamsProxy?.SetEmails(_emails);
            }
        }

        public String[] Emails
        {
            get => _emails;
            set
            {
                _emails = value;

                _customParamsProxy?.SetEmails(_emails);
            }
        }

        public GenderEnum Gender
        {
            get => _gender;
            set
            {
                _gender = value;

                _customParamsProxy?.SetGender(value);
            }
        }

        public UInt32? IcqId
        {
            get => _icqIds == null ? default(UInt32?) : _icqIds.FirstOrDefault();
            set
            {
                _icqIds = value == null ? null : new[] { value.Value };

                _customParamsProxy?.SetIcqIds(_icqIds);
            }
        }

        public UInt32[] IcqIds
        {
            get => _icqIds;
            set
            {
                _icqIds = value;

                _customParamsProxy?.SetIcqIds(_icqIds);
            }
        }

        public String Lang
        {
            get => _lang;
            set
            {
                _lang = value;

                _customParamsProxy?.SetLang(_lang);
            }
        }

        public String MrgsAppId
        {
            get => _mrgsAppId;
            set
            {
                _mrgsAppId = value;

                _customParamsProxy?.SetMrgsAppId(_mrgsAppId);
            }
        }

        public String MrgsId
        {
            get => _mrgsId;
            set
            {
                _mrgsId = value;

                _customParamsProxy?.SetMrgsId(_mrgsId);
            }
        }

        public String MrgsUserId
        {
            get => _mrgsUserId;
            set
            {
                _mrgsUserId = value;

                _customParamsProxy?.SetMrgsUserId(_mrgsUserId);
            }
        }

        public String OkId
        {
            get => _okIds?.FirstOrDefault();
            set
            {
                _okIds = value == null ? null : new[] { value };

                _customParamsProxy?.SetOkIds(_okIds);
            }
        }

        public String[] OkIds
        {
            get => _okIds;
            set
            {
                _okIds = value;

                _customParamsProxy?.SetOkIds(_okIds);
            }
        }

        public String VkId
        {
            get => _vkIds?.FirstOrDefault();
            set
            {
                _vkIds = value == null ? null : new[] { value };

                _customParamsProxy?.SetVkIds(_vkIds);
            }
        }

        public String[] VkIds
        {
            get => _vkIds;
            set
            {
                _vkIds = value;

                _customParamsProxy?.SetVkIds(_vkIds);
            }
        }

        public String CustomUserId
        {
            get => _customUserIds?.FirstOrDefault();
            set
            {
                _customUserIds = value == null ? null : new[] { value };

                _customParamsProxy?.SetCustomUserIds(_customUserIds);
            }
        }

        public String[] CustomUserIds
        {
            get => _customUserIds;
            set
            {
                _customUserIds = value;

                _customParamsProxy?.SetCustomUserIds(_customUserIds);
            }
        }

        public void SetCustomParam(String key, String value)
        {
            if (String.IsNullOrEmpty(key))
            {
                return;
            }

            _customParams[key] = value;

            _customParamsProxy?.SetCustomParam(key, value);
        }

        public String GetCustomParam(String key)
        {
            if (String.IsNullOrEmpty(key))
            {
                return null;
            }

            return _customParams[key];
        }

        internal void SetCustomParamsProxy(ICustomParamsProxy proxy)
        {
            _customParamsProxy?.Dispose();

            _customParamsProxy = proxy;
            if (_customParamsProxy == null)
            {
                return;
            }

            _customParamsProxy.SetAge(_age);
            _customParamsProxy.SetEmails(_emails);
            _customParamsProxy.SetGender(_gender);
            _customParamsProxy.SetIcqIds(_icqIds);
            _customParamsProxy.SetLang(_lang);
            _customParamsProxy.SetMrgsAppId(_mrgsAppId);
            _customParamsProxy.SetMrgsId(_mrgsId);
            _customParamsProxy.SetMrgsUserId(_mrgsUserId);
            _customParamsProxy.SetOkIds(_okIds);
            _customParamsProxy.SetVkIds(_vkIds);
            _customParamsProxy.SetCustomUserIds(_customUserIds);

            foreach (var item in _customParams)
            {
                _customParamsProxy.SetCustomParam(item.Key, item.Value);
            }
        }

        public void Dispose() => _customParamsProxy?.Dispose();
    }
}