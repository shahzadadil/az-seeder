namespace Msa.Seeder.Steps.Azure.Storage
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Msa.Seeder.Core;
    using Msa.Seeder.Azure.Interface.Storage;
    using System.Globalization;
    using Microsoft.Azure.Cosmos.Table;
    using System.Threading;

    public class CheckRowInTableStep : Step<CheckRowInTableConfig>
    {
        private Int32 _NumberOfRetries = 0;

        public override async Task Execute()
        {
            var storageRepository = new TableStorageRepository(this._StepConfig.StorageConfig.ConnectionString);
            var storageAccountUri = storageRepository.TableStorageUri;

            if (storageAccountUri == null || String.IsNullOrWhiteSpace(storageAccountUri.AbsoluteUri))
            {
                throw new UriFormatException("The table storage account Uri in invalid");
            }

            var queryBuilder = new StringBuilder(storageAccountUri.AbsoluteUri);
            queryBuilder.Append($"{this._StepConfig.TableName}");

            var baseUrl = queryBuilder.ToString();

            if (!String.IsNullOrWhiteSpace(this._StepConfig.PartitionKey) && !String.IsNullOrWhiteSpace(this._StepConfig.RowKey))
            {
                queryBuilder.Append($"(PartitionKey='{this._StepConfig.PartitionKey}',RowKey='{this._StepConfig.RowKey}')");
            }
            else
            {
                queryBuilder.Append("()");
            }

            var retryConfig = this._StepConfig.RetryConfig;

            var storageAccount = new CloudStorageAccount(
                new StorageCredentials(storageRepository.AccountName, storageRepository.SharedKey),
                true);
            
            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference(this._StepConfig.TableName);

            var tableQueryOperation = TableOperation.Retrieve(
                this._StepConfig.PartitionKey,
                this._StepConfig.RowKey);
            
            do
            {
                this._NumberOfRetries++;

                var entity = await table.ExecuteAsync(tableQueryOperation);

                if (entity.Result != null)
                {
                    return;
                }

                Thread.Sleep(this._StepConfig.RetryConfig.RetryInterval);

            } while (retryConfig.ShouldRetry && retryConfig.MaxRetries > this._NumberOfRetries);

            // If retries have been exhausted, it is not successful
            throw new TimeoutException("Record not found after timeout");
        }
    }
}