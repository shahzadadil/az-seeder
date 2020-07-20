namespace Msa.Seeder.Steps.Azure.Storage
{
    using System;
    using Msa.Seeder.Core;

    public class TableStorageConfig : StepConfig
    {
        public TableStorageConfig(String connectionString) : base(null)
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