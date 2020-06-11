using System;

namespace DFC.Compui.Cosmos.Contracts
{
    public interface IDocumentModel
    {
        Guid Id { get; set; }

        string? Etag { get; set; }

        string? PartitionKey { get; set; }
    }
}
