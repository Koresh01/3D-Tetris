namespace Mycom.Target.Unity.Internal.Interfaces
{
    using System;

    internal interface IMyTargetViewProxy : IAdProxy
    {
        void SetHeight(Double value);

        void SetWidth(Double value);

        void SetX(Double value);

        void SetY(Double value);

        void Start();

        void Stop();

        event Action AdShown;
    }
}