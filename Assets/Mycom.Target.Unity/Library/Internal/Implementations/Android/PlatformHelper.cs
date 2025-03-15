#if UNITY_ANDROID
namespace Mycom.Target.Unity.Internal.Implementations.Android
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    internal static class PlatformHelper
    {
        private static readonly HashSet<AndroidJavaRunnable> AndroidJavaRunnables = new HashSet<AndroidJavaRunnable>();

        private static readonly Lazy<Single> _density = new Lazy<Single>(() =>
            {
                var context = ApplicationContext;
                var resources = context.Call<AndroidJavaObject>("getResources");
                var displayMetrics = resources.Call<AndroidJavaObject>("getDisplayMetrics");
                return displayMetrics.Get<Single>("density");
            },
            System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);

        internal static AndroidJavaObject ApplicationContext => CurrentActivity.Call<AndroidJavaObject>("getApplicationContext");

        internal static AndroidJavaObject CurrentActivity => new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");

        internal static Single Density => _density.Value;

        internal static AndroidJavaObject CreateJavaString(String value) => value == null ? null : new AndroidJavaObject("java.lang.String", value);

        internal static AndroidJavaObject CreateJavaStringArray(String[] values)
        {
            if (values == null)
            {
                return null;
            }

            using var arrayClass = new AndroidJavaClass("java.lang.reflect.Array");

            using var stringClass = new AndroidJavaClass("java.lang.String");

            var arrayObject = arrayClass.CallStatic<AndroidJavaObject>("newInstance", stringClass, values.Length);
            for (var i = 0; i < values.Length; ++i)
            {
                using var stringValue = new AndroidJavaObject("java.lang.String", values[i]);

                arrayClass.CallStatic("set", arrayObject, i, stringValue);
            }

            return arrayObject;
        }

        internal static String[] CreateStringArray(AndroidJavaObject javaObject)
        {
            if (javaObject == null)
            {
                return null;
            }

            var length = javaObject.Get<Int32>("length");
            var result = new String[length];
            using (var arrayClass = new AndroidJavaClass("java.lang.reflect.Array"))
            {
                for (int i = 0; i < length; i++)
                {
                    result[i] = arrayClass.CallStatic<String>("get", javaObject);
                }
            }

            return result;
        }

        internal static Int32 GetInDp(Single value) => (Int32)(Density * value);

        internal static Int32 GetInDp(Double value) => (Int32)(Density * value);

        internal static void RunInUiThread(Action action)
        {
            AndroidJavaRunnable javaRunnable = null;
            javaRunnable = () =>
                           {
                               action();
                               AndroidJavaRunnables.Remove(javaRunnable);
                           };

            if (AndroidJavaRunnables.Add(javaRunnable))
            {
                CurrentActivity.Call("runOnUiThread", javaRunnable);
            }
        }
    }
}
#endif