using System;
using Mycom.Target.Unity.Internal;
using Mycom.Target.Unity.Internal.Interfaces;

namespace Mycom.Target.Unity.Ads
{
    public sealed class RewardedAd : AbstractAd
    {
        private readonly Object _syncRoot = new Object();
        private readonly UInt32 _slotId;

        private readonly Action _onAdDismissed;
        private readonly Action _onAdDisplayed;
        private readonly Action<String> _onAdRewarded;
        private readonly Action _onAdFailedToShow;

        private volatile IRewardedAdProxy _rewardedAdProxy;

        public RewardedAd(UInt32 slotId)
        {
            MyTargetLogger.Log("MyTarget.Unity(" + SDKVersion.Version + "): RewardedAd is created");

            _onAdDismissed = () => _unityDispatcher.Perform(() => AdDismissed?.Invoke(this, EventArgs.Empty));
            _onAdDisplayed = () => _unityDispatcher.Perform(() => AdDisplayed?.Invoke(this, EventArgs.Empty));
            _onAdRewarded = (type) => _unityDispatcher.Perform(() => AdRewarded?.Invoke(this, new RewardEventArgs(type)));
            _onAdFailedToShow = () => _unityDispatcher.Perform(() => AdFailedToShow?.Invoke(this, EventArgs.Empty));

            _slotId = slotId;
        }

        public void Dismiss() => _platformDispatcher.Perform(() =>
        {
            if (_rewardedAdProxy == null)
            {
                return;
            }

            lock (_syncRoot)
            {
                _rewardedAdProxy?.Dismiss();
            }
        });

        public override void Load() => _platformDispatcher.Perform(() =>
        {
            if (_rewardedAdProxy != null)
            {
                return;
            }

            lock (_syncRoot)
            {
                if (_rewardedAdProxy != null)
                {
                    return;
                }

                _rewardedAdProxy = PlatformFactory.CreateRewarded(_slotId);

                _customParams.SetCustomParamsProxy(_rewardedAdProxy.CustomParamsProxy);

                _rewardedAdProxy.AdClicked += _onAdClicked;
                _rewardedAdProxy.AdDismissed += _onAdDismissed;
                _rewardedAdProxy.AdDisplayed += _onAdDisplayed;
                _rewardedAdProxy.AdLoadCompleted += _onAdLoadCompleted;
                _rewardedAdProxy.AdLoadFailed += _onAdLoadFailed;
                _rewardedAdProxy.AdRewarded += _onAdRewarded;
                _rewardedAdProxy.AdFailedToShow += _onAdFailedToShow;

                _rewardedAdProxy.Load();
            }
        });

        public void Show() => _platformDispatcher.Perform(() =>
        {
            if (_rewardedAdProxy == null)
            {
                return;
            }

            lock (_syncRoot)
            {
                _rewardedAdProxy?.Show();
            }
        });

        protected override void DisposeImpl()
        {
            if (_rewardedAdProxy == null)
            {
                return;
            }

            lock (_syncRoot)
            {
                if (_rewardedAdProxy == null)
                {
                    return;
                }

                var referenceCopy = _rewardedAdProxy;

                _rewardedAdProxy = null;

                referenceCopy.AdClicked -= _onAdClicked;
                referenceCopy.AdDismissed -= _onAdDismissed;
                referenceCopy.AdDisplayed -= _onAdDisplayed;
                referenceCopy.AdLoadCompleted -= _onAdLoadCompleted;
                referenceCopy.AdLoadFailed -= _onAdLoadFailed;
                referenceCopy.AdRewarded -= _onAdRewarded;

                _platformDispatcher.Perform(referenceCopy.Dispose);
            }
        }

        public event EventHandler AdDismissed;
        public event EventHandler AdDisplayed;
        public event EventHandler<RewardEventArgs> AdRewarded;
        public event EventHandler AdFailedToShow;
    }
}