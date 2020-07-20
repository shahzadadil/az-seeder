namespace Msa.Seeder.Core.Configs
{
    using System;

    public class RetryConfig
    {
        public RetryConfig(
            Int32 retryInterval,
            Int32 maxRetries)
        {
            if (retryInterval <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(retryInterval));
            }    

            if (maxRetries < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxRetries));
            }

            this.ShouldRetry = maxRetries > 0;
            this.MaxRetries = maxRetries;
            this.RetryInterval = retryInterval;
        }

        public Boolean ShouldRetry { get; private set;}
        public Int32 RetryInterval { get; private set; }
        public Int32 MaxRetries { get; private set; }
    }
}