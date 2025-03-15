using System;
using Mycom.Target.Unity.Internal;
using Mycom.Target.Unity.Internal.Interfaces;

namespace Mycom.Target.Unity.Ads
{
    public abstract class AbstractAd : IDisposable
    {
        private Boolean _isDisposed;

        internal readonly CustomParams _customParams = new CustomParams();
        internal readonly IDispatcher _platformDispatcher = PlatformFactory.CreateDispatcher();
        internal readonly IDispatcher _unityDispatcher = UnityMainThreadDispatcher.GetInstance();

        internal readonly Action _onAdClicked;
        internal readonly Action _onAdLoadCompleted;
        internal readonly Action<Int64, String> _onAdLoadFailed;

        public CustomParams CustomParams => _customParams;

        protected AbstractAd()
        {
            _onAdClicked = () => _unityDispatcher.Perform(() => AdClicked?.Invoke(this, EventArgs.Empty));
            _onAdLoadCompleted = () => _unityDispatcher.Perform(() => AdLoadCompleted?.Invoke(this, EventArgs.Empty));
            _onAdLoadFailed = (code, error) => _unityDispatcher.Perform(() => AdLoadFailed?.Invoke(this, new ErrorEventArgs(code, error)));
        }

        ~AbstractAd()
        {
            Dispose();
        }

        public abstract void Load();

        public event EventHandler AdClicked;
        public event EventHandler AdLoadCompleted;
        public event EventHandler<ErrorEventArgs> AdLoadFailed;

        public void Dispose()
        {
            if (_isDisposed == true)
            {
                return;
            }

            _isDisposed = true;

            _customParams.Dispose();

            DisposeImpl();

            GC.SuppressFinalize(this);
        }

        protected abstract void DisposeImpl();
    }
}