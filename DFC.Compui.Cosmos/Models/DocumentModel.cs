using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.Compui.Cosmos.Contracts
{
    public abstract class DocumentModel : IDocumentModel
    {
        [Required]
        [Guid]
        [JsonProperty(PropertyName = "id", Order = -30)]
        public Guid Id { get; set; }

        [JsonProperty(Order = -20)]
        public abstract string? PartitionKey { get; set; }

        [JsonProperty(PropertyName = "_etag")]
        public string? Etag { get; set; }
    }
}
