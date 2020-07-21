namespace Msa.Seeder.Steps.Azure.Storage
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Msa.Seeder.Core;
    using Msa.Seeder.Azure.Interface.Storage;

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
            queryBuilder.Append($"/{this._StepConfig.TableName}");

            if (!String.IsNullOrWhiteSpace(this._StepConfig.PartitionKey) && !String.IsNullOrWhiteSpace(this._StepConfig.RowKey))
            {
                queryBuilder.Append($"(PartitionKey='{this._StepConfig.PartitionKey}',RowKey='{this._StepConfig.RowKey}')?");
            }
            else
            {
                queryBuilder.Append("()?");
            }

            // TODO: filter implementation would be the next step

            // if (this._StepConfig.Filters.Any())
            // {
            //     queryBuilder.Append("$filter=");
            // }

            // foreach (var filter in this._StepConfig.Filters)
            // {
            //     queryBuilder.Append()
            // }

            using (var tableQueryClient = new HttpClient())
            {
                var authHeaderParam = $"{storageRepository.AccountName}:{storageRepository.SharedKey}";
                tableQueryClient.DefaultRequestHeaders.Date = DateTime.UtcNow;
                tableQueryClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("SharedKey", authHeaderParam);

                var retryConfig = this._StepConfig.RetryConfig;

                do
                {
                    this._NumberOfRetries++;

                    var responseText = await tableQueryClient.GetStringAsync(queryBuilder.ToString());

                    if (!String.IsNullOrWhiteSpace(responseText))
                    {
                        return;
                    }

                } while (retryConfig.ShouldRetry && retryConfig.MaxRetries > this._NumberOfRetries);

            }
        }
    }
}