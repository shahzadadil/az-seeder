namespace Msa.Seeder.Steps.Azure.Storage
{
    using System;
    using Msa.Seeder.Core;
    using Msa.Seeder.Core.Configs;

    public class TableStorageConfig : StepConfig
    {
        public TableStorageConfig(
            String connectionString,
            RetryConfig retryConfig = null) : base(retryConfig)
        {
            if (String.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            this.ConnectionString = connectionString;
        }

        public String ConnectionString { get; private set; }    
    }
}