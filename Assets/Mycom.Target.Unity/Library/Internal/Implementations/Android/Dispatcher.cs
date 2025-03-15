#if UNITY_ANDROID
namespace Mycom.Target.Unity.Internal.Implementations.Android
{
    using System;
    using Interfaces;

    internal sealed class Dispatcher : IDispatcher
    {
        private readonly IDispatcher _dispatcher = UnityMainThreadDispatcher.GetInstance();

        public void Perform(Action action) => _dispatcher.Perform(() => PlatformHelper.RunInUiThread(action));
    }
}
#endif