using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AWC.Function
{
    public class OnFileUpload
    {
        private readonly ILogger<OnFileUpload> _logger;

        public OnFileUpload(ILogger<OnFileUpload> logger)
        {
            _logger = logger;
        }

        public Dictionary<string, int> getTop5Words(string content)
        {
            var words = content.Split(' ');
            var wordCount = new Dictionary<string, int>();
            foreach (var word in words)
            {
                if (wordCount.ContainsKey(word))
                {
                    wordCount[word]++;
                }
                else
                {
                    wordCount[word] = 1;
                }
            }
            return wordCount.OrderByDescending(x => x.Value).Take(5).ToDictionary(x => x.Key, x => x.Value);
        }

        [Function(nameof(OnFileUpload))]
        [SignalROutput(HubName = "serverless")]
        public async Task<SignalRMessageAction> Run([BlobTrigger("text-file-uploads/{name}", Connection = "AzureWebJobsStorage")] Stream stream, string name)
        {
            using var blobStreamReader = new StreamReader(stream);
            var content = await blobStreamReader.ReadToEndAsync();
            var top5Words = getTop5Words(content);
            _logger.LogInformation($"Top 5 words in {name}: {string.Join(", ", top5Words.Select(x => $"{x.Key} ({x.Value})"))}");

            var userId = name.Split('_')[0];
            _logger.LogInformation($"User ID: {userId}");

            SignalRMessageAction action = new SignalRMessageAction("newMessage", [top5Words])
            {
                UserId = userId
            };
            return action;
        }
    }
}
