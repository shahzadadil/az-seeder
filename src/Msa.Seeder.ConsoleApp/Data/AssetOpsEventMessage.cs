namespace Msa.Seeder.ConsoleApp.Data
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    
    /// <summary>
    /// This is the event which is raised when an operation is performed on an asset
    /// </summary>
    public class AssetOperationalEvent
    {
        /// <summary>
        /// Unique identifier for the event
        /// </summary>
        [JsonProperty("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// The id of the asset on which an operation was carried on
        /// </summary>
        [JsonProperty("assetId")]
        public Int32 AssetId { get; set; }

        /// <summary>
        /// The identity which initiated the operation
        /// </summary>
        [JsonProperty("identityId")]
        public Int32 IdentityId { get; set; }

        /// <summary>
        /// The domain which initiated the operation on the asset
        /// </summary>
        [JsonProperty("origin")]
        public String Origin { get; set; }

        /// <summary>
        /// The date and time of the event in UTC
        /// </summary>
        [JsonProperty("eventDateTime")]
        public DateTime EventDateTime { get; set; }

        /// <summary>
        /// The details of the event
        /// </summary>
        [JsonProperty("eventData")]
        public String EventData { get; set; }

        /// <summary>
        /// Collection of key value pairs for extra data
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<String, String> Metadata { get; set; }

        /// <summary>
        /// The status of the operation on the asset
        /// </summary>
        [JsonProperty("status")]
        public AssetOperationalEventStatus Status { get; set; }

    }

    public enum AssetOperationalEventStatus
    {
        InProgress,
        Succeeded,
        Failed
    }
}