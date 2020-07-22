using DFC.Compui.Telemetry.Models;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.Compui.Cosmos.Contracts
{
    public abstract class DocumentModel : RequestTrace, IDocumentModel
    {
        [Required]
        [Guid]
        [JsonProperty(PropertyName = "id", Order = -30)]
        public Guid Id { get; set; }

        [Required]
        [JsonProperty(Order = -30)]
        public abstract string? PartitionKey { get; set; }

        [JsonProperty(PropertyName = "_etag", Order = 0)]
        public string? Etag { get; set; }
    }
}
