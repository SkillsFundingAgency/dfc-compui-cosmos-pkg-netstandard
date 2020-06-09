using DFC.Compui.Cosmos.Attributes;
using DFC.Compui.Cosmos.Contracts;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DFC.Compui.Cosmos.Models
{
    public class ContentPageModel : DocumentModel, IContentPageModel
    {
        [Required]
        [JsonProperty(Order = -10)]
        public override string? PartitionKey { get; set; } = "unknown-value";

        [Required]
        [LowerCase]
        [UrlPath]
        [JsonProperty(Order = -10)]
        public string? CanonicalName { get; set; }

        [UrlPath]
        [LowerCase]
        [JsonProperty(Order = -10)]
        public IList<string>? AlternativeNames { get; set; }
    }
}
