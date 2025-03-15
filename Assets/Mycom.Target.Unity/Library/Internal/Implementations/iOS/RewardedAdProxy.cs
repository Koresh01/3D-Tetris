#if UNITY_IOS
namespace Mycom.Target.Unity.Internal.Implementations.iOS
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Threading;
    using AOT;
    using Interfaces;

    internal sealed class RewardedAdProxy : IRewardedAdProxy
    {
        private static readonly Dictionary<UInt32, RewardedAdProxy> Instanses = new();

        static RewardedAdProxy()
        {
            MTRGRewardedAdSetCallbackOnClick(OnAdClicked);
            MTRGRewardedAdSetCallbackOnClose(OnAdDismissed);
            MTRGRewardedAdSetCallbackOnDisplay(OnAdDisplayed);
            MTRGRewardedAdSetCallbackOnLoad(OnAdLoadCompleted);
            MTRGRewardedAdSetCallbackOnAdLoadFailed(OnAdLoadFailed);
            MTRGRewardedAdSetCallbackOnReward(OnAdReward);
        }

        [DllImport("__Internal")]
        private static extern void MTRGRewardedAdClose(UInt32 adId);

        [DllImport("__Internal")]
        private static extern UInt32 MTRGRewardedAdCreate(UInt32 slotId);

        [DllImport("__Internal")]
        private static extern void MTRGRewardedAdDelete(UInt32 adId);

        [DllImport("__Internal")]
        private static extern void MTRGRewardedAdLoad(UInt32 adId);

        [DllImport("__Internal")]
        private static extern void MTRGRewardedAdSetCallbackOnClick(Action<UInt32> callback);

        [DllImport("__Internal")]
        private static extern void MTRGRewardedAdSetCallbackOnClose(Action<UInt32> callback);

        [DllImport("__Internal")]
        private static extern void MTRGRewardedAdSetCallbackOnDisplay(Action<UInt32> callback);

        [DllImport("__Internal")]
        private static extern void MTRGRewardedAdSetCallbackOnLoad(Action<UInt32> callback);

        [DllImport("__Internal")]
        private static extern void MTRGRewardedAdSetCallbackOnAdLoadFailed(Action<UInt32, Int64, String> callback);

        [DllImport("__Internal")]
        private static extern void MTRGRewardedAdSetCallbackOnReward(Action<UInt32, String> callback);

        [DllImport("__Internal")]
        private static extern void MTRGRewardedAdShow(UInt32 adId);

        [MonoPInvokeCallback(typeof(Action<UInt32>))]
        private static void OnAdClicked(UInt32 adId)
        {
            lock (Instanses)
            {
                if (Instanses.TryGetValue(adId, out RewardedAdProxy interstitialAdProxy))
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
                if (Instanses.TryGetValue(adId, out RewardedAdProxy interstitialAdProxy))
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
                if (Instanses.TryGetValue(adId, out RewardedAdProxy interstitialAdProxy))
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
                if (Instanses.TryGetValue(adId, out RewardedAdProxy interstitialAdProxy))
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
                if (Instanses.TryGetValue(adId, out RewardedAdProxy interstitialAdProxy))
                {
                    interstitialAdProxy.AdLoadFailed?.Invoke(code, description);
                }
            }
        }

        [MonoPInvokeCallback(typeof(Action<UInt32>))]
        private static void OnAdReward(UInt32 adId, String type)
        {
            lock (Instanses)
            {
                if (Instanses.TryGetValue(adId, out RewardedAdProxy interstitialAdProxy))
                {
                    interstitialAdProxy.AdRewarded?.Invoke(type);
                }
            }
        }

        private readonly UInt32 _instanseId;
        private Int64 _isDisposed;

        public RewardedAdProxy(UInt32 slotId)
        {
            _instanseId = MTRGRewardedAdCreate(slotId);
            CustomParamsProxy = new CustomParamsProxy(_instanseId);

            lock (Instanses)
            {
                Instanses[_instanseId] = this;
            }
        }

        ~RewardedAdProxy()
        {
            ((IDisposable)this).Dispose();
        }

        public event Action AdClicked;
        public event Action AdLoadCompleted;
        public event Action<Int64, String> AdLoadFailed;
        public event Action AdDismissed;
        public event Action AdDisplayed;
        public event Action<String> AdRewarded;

        public ICustomParamsProxy CustomParamsProxy { get; private set; }

        void IAdProxy.Load() => MTRGRewardedAdLoad(_instanseId);
        void IRewardedAdProxy.Dismiss() => MTRGRewardedAdClose(_instanseId);
        void IRewardedAdProxy.Show() => MTRGRewardedAdShow(_instanseId);

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

            MTRGRewardedAdDelete(_instanseId);

            GC.SuppressFinalize(this);
        }
    }
}

#endif