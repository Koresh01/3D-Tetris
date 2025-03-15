namespace Mycom.Target.Unity.Internal.Interfaces
{
    using System;

    internal interface IDispatcher
    {
        void Perform(Action action);
    }
}