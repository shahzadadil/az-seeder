using System;
using Microsoft.Azure.Storage;

namespace Msa.Seeder.Azure.Interface.Storage
{
    public class TableStorageRepository
    {
        private readonly String _ConnectionString;

        public TableStorageRepository(String connectionString)
        {
            if (String.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            _ConnectionString = connectionString;
        }

        public Uri TableStorageUri
        {
            get
            {
                if (!CloudStorageAccount.TryParse(this._ConnectionString, out CloudStorageAccount account))
                {
                    return null;
                } 

                return account.TableStorageUri.PrimaryUri;
            }
        }

        public String AccountName
        {
            get
            {
                if (!CloudStorageAccount.TryParse(this._ConnectionString, out CloudStorageAccount account))
                {
                    return null;
                } 

                return account.Credentials.AccountName;
            }
        }

        public String SharedKey
        {
            get
            {
                if (!CloudStorageAccount.TryParse(this._ConnectionString, out CloudStorageAccount account))
                {
                    return null;
                } 

                return System.Text.Encoding.UTF8.GetString(account.Credentials.ExportKey());
            }
        }
    }
}