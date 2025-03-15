using System;
using Mycom.Target.Unity.Internal;
using Mycom.Target.Unity.Internal.Interfaces;

namespace Mycom.Target.Unity.Ads
{
    public sealed class InterstitialAd : AbstractAd
    {
        private readonly Object _syncRoot = new Object();
        private readonly UInt32 _slotId;

        private readonly Action _onAdDismissed;
        private readonly Action _onAdDisplayed;
        private readonly Action _onAdVideoCompleted;
        private readonly Action _onAdFailedToShow;

        private volatile IInterstitialAdProxy _interstitialAdProxy;

        public InterstitialAd(UInt32 slotId)
        {
            MyTargetLogger.Log("MyTarget.Unity(" + SDKVersion.Version + "): InterstitialAd is created");

            _onAdDismissed = () => _unityDispatcher.Perform(() => AdDismissed?.Invoke(this, EventArgs.Empty));
            _onAdDisplayed = () => _unityDispatcher.Perform(() => AdDisplayed?.Invoke(this, EventArgs.Empty));
            _onAdVideoCompleted = () => _unityDispatcher.Perform(() => AdVideoCompleted?.Invoke(this, EventArgs.Empty));
            _onAdFailedToShow = () => _unityDispatcher.Perform(() => AdFailedToShow?.Invoke(this, EventArgs.Empty));

            _slotId = slotId;
        }

        public void Dismiss() => _platformDispatcher.Perform(() =>
        {
            if (_interstitialAdProxy == null)
            {
                return;
            }

            lock (_syncRoot)
            {
                _interstitialAdProxy?.Dismiss();
            }
        });

        public override void Load() => _platformDispatcher.Perform(() =>
        {
            if (_interstitialAdProxy != null)
            {
                return;
            }

            lock (_syncRoot)
            {
                if (_interstitialAdProxy != null)
                {
                    return;
                }

                _interstitialAdProxy = PlatformFactory.CreateInterstitial(_slotId);

                _customParams.SetCustomParamsProxy(_interstitialAdProxy.CustomParamsProxy);

                _interstitialAdProxy.AdClicked += _onAdClicked;
                _interstitialAdProxy.AdDismissed += _onAdDismissed;
                _interstitialAdProxy.AdDisplayed += _onAdDisplayed;
                _interstitialAdProxy.AdLoadCompleted += _onAdLoadCompleted;
                _interstitialAdProxy.AdLoadFailed += _onAdLoadFailed;
                _interstitialAdProxy.AdVideoCompleted += _onAdVideoCompleted;
                _interstitialAdProxy.AdFailedToShow += _onAdFailedToShow;

                _interstitialAdProxy.Load();
            }
        });

        public void Show() => _platformDispatcher.Perform(() =>
        {
            if (_interstitialAdProxy == null)
            {
                return;
            }

            lock (_syncRoot)
            {
                _interstitialAdProxy?.Show();
            }
        });

        protected override void DisposeImpl()
        {
            if (_interstitialAdProxy == null)
            {
                return;
            }

            lock (_syncRoot)
            {
                if (_interstitialAdProxy == null)
                {
                    return;
                }

                var referenceCopy = _interstitialAdProxy;

                _interstitialAdProxy = null;

                referenceCopy.AdClicked -= _onAdClicked;
                referenceCopy.AdDismissed -= _onAdDismissed;
                referenceCopy.AdDisplayed -= _onAdDisplayed;
                referenceCopy.AdLoadCompleted -= _onAdLoadCompleted;
                referenceCopy.AdLoadFailed -= _onAdLoadFailed;
                referenceCopy.AdVideoCompleted -= _onAdVideoCompleted;

                _platformDispatcher.Perform(referenceCopy.Dispose);
            }
        }

        public event EventHandler AdDismissed;
        public event EventHandler AdDisplayed;
        public event EventHandler AdVideoCompleted;
        public event EventHandler AdFailedToShow;
    }
}