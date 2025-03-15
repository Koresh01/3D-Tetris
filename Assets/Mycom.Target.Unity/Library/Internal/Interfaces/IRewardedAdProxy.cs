namespace Mycom.Target.Unity.Internal.Interfaces
{
    using System;

    internal interface IRewardedAdProxy : IAdProxy
    {
        void Dismiss();
        void Show();

        event Action AdDismissed;
        event Action AdDisplayed;
        event Action<String> AdRewarded;
        event Action AdFailedToShow;
    }
}