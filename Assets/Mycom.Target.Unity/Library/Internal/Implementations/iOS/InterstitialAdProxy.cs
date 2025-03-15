#if UNITY_IOS
namespace Mycom.Target.Unity.Internal.Implementations.iOS
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Threading;
    using AOT;
    using Interfaces;

    internal sealed class InterstitialAdProxy : IInterstitialAdProxy
    {
        private static readonly Dictionary<UInt32, InterstitialAdProxy> Instanses = new();

        static InterstitialAdProxy()
        {
            MTRGInterstitialAdSetCallbackOnClick(OnAdClicked);
            MTRGInterstitialAdSetCallbackOnClose(OnAdDismissed);
            MTRGInterstitialAdSetCallbackOnDisplay(OnAdDisplayed);
            MTRGInterstitialAdSetCallbackOnLoad(OnAdLoadCompleted);
            MTRGInterstitialAdSetCallbackOnAdLoadFailed(OnAdLoadFailed);
            MTRGInterstitialAdSetCallbackOnVideoComplete(OnAdVideoCompleted);
        }

        [DllImport("__Internal")]
        private static extern void MTRGInterstitialAdClose(UInt32 adId);

        [DllImport("__Internal")]
        private static extern UInt32 MTRGInterstitialAdCreate(UInt32 slotId);

        [DllImport("__Internal")]
        private static extern void MTRGInterstitialAdDelete(UInt32 adId);

        [DllImport("__Internal")]
        private static extern void MTRGInterstitialAdLoad(UInt32 adId);

        [DllImport("__Internal")]
        private static extern void MTRGInterstitialAdSetCallbackOnClick(Action<UInt32> callback);

        [DllImport("__Internal")]
        private static extern void MTRGInterstitialAdSetCallbackOnClose(Action<UInt32> callback);

        [DllImport("__Internal")]
        private static extern void MTRGInterstitialAdSetCallbackOnDisplay(Action<UInt32> callback);

        [DllImport("__Internal")]
        private static extern void MTRGInterstitialAdSetCallbackOnLoad(Action<UInt32> callback);

        [DllImport("__Internal")]
        private static extern void MTRGInterstitialAdSetCallbackOnAdLoadFailed(Action<UInt32, Int64, String> callback);

        [DllImport("__Internal")]
        private static extern void MTRGInterstitialAdSetCallbackOnVideoComplete(Action<UInt32> callback);

        [DllImport("__Internal")]
        private static extern void MTRGInterstitialAdShow(UInt32 adId);

        [MonoPInvokeCallback(typeof(Action<UInt32>))]
        private static void OnAdClicked(UInt32 adId)
        {
            lock (Instanses)
            {
                if (Instanses.TryGetValue(adId, out InterstitialAdProxy interstitialAdProxy))
                {
                    interstitialAdProxy.AdClicked?.Invoke();
                }
            }
        }

        [MonoPInvokeCallback(typeof(Action<UInt32>))]
        private static void OnAdDismissed(UInt32 adId)
        {
            lock (Instanses)
            {
                if (Instanses.TryGetValue(adId, out InterstitialAdProxy interstitialAdProxy))
                {
                    interstitialAdProxy.AdDismissed?.Invoke();
                }
            }
        }

        [MonoPInvokeCallback(typeof(Action<UInt32>))]
        private static void OnAdDisplayed(UInt32 adId)
        {
            lock (Instanses)
            {
                if (Instanses.TryGetValue(adId, out InterstitialAdProxy interstitialAdProxy))
                {
                    interstitialAdProxy.AdDisplayed?.Invoke();
                }
            }
        }

        [MonoPInvokeCallback(typeof(Action<UInt32>))]
        private static void OnAdLoadCompleted(UInt32 adId)
        {
            lock (Instanses)
            {
                if (Instanses.TryGetValue(adId, out InterstitialAdProxy interstitialAdProxy))
                {
                    interstitialAdProxy.AdLoadCompleted?.Invoke();
                }
            }
        }

        [MonoPInvokeCallback(typeof(Action<UInt32, Int64, String>))]
        private static void OnAdLoadFailed(UInt32 adId, Int64 code, String description)
        {
            lock (Instanses)
            {
                if (Instanses.TryGetValue(adId, out InterstitialAdProxy interstitialAdProxy))
                {
                    interstitialAdProxy.AdLoadFailed?.Invoke(code, description);
                }
            }
        }

        [MonoPInvokeCallback(typeof(Action<UInt32>))]
        private static void OnAdVideoCompleted(UInt32 adId)
        {
            lock (Instanses)
            {
                if (Instanses.TryGetValue(adId, out InterstitialAdProxy interstitialAdProxy))
                {
                    interstitialAdProxy.AdVideoCompleted?.Invoke();
                }
            }
        }

        private readonly UInt32 _instanseId;
        private Int64 _isDisposed;

        public InterstitialAdProxy(UInt32 slotId)
        {
            _instanseId = MTRGInterstitialAdCreate(slotId);
            CustomParamsProxy = new CustomParamsProxy(_instanseId);

            lock (Instanses)
            {
                Instanses[_instanseId] = this;
            }
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

        public ICustomParamsProxy CustomParamsProxy { get; private set; }

        void IAdProxy.Load() => MTRGInterstitialAdLoad(_instanseId);
        void IInterstitialAdProxy.Dismiss() => MTRGInterstitialAdClose(_instanseId);
        void IInterstitialAdProxy.Show() => MTRGInterstitialAdShow(_instanseId);

        void IDisposable.Dispose()
        {
            if (Interlocked.CompareExchange(ref _isDisposed, 1, 0) != 0)
            {
                return;
            }

            lock (Instanses)
            {
                Instanses.Remove(_instanseId);
            }

            MTRGInterstitialAdDelete(_instanseId);

            GC.SuppressFinalize(this);
        }
    }
}

#endif