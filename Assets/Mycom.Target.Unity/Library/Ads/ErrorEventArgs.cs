using System;

namespace Mycom.Target.Unity.Ads
{
    public sealed class ErrorEventArgs : EventArgs
    {
        public String Message { get; private set; }

        public Int64 Code { get; private set; }

        internal ErrorEventArgs(Int64 code, String message)
        {
            Code = code;
            Message = message;
        }
    }
}