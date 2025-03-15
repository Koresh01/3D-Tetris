#if UNITY_ANDROID
namespace Mycom.Target.Unity.Internal.Implementations.Android
{
    using System;
    using System.Threading;
    using Interfaces;
    using UnityEngine;

    internal sealed class InterstitialAdProxy : AndroidJavaProxy, IInterstitialAdProxy
    {
        private readonly AndroidJavaObject _interstitialAdObject;
        
        private Int64 _isDisposed;

        public InterstitialAdProxy(UInt32 slotId) : base("com.my.target.ads.InterstitialAd$InterstitialAdListener")
        {
            var currentActivity = PlatformHelper.CurrentActivity;

            _interstitialAdObject = new AndroidJavaObject("com.my.target.ads.InterstitialAd",
                                                          (Int32)slotId,
                                                          currentActivity);

            _interstitialAdObject.Call("setListener", this);

            CustomParamsProxy = new CustomParamsProxy(_interstitialAdObject.Call<AndroidJavaObject>("getCustomParams"));
        }

        ~InterstitialAdProxy()
        {
            ((IDisposable)this).Dispose();
        }

        public event Action AdClicked;
        public event Action AdLoadCompleted;
        public event Action<Int64, String> AdLoadFailed;
        public event Action AdDismissed;
        public event Action AdDisplayed;
        public event Action AdVideoCompleted;
        public event Action AdFailedToShow;

        void IAdProxy.Load()
        {
            if (Interlocked.Read(ref _isDisposed) == 1)
            {
                return;
            }
            _interstitialAdObject.Call("load");
        }

        public ICustomParamsProxy CustomParamsProxy { get; private set; }

        void IDisposable.Dispose()
        {
            if (Interlocked.CompareExchange(ref _isDisposed, 1, 0) != 0)
            {
                return;
            }

            if (_interstitialAdObject != null)
            {
                _interstitialAdObject.Call("destroy");

                _interstitialAdObject.Dispose();
            }

            CustomParamsProxy?.Dispose();

            GC.SuppressFinalize(this);
        }

        void IInterstitialAdProxy.Dismiss()
        {
            if (Interlocked.Read(ref _isDisposed) == 1)
            {
                return;
            }

            PlatformHelper.RunInUiThread(() => _interstitialAdObject.Call("dismiss"));
        }

        void IInterstitialAdProxy.Show()
        {
            if (Interlocked.Read(ref _isDisposed) == 1)
            {
                return;
            }

            PlatformHelper.RunInUiThread(() => _interstitialAdObject.Call("show"));
        }

        public void onLoad(AndroidJavaObject o) => AdLoadCompleted?.Invoke();
        
        public void onNoAd(AndroidJavaObject errorObject, AndroidJavaObject o) => AdLoadFailed?.Invoke(errorObject.Call<int>("getCode"), errorObject.Call<String>("getMessage"));
        
        public void onClick(AndroidJavaObject o) => AdClicked?.Invoke();
        
        public void onDismiss(AndroidJavaObject o) => AdDismissed?.Invoke();
        
        public void onVideoCompleted(AndroidJavaObject o) => AdVideoCompleted?.Invoke();
        
        public void onDisplay(AndroidJavaObject o) => AdDisplayed?.Invoke();

        public void onFailedToShow(AndroidJavaObject o) => AdFailedToShow?.Invoke();
    }
}
#endif