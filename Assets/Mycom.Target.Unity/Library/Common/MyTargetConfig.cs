namespace Mycom.Target.Unity.Common
{
    using System;

    public sealed class MyTargetConfig
    {
        public static Builder NewBuilder() => new Builder();

        public readonly Boolean IsTrackingEnvironmentEnabled;
        public readonly Boolean IsTrackingLocationEnabled;
        public readonly String[] TestDevices;

        private MyTargetConfig(Boolean isTrackingEnvironmentEnabled,
                               Boolean isTrackingLocationEnabled,
                               String[] testDevices)
        {
            IsTrackingEnvironmentEnabled = isTrackingEnvironmentEnabled;
            IsTrackingLocationEnabled = isTrackingLocationEnabled;
            TestDevices = testDevices;
        }

        public sealed class Builder
        {
            private const Boolean DefaultTrackingEnvironment = true;
            private const Boolean DefaultTrackingLocation = true;

            internal Builder() { }

            public static Builder From(MyTargetConfig myTargetConfig) => new Builder
            {
                trackingLocation = myTargetConfig.IsTrackingLocationEnabled,
                trackingEnvironment = myTargetConfig.IsTrackingEnvironmentEnabled,
                testDevices = myTargetConfig.TestDevices
            };

            private Boolean trackingEnvironment = DefaultTrackingEnvironment;
            private Boolean trackingLocation = DefaultTrackingLocation;
            private String[] testDevices;

            public MyTargetConfig Build() => new MyTargetConfig(trackingEnvironment, trackingLocation, testDevices);

            public Builder WithTrackingEnvironment(Boolean enabled)
            {
                trackingEnvironment = enabled;
                return this;
            }

            public Builder WithTrackingLocation(Boolean enabled)
            {
                trackingLocation = enabled;
                return this;
            }

            public Builder WithTestDevices(params String[] testDevices)
            {
                this.testDevices = testDevices;
                return this;
            }
        }
    }
}