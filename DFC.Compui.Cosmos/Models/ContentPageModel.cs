using DFC.Compui.Cosmos.Attributes;
using DFC.Compui.Cosmos.Contracts;
using DFC.Compui.Cosmos.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DFC.Compui.Cosmos.Models
{
    [ExcludeFromCodeCoverage]
    public abstract class ContentPageModel : DocumentModel, IContentPageModel
    {
        [Required]
        [JsonProperty(Order = -20)]
        public override string? PartitionKey { get; set; } = "unknown-value";

        [Required]
        [LowerCase]
        [UrlPath]
        [JsonProperty(Order = -20)]
        public string? CanonicalName { get; set; }

        [JsonProperty(Order = -20)]
        public bool IsDefaultForPageLocation { get; set; }

        [Required]
        [LowerCase]
        [UrlPath]
        [JsonProperty(Order = -20)]
        public abstract string? PageLocation { get; set; }

        [UrlPath]
        [LowerCase]
        [JsonProperty(Order = -20)]
        public IList<string>? RedirectLocations { get; set; }

        [JsonProperty(Order = -20)]
        [Display(Name = "Include In SiteMap")]
        public bool IncludeInSitemap { get; set; }

        [JsonProperty(Order = -20)]
        [Display(Name = "SiteMap Priority")]
        public double SiteMapPriority { get; set; }

        [JsonProperty(Order = -20)]
        [Display(Name = "SiteMap Change Frequency")]
        public SiteMapChangeFrequency SiteMapChangeFrequency { get; set; }

        [Required]
        [JsonProperty(Order = -20)]
        public Uri? Url { get; set; }

        [Required]
        [JsonProperty(Order = -20)]
        public Guid? Version { get; set; }

        [Required]
        [JsonProperty(Order = -20)]
        [Display(Name = "Last Reviewed")]
        public DateTime? LastReviewed { get; set; }

        [Required]
        [JsonProperty(Order = -20)]
        [Display(Name = "Last Cached")]
        public DateTime LastCached { get; set; } = DateTime.UtcNow;

        [Required]
        [JsonProperty(Order = -20)]
        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }

        [JsonProperty(Order = -20)]
        public MetaTagsModel MetaTags { get; set; } = new MetaTagsModel();

        [Required]
        [JsonProperty(Order = -20)]
        public string? Content { get; set; }
    }
}
