using DFC.Compui.Cosmos.Contracts;
using Newtonsoft.Json;
using System;

namespace DFC.Compui.Cosmos.UnitTests.Models
{
    public class TestDocumentModel : DocumentModel
    {
        [JsonProperty(Order = -10)]
        public override string? PartitionKey { get; set; } = "test-value";

        public string? Content { get; set; }

        public DateTime LastReviewed { get; set; }
    }
}
