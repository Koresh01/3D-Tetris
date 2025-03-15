using System;

namespace Mycom.Target.Unity.Ads
{
    public sealed class RewardEventArgs : EventArgs
    {
        public readonly String type;

        internal RewardEventArgs(String type)
        {
            this.type = type;
        }
    }
}