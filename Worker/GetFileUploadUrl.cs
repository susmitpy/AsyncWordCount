// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Azure.Functions.Worker;
// using Microsoft.Extensions.Logging;
// using Azure.Storage.Blobs;
// using Azure.Storage.Sas;
// using Microsoft.Azure.Functions.Worker.Http;
// using FromBody = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;


// namespace AWC.Function
// {
//     public class GetFileUploadURL
//     {
//         private readonly ILogger<GetFileUploadURL> _logger;
//         const string containerName = "text-file-uploads";

//         public GetFileUploadURL(ILogger<GetFileUploadURL> logger)
//         {
//             _logger = logger;
//         }

//         private string GenerateUri(string connectionId)
//         {
//             string storageAccountConnectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
//             DateTimeOffset expiryTime = DateTime.UtcNow.AddHours(1);
//             var blobServiceClient = new BlobServiceClient(storageAccountConnectionString);
//             var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

//             var blobName = $"{connectionId}_{Guid.NewGuid()}.txt";
//             var blobClient = containerClient.GetBlobClient(blobName);

//             var sasBuilder = new BlobSasBuilder
//             {
//                 BlobContainerName = containerName,
//                 BlobName = blobName,
//                 Resource = "b",
//                 StartsOn = DateTimeOffset.UtcNow,
//                 ExpiresOn = expiryTime
//             };

//             sasBuilder.SetPermissions(BlobSasPermissions.Write);
//             var sasToken = blobClient.GenerateSasUri(sasBuilder);
//             var sasUrl = sasToken.ToString();
//             return sasUrl;
//         }

//         [Function("GetFileUploadURL")]
//         public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
//         {
//             _logger.LogInformation("C# HTTP trigger function processed a request.");

//             var uri = GenerateUri("1243");
//             return new OkObjectResult(new
//             {
//                 uri = uri.ToString()
//             });
//         }
//     }

//     public record ReqData(string connectionId);
// }
