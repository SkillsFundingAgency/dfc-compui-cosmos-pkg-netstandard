using DFC.Compui.Cosmos.Contracts;
using DFC.Compui.Cosmos.Models;
using System;

namespace DFC.Compui.Cosmos.UnitTests.Models
{
    public class TestContentPageModel : ContentPageModel, IContentPageModel
    {
        [Obsolete("May be removed once Service Bus and Message Function app removed from solution")]
        public long SequenceNumber { get; set; }

        public Guid? Version { get; set; }

        public string? BreadcrumbTitle { get; set; }

        public bool IncludeInSitemap { get; set; }

        public Uri? Url { get; set; }

        public string? Content { get; set; }

        public DateTime LastReviewed { get; set; }
    }
}
