namespace Mycom.Target.Unity.Internal.Interfaces
{
    using System;

    internal interface IInterstitialAdProxy : IAdProxy
    {
        void Dismiss();
        void Show();

        event Action AdDismissed;
        event Action AdDisplayed;
        event Action AdVideoCompleted;
        event Action AdFailedToShow;
    }
}