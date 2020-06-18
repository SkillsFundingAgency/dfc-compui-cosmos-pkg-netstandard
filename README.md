# Digital First Careers - Cosmos Repository

## Introduction

This Nuget provides a Cosmos repository service for the storage/retrieval of documents in Cosmos.

## Getting Started

This is a self-contained Visual Studio 2019 solution containing a number of projects (Cosmos repository service and a unit test project) that is used to build a Nuget for consumption by Composite UI child apps.

## Using this Nuget

To use this Nuget in a Composite UI child app, add DFC.Compui.Cosmos from Nuget. Then apply the following to your Composite Child app's startup class, ConfigureServices method:

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

There are two use cases for this Nuget:

1. Simple Document service
2. Content Page service

Simple Document Service use

To register the Cosmos repository Nuget, add either of the following to the child app Startup class ConfigureServices method:

```c#
services.AddDocumentServices<DocumentModel>(cosmosDbConnectionContentPages, env.IsDevelopment());
```

To use of the Cosmos repository Nuget, inject the following in class constructors:

```c#
IDocumentService<DocumentModel> contentPageService
```

Sample use of the Cosmos repository in code:

```c#
...
var documentModels = await documentService.GetAllAsync().ConfigureAwait(false);
...
var existingDocument = await documentService.GetByIdAsync(id).ConfigureAwait(false);
...
var response = await documentService.UpsertAsync(documentModel).ConfigureAwait(false);
...
var isDeleted = await documentService.documentService(id).ConfigureAwait(false);
...
```

Content Page Service use

To register the Cosmos repository Nuget, add either of the following to the child app Startup class ConfigureServices method:

```c#
services.AddContentPageServices<ContentPageModel>(cosmosDbConnectionContentPages, env.IsDevelopment());
```

To use of the Cosmos repository Nuget, inject the following in class constructors:

```c#
IContentPageService<ContentPageModel> contentPageService
```

Sample use of the Cosmos repository in code:

```c#
...
var contentPageModels = await contentPageService.GetAllAsync().ConfigureAwait(false);
...
var existingDocument = await contentPageService.GetByIdAsync(id).ConfigureAwait(false);
...
var contentPageModel = await contentPageService.GetByNameAsync(canonicalName).ConfigureAwait(false);
...
var contentPageModel = await contentPageService.GetByAlternativeNameAsync(alternativeName).ConfigureAwait(false);
...
var response = await contentPageService.UpsertAsync(contentPageModel).ConfigureAwait(false);
...
var isDeleted = await contentPageService.DeleteAsync(id).ConfigureAwait(false);
...
```



In the above sample code, the ContentPageModel is a model inherited from the DFC.Compui.Cosmos.Models.ContentPageModel as follows:

```c#
public class ContentPageModel : DFC.Compui.Cosmos.Models.ContentPageModel
{
    public override string PartitionKey => "static-page";
    public string? Content { get; set; }
    public DateTime LastReviewed { get; set; }
}
```
## Built With

* Microsoft Visual Studio 2019
* .Net Standard 2.1
