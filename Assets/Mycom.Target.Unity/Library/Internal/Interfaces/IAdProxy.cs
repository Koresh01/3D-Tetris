namespace Mycom.Target.Unity.Internal.Interfaces
{
    using System;

    internal interface IAdProxy : IDisposable
    {
        ICustomParamsProxy CustomParamsProxy { get; }

        void Load();

        event Action AdClicked;
        event Action AdLoadCompleted;
        event Action<Int64, String> AdLoadFailed;
    }
}