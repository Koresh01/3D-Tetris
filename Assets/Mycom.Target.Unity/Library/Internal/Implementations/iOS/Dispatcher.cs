#if UNITY_IOS
namespace Mycom.Target.Unity.Internal.Implementations.iOS
{
    using System;
    using Interfaces;

    internal class Dispatcher : IDispatcher
    {
        private readonly IDispatcher _dispatcher = UnityMainThreadDispatcher.GetInstance();

        public void Perform(Action action) => _dispatcher.Perform(action);
    }
}
#endif