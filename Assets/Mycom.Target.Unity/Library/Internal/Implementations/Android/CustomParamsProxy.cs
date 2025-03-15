#if UNITY_ANDROID
namespace Mycom.Target.Unity.Internal.Implementations.Android
{
    using System;
    using System.Threading;
    using Ads;
    using Interfaces;
    using UnityEngine;

    internal class CustomParamsProxy : ICustomParamsProxy
    {
        private readonly AndroidJavaObject _javaCustomParams;
        private Int64 _isDisposed;

        public CustomParamsProxy(AndroidJavaObject javaCustomParams)
        {
            _javaCustomParams = javaCustomParams;

            ((ICustomParamsProxy)this).SetCustomParam("framework", "1");
        }

        ~CustomParamsProxy()
        {
            ((IDisposable)this).Dispose();
        }

        void ICustomParamsProxy.SetAge(UInt32? value)
        {
            if (Interlocked.Read(ref _isDisposed) == 1)
            {
                return;
            }

            _javaCustomParams.Call("setAge", value == null ? -1 : (Int32)value);
        }

        void ICustomParamsProxy.SetGender(GenderEnum value)
        {
            if (Interlocked.Read(ref _isDisposed) == 1)
            {
                return;
            }
            _javaCustomParams.Call("setGender", (Int32)value);
        }

        void ICustomParamsProxy.SetEmails(String[] value)
        {
            if (Interlocked.Read(ref _isDisposed) == 1)
            {
                return;
            }

            using var javaArray = PlatformHelper.CreateJavaStringArray(value);

            _javaCustomParams.Call("setEmails", javaArray);
        }

        void ICustomParamsProxy.SetLang(String value)
        {
            if (Interlocked.Read(ref _isDisposed) == 1)
            {
                return;
            }

            using var javaValue = PlatformHelper.CreateJavaString(value);

            _javaCustomParams.Call("setLang", javaValue);
        }

        void ICustomParamsProxy.SetIcqIds(UInt32[] value)
        {
            if (Interlocked.Read(ref _isDisposed) == 1)
            {
                return;
            }

            const String methodName = "setIcqIds";
            if (value == null)
            {
                _javaCustomParams.Call(methodName, null);
            }
            else
            {
                String[] array = new String[value.Length];
                for (int i = 0; i < value.Length; i++)
                {
                    array[i] = Convert.ToString(value[i]);
                }

                using var javaArray = PlatformHelper.CreateJavaStringArray(array);

                _javaCustomParams.Call(methodName, javaArray);
            }
        }

        void ICustomParamsProxy.SetMrgsAppId(String value)
        {
            if (Interlocked.Read(ref _isDisposed) == 1)
            {
                return;
            }

            using var javaValue = PlatformHelper.CreateJavaString(value);

            _javaCustomParams.Call("setMrgsAppId", javaValue);
        }

        void ICustomParamsProxy.SetMrgsId(String value)
        {
            if (Interlocked.Read(ref _isDisposed) == 1)
            {
                return;
            }

            using var javaValue = PlatformHelper.CreateJavaString(value);

            _javaCustomParams.Call("setMrgsId", javaValue);
        }

        void ICustomParamsProxy.SetMrgsUserId(String value)
        {
            if (Interlocked.Read(ref _isDisposed) == 1)
            {
                return;
            }

            using var javaValue = PlatformHelper.CreateJavaString(value);

            _javaCustomParams.Call("setMrgsUserId", javaValue);
        }

        void ICustomParamsProxy.SetOkIds(String[] value)
        {
            if (Interlocked.Read(ref _isDisposed) == 1)
            {
                return;
            }

            using var javaArray = PlatformHelper.CreateJavaStringArray(value);

            _javaCustomParams.Call("setOkIds", javaArray);
        }

        void ICustomParamsProxy.SetVkIds(String[] value)
        {
            if (Interlocked.Read(ref _isDisposed) == 1)
            {
                return;
            }

            using var javaArray = PlatformHelper.CreateJavaStringArray(value);

            _javaCustomParams.Call("setVKIds", javaArray);
        }

        void ICustomParamsProxy.SetCustomUserIds(string[] value)
        {
            if (Interlocked.Read(ref _isDisposed) == 1)
            {
                return;
            }

            using var javaArray = PlatformHelper.CreateJavaStringArray(value);

            _javaCustomParams.Call("setCustomUserIds", javaArray);
        }

        void ICustomParamsProxy.SetCustomParam(String key, String value)
        {
            if (Interlocked.Read(ref _isDisposed) == 1)
            {
                return;
            }

            using var javaKey = PlatformHelper.CreateJavaString(key);

            using var javaValue = PlatformHelper.CreateJavaString(value);

            _javaCustomParams.Call("setCustomParam", javaKey, javaValue);
        }

        void IDisposable.Dispose()
        {
            if (Interlocked.CompareExchange(ref _isDisposed, 1, 0) != 0)
            {
                return;
            }

            _javaCustomParams.Dispose();
        }
    }
}

#endif