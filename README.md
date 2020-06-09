# Digital First Careers - Cosmos Repository

## Introduction

This Nuget provides a Cosmos repository service for the storage/retrieval of documents in Cosmos.

## Getting Started

This is a self-contained Visual Studio 2019 solution containing a number of projects (Cosmos repository service and a unit test project) that is used to build a Nuget for consumption by Composite UI child apps.

## Using this Nuget

To use this Nuget in a Composit UI child app, add DFC.Compui.Cosmos from Nuget. Then apply the following to your Composite Child app's startup class, ConfigureServices method:

```c#
var cosmosDbConnectionContentPages = configuration.GetSection("Configuration:CosmosDbConnections:ContentPages").Get<CosmosDbConnection>();
            
services.AddDocumentServices<ContentPageModel>(cosmosDbConnectionContentPages, env.IsDevelopment());
```

The appsettings for the Cosmos connection should be loaded into a CosmosDbConnection model and passed into the AddDocumentServices method. For example:

```json
{
  "Configuration": {
    "CosmosDbConnections": {
      "ContentPages": {
        "AccessKey": "[ Cosmos access key ]",
        "EndpointUrl": "[ Cosmos endpoint ]",
        "DatabaseId": "[ Database name ]",
        "CollectionId": "[ Collection name",
        "PartitionKey": "[ Partition field path ]"
      }
    }
  }
}
```

To access the Cosmos service, inject the following in to class constructors:

```c#
IContentPageService<ContentPageModel> contentPageService
```

The Cosmos service makes available the following methods:

```c#
...
var contentPageModels = await contentPageService.GetAllAsync().ConfigureAwait(false);
...
var existingDocument = await contentPageService.GetByIdAsync(id).ConfigureAwait(false);
...
var contentPageModel = await contentPageService.GetByNameAsync(canonicalName).ConfigureAwait(false);
...
var contentPageModel = await contentPageService.GetByAlternativeNameAsync(alternativeName).ConfigureAwait(false);
...var response = await contentPageService.UpsertAsync(contentPageModel).ConfigureAwait(false);
...
var isDeleted = await contentPageService.DeleteAsync(id).ConfigureAwait(false);
...

```

In the above sample code, the ContentPageModel is a model inherited from the DFC.Compui.Cosmos.Models.ContentPageModel as follows:

```c#
public class ContentPageModel : DFC.Compui.Cosmos.Models.ContentPageModel
{
    public override string PartitionKey => "static-page";
    public long SequenceNumber { get; set; }
    public string? Content { get; set; }
    public DateTime LastReviewed { get; set; }
}
```
## Built With

* Microsoft Visual Studio 2019
* .Net Standard 2.1
