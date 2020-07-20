namespace Msa.Seeder.Steps.Azure.Storage
{
    using System;
    using System.Collections.Generic;
    using Msa.Seeder.Core;
    using Msa.Seeder.Core.Configs;
    using Msa.Seeder.Core.Models;

    public class CheckRowInTableConfig : StepConfig
    {
        public CheckRowInTableConfig(
            TableStorageConfig tableStorageConfig,
            String tableName,
            String partitionKey,
            String rowKey,
            IEnumerable<ComparisonInfo> comparisonFilters = null,
            RetryConfig retryConfig = null) : base(retryConfig)
        {
            if (tableStorageConfig == null)
            {
                throw new ArgumentNullException(nameof(tableStorageConfig));
            }

            if (String.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException(nameof(tableName));
            }

            this.TableName = tableName;
            this.StorageConfig = tableStorageConfig;
            this.PartitionKey = partitionKey;
            this.RowKey = rowKey;
            this.Filters = comparisonFilters ?? new List<ComparisonInfo>();
        }

        public TableStorageConfig StorageConfig { get; private set; }
        public String TableName { get; private set; }
        public String PartitionKey { get; private set; }
        public String RowKey { get; private set; }
        public IEnumerable<ComparisonInfo> Filters { get; private set; }
    }
}