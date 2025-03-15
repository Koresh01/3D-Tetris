using System;
using Mycom.Target.Unity.Internal;
using Mycom.Target.Unity.Internal.Interfaces;

namespace Mycom.Target.Unity.Ads
{
    public sealed class MyTargetView : AbstractAd
    {
        public enum AdSize
        {
            Size320x50 = 0,
            Size300x250 = 1,
            Size728x90 = 2
        }

        private const Double Height300x250 = 250.0;
        private const Double Height320x50 = 50.0;
        private const Double Height728x90 = 90.0;
        private const Double Width320x50 = 320.0;
        private const Double Width300x250 = 300.0;
        private const Double Width728x90 = 728.0;

        private readonly Object _syncRoot = new Object();
        private readonly AdSize _adSize;
        private readonly Boolean _isRefreshAd;
        private readonly UInt32 _slotId;

        private readonly Action _onAdShown;

        private volatile IMyTargetViewProxy _myTargetViewProxy;

        private Double _height;
        private Double _width;
        private Double _x;
        private Double _y;

        public Double Height
        {
            get => _height;
            set
            {
                _height = value;
                _platformDispatcher.Perform(() => { lock (_syncRoot) _myTargetViewProxy?.SetHeight(value); });
            }
        }

        public Double Width
        {
            get => _width;
            set
            {
                _width = value;
                _platformDispatcher.Perform(() => { lock (_syncRoot) _myTargetViewProxy?.SetWidth(value); });
            }
        }

        public Double X
        {
            get => _x;
            set
            {
                _x = value;
                _platformDispatcher.Perform(() => { lock (_syncRoot) _myTargetViewProxy?.SetX(value); });
            }
        }

        public Double Y
        {
            get => _y;
            set
            {
                _y = value;
                _platformDispatcher.Perform(() => { lock (_syncRoot) _myTargetViewProxy?.SetY(value); });
            }
        }

        public MyTargetView(UInt32 slotId,
                            AdSize adSize = AdSize.Size320x50,
                            Boolean isRefreshAd = true)
        {
            MyTargetLogger.Log("MyTarget.Unity(" + SDKVersion.Version + "): MyTargetView is created");

            _onAdShown = () => _unityDispatcher.Perform(() => AdShown?.Invoke(this, EventArgs.Empty));

            _slotId = slotId;
            _adSize = adSize;
            _isRefreshAd = isRefreshAd;

            switch (adSize)
            {
                case AdSize.Size300x250:
                    _height = Height300x250;
                    _width = Width300x250;
                    break;
                case AdSize.Size728x90:
                    _height = Height728x90;
                    _width = Width728x90;
                    break;
                default:
                    _height = Height320x50;
                    _width = Width320x50;
                    break;
            }
        }

        public override void Load() => _platformDispatcher.Perform(() =>
        {
            if (_myTargetViewProxy != null)
            {
                return;
            }

            lock (_syncRoot)
            {
                if (_myTargetViewProxy != null)
                {
                    return;
                }

                _myTargetViewProxy = PlatformFactory.CreateMyTargetControl(_slotId, _adSize, _isRefreshAd);

                _customParams.SetCustomParamsProxy(_myTargetViewProxy.CustomParamsProxy);

                _myTargetViewProxy.SetHeight(_height);
                _myTargetViewProxy.SetWidth(_width);
                _myTargetViewProxy.SetX(_x);
                _myTargetViewProxy.SetY(_y);

                _myTargetViewProxy.AdClicked += _onAdClicked;
                _myTargetViewProxy.AdLoadCompleted += _onAdLoadCompleted;
                _myTargetViewProxy.AdLoadFailed += _onAdLoadFailed;
                _myTargetViewProxy.AdShown += _onAdShown;

                _myTargetViewProxy.Load();
            }
        });

        public void Start() => _platformDispatcher.Perform(() =>
        {
            if (_myTargetViewProxy == null)
            {
                return;
            }

            lock (_syncRoot)
            {
                _myTargetViewProxy?.Start();
            }
        });

        public void Stop() => _platformDispatcher.Perform(() =>
        {
            if (_myTargetViewProxy == null)
            {
                return;
            }

            lock (_syncRoot)
            {
                _myTargetViewProxy?.Stop();
            }
        });

        protected override void DisposeImpl()
        {
            if (_myTargetViewProxy == null)
            {
                return;
            }

            lock (_syncRoot)
            {
                if (_myTargetViewProxy == null)
                {
                    return;
                }

                var referenceCopy = _myTargetViewProxy;
                _myTargetViewProxy = null;

                referenceCopy.AdClicked -= _onAdClicked;
                referenceCopy.AdLoadCompleted -= _onAdLoadCompleted;
                referenceCopy.AdLoadFailed -= _onAdLoadFailed;
                referenceCopy.AdShown -= _onAdShown;

                _platformDispatcher.Perform(referenceCopy.Dispose);
            }
        }

        public event EventHandler AdShown;
    }
}