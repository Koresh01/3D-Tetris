#if UNITY_IOS
namespace Mycom.Target.Unity.Internal.Implementations.iOS
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Threading;
    using AOT;
    using Ads;
    using Interfaces;

    internal sealed class MyTargetViewProxy : IMyTargetViewProxy
    {
        private static readonly Dictionary<UInt32, MyTargetViewProxy> Instanses = new();

        static MyTargetViewProxy()
        {
            MTRGStandardAdSetCallbackOnAdClicked(OnAdClicked);
            MTRGStandardAdSetCallbackOnAdLoadCompleted(OnAdLoadCompleted);
            MTRGStandardAdSetCallbackOnAdLoadFailed(OnAdLoadFailed);
            MTRGStandardAdSetCallbackOnAdShown(OnAdShown);
        }

        [DllImport("__Internal")]
        private static extern UInt32 MTRGStandardAdCreate(UInt32 slotId, Boolean isRefreshAd, UInt32 adSize);

        [DllImport("__Internal")]
        private static extern void MTRGStandardAdDelete(UInt32 adId);

        [DllImport("__Internal")]
        private static extern void MTRGStandardAdLoad(UInt32 adId);

        [DllImport("__Internal")]
        private static extern void MTRGStandardAdSetCallbackOnAdClicked(Action<UInt32> callback);

        [DllImport("__Internal")]
        private static extern void MTRGStandardAdSetCallbackOnAdLoadCompleted(Action<UInt32> callback);

        [DllImport("__Internal")]
        private static extern void MTRGStandardAdSetCallbackOnAdLoadFailed(Action<UInt32, Int64, String> callback);

        [DllImport("__Internal")]
        private static extern void MTRGStandardAdSetCallbackOnAdShown(Action<UInt32> callback);

        [DllImport("__Internal")]
        private static extern void MTRGStandardAdSetHeight(UInt32 adId, UInt32 height);

        [DllImport("__Internal")]
        private static extern void MTRGStandardAdSetWidth(UInt32 adId, UInt32 width);

        [DllImport("__Internal")]
        private static extern void MTRGStandardAdSetX(UInt32 adId, UInt32 x);

        [DllImport("__Internal")]
        private static extern void MTRGStandardAdSetY(UInt32 adId, UInt32 y);

        [DllImport("__Internal")]
        private static extern void MTRGStandardAdStart(UInt32 adId);

        [DllImport("__Internal")]
        private static extern void MTRGStandardAdStop(UInt32 adId);

        [MonoPInvokeCallback(typeof(Action<UInt32>))]
        private static void OnAdClicked(UInt32 adId)
        {
            lock (Instanses)
            {
                if (Instanses.TryGetValue(adId, out MyTargetViewProxy myTargetViewProxy))
                {
                    myTargetViewProxy.AdClicked?.Invoke();
                }
            }
        }

        [MonoPInvokeCallback(typeof(Action<UInt32>))]
        private static void OnAdLoadCompleted(UInt32 adId)
        {
            lock (Instanses)
            {
                if (Instanses.TryGetValue(adId, out MyTargetViewProxy myTargetViewProxy))
                {
                    myTargetViewProxy.AdLoadCompleted?.Invoke();
                }
            }
        }

        [MonoPInvokeCallback(typeof(Action<UInt32, Int64, String>))]
        private static void OnAdLoadFailed(UInt32 adId, Int64 code, String description)
        {
            lock (Instanses)
            {
                if (Instanses.TryGetValue(adId, out MyTargetViewProxy myTargetViewProxy))
                {
                    myTargetViewProxy.AdLoadFailed?.Invoke(code, description);
                }
            }
        }

        [MonoPInvokeCallback(typeof(Action<UInt32>))]
        private static void OnAdShown(UInt32 adId)
        {
            lock (Instanses)
            {
                if (Instanses.TryGetValue(adId, out MyTargetViewProxy myTargetViewProxy))
                {
                    myTargetViewProxy.AdShown?.Invoke();
                }
            }
        }

        private readonly UInt32 _instanseId;
        private Int64 _isDisposed;

        public MyTargetViewProxy(UInt32 slotId,
                                 MyTargetView.AdSize adSize,
                                 Boolean isRefreshAd)
        {
            _instanseId = MTRGStandardAdCreate(slotId, isRefreshAd, (UInt32)adSize);
            CustomParamsProxy = new CustomParamsProxy(_instanseId);

            lock (Instanses)
            {
                Instanses[_instanseId] = this;
            }
        }

        ~MyTargetViewProxy()
        {
            ((IDisposable)this).Dispose();
        }

        public event Action AdClicked;
        public event Action AdLoadCompleted;
        public event Action<Int64, String> AdLoadFailed;
        public event Action AdShown;

        public ICustomParamsProxy CustomParamsProxy { get; private set; }

        void IAdProxy.Load() => MTRGStandardAdLoad(_instanseId);
        void IMyTargetViewProxy.SetHeight(Double value) => MTRGStandardAdSetHeight(_instanseId, (UInt32)value);
        void IMyTargetViewProxy.SetWidth(Double value) => MTRGStandardAdSetWidth(_instanseId, (UInt32)value);
        void IMyTargetViewProxy.SetX(Double value) => MTRGStandardAdSetX(_instanseId, (UInt32)value);
        void IMyTargetViewProxy.SetY(Double value) => MTRGStandardAdSetY(_instanseId, (UInt32)value);
        void IMyTargetViewProxy.Start() => MTRGStandardAdStart(_instanseId);
        void IMyTargetViewProxy.Stop() => MTRGStandardAdStop(_instanseId);

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

            MTRGStandardAdDelete(_instanseId);

            GC.SuppressFinalize(this);
        }
    }
}

#endif