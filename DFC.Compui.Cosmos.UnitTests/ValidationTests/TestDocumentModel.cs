using DFC.Compui.Cosmos.Contracts;

namespace DFC.Compui.Cosmos.UnitTests.ValidationTests
{
    public class TestDocumentModel : DocumentModel
    {
        public override string? PartitionKey { get; set; } = "test-value";
    }
}
