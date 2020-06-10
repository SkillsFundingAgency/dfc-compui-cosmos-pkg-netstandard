using DFC.Compui.Cosmos.Contracts;
using DFC.Compui.Cosmos.Models;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace DFC.Compui.Cosmos.UnitTests.Models
{
    public class TestContentPageModel : ContentPageModel, IContentPageModel
    {
        [Required]
        [JsonProperty(Order = -10)]
        public override string? PartitionKey { get; set; } = "test-value";
    }
}
