namespace Msa.Seeder.Core.Configs
{
    using System;

    public class RetryConfig
    {
        public RetryConfig(
            Int32 retryIntervalSeconds,
            Int32 maxRetries)
        {
            if (retryIntervalSeconds <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(retryIntervalSeconds));
            }    

            if (maxRetries < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxRetries));
            }

            this.ShouldRetry = maxRetries > 0;
            this.MaxRetries = maxRetries;
            this.RetryIntervalSecs = retryIntervalSeconds;
        }

        public Boolean ShouldRetry { get; private set;}
        public Int32 RetryIntervalSecs { get; private set; }
        public Int32 MaxRetries { get; private set; }
    }
}