#if UNITY_ANDROID
namespace Mycom.Target.Unity.Internal.Implementations.Android
{
    using System;
    using System.Threading;
    using Ads;
    using Interfaces;
    using UnityEngine;

    internal sealed class MyTargetViewProxy : AndroidJavaProxy, IMyTargetViewProxy
    {
        const String MethodNameRequestLayout = "requestLayout";

        readonly AndroidJavaObject _layoutParams;
        readonly AndroidJavaObject _myTargetViewObject;

        Int64 _isDisposed;
        Boolean _isShown;

        public MyTargetViewProxy(UInt32 slotId, MyTargetView.AdSize adSize, Boolean isRefreshAd) : base("com.my.target.ads.MyTargetView$MyTargetViewListener")
        {
            var currentActivity = PlatformHelper.CurrentActivity;

            _myTargetViewObject = new AndroidJavaObject("com.my.target.ads.MyTargetView", currentActivity);

            _layoutParams = new AndroidJavaObject("android.widget.FrameLayout$LayoutParams", 0, 0);

            _myTargetViewObject.Call("setListener", this);
            _myTargetViewObject.Call("init", (Int32)slotId, (Int32)adSize, isRefreshAd);
            _myTargetViewObject.Call("setLayoutParams", _layoutParams);

            CustomParamsProxy = new CustomParamsProxy(_myTargetViewObject.Call<AndroidJavaObject>("getCustomParams"));
        }

        ~MyTargetViewProxy()
        {
            ((IDisposable)this).Dispose();
        }

        public event Action AdClicked;
        public event Action AdLoadCompleted;
        public event Action<Int64, String> AdLoadFailed;
        public event Action AdShown;

        void IAdProxy.Load()
        {
            if (Interlocked.Read(ref _isDisposed) == 1)
            {
                return;
            }
            _myTargetViewObject.Call("load");
        }

        public ICustomParamsProxy CustomParamsProxy { get; private set; }

        void IDisposable.Dispose()
        {
            if (Interlocked.CompareExchange(ref _isDisposed, 1, 0) != 0)
            {
                return;
            }

            PlatformHelper.RunInUiThread(() =>
                                         {
                                             if (_myTargetViewObject != null)
                                             {
                                                 _myTargetViewObject.Call<AndroidJavaObject>("getParent")
                                                                    .Call("removeView", _myTargetViewObject);

                                                 _myTargetViewObject.Call("destroy");

                                                 _myTargetViewObject.Dispose();
                                             }

                                             CustomParamsProxy?.Dispose();
                                         });

            GC.SuppressFinalize(this);
        }

        void IMyTargetViewProxy.SetHeight(Double value)
        {
            if (Interlocked.Read(ref _isDisposed) == 1)
            {
                return;
            }

            PlatformHelper.RunInUiThread(() =>
                                         {
                                             _layoutParams.Set("height", PlatformHelper.GetInDp(value));
                                             _myTargetViewObject.Call(MethodNameRequestLayout);
                                         });
        }

        void IMyTargetViewProxy.SetWidth(Double value)
        {
            if (Interlocked.Read(ref _isDisposed) == 1)
            {
                return;
            }

            PlatformHelper.RunInUiThread(() =>
                                         {
                                             _layoutParams.Set("width", PlatformHelper.GetInDp(value));
                                             _myTargetViewObject.Call(MethodNameRequestLayout);
                                         });
        }

        void IMyTargetViewProxy.SetX(Double value)
        {
            if (Interlocked.Read(ref _isDisposed) == 1)
            {
                return;
            }

            PlatformHelper.RunInUiThread(() =>
                                         {
                                             _layoutParams.Set("leftMargin", PlatformHelper.GetInDp(value));
                                             _myTargetViewObject.Call(MethodNameRequestLayout);
                                         });
        }

        void IMyTargetViewProxy.SetY(Double value)
        {
            if (Interlocked.Read(ref _isDisposed) == 1)
            {
                return;
            }

            PlatformHelper.RunInUiThread(() =>
                                         {
                                             _layoutParams.Set("topMargin", PlatformHelper.GetInDp(value));
                                             _myTargetViewObject.Call(MethodNameRequestLayout);
                                         });
        }

        void IMyTargetViewProxy.Start()
        {
            if (Interlocked.Read(ref _isDisposed) == 1)
            {
                return;
            }

            if (_isShown)
            {
                return;
            }

            _isShown = true;

            PlatformHelper.RunInUiThread(() =>
                                         {
                                             var activity = PlatformHelper.CurrentActivity;

                                             var r = new AndroidJavaClass("android.R$id");

                                             var contentId = r.GetStatic<Int32>("content");

                                             activity.Call<AndroidJavaObject>("findViewById", contentId)
                                                     .Call("addView", _myTargetViewObject);
                                         });
        }

        void IMyTargetViewProxy.Stop()
        {
            if (Interlocked.Read(ref _isDisposed) == 1)
            {
                return;
            }

            if (Interlocked.Read(ref _isDisposed) == 1)
            {
                return;
            }

            _isShown = false;

            PlatformHelper.RunInUiThread(() =>
                                         {
                                             _myTargetViewObject.Call<AndroidJavaObject>("getParent")
                                                                .Call("removeView", _myTargetViewObject);
                                         });
        }

        public void onClick(AndroidJavaObject o) => AdClicked?.Invoke();

        public void onLoad(AndroidJavaObject o) => AdLoadCompleted?.Invoke();

        public void onNoAd(AndroidJavaObject errorObject, AndroidJavaObject o) => AdLoadFailed?.Invoke(errorObject.Call<int>("getCode"), errorObject.Call<String>("getMessage"));
        
        public void onShow(AndroidJavaObject o) => AdShown?.Invoke();
    }
}

#endif