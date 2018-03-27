using System;
using System.IO;
using System.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.ProjectOxford.Vision;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace GeekBurger.LabelLoaderFunction
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static void Run([TimerTrigger("*/30 * * * * *")]TimerInfo myTimer, TraceWriter log)
        {
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");

            var client = new VisionServiceClient("acff22f5f9b541c8a6849e4239c66744", "https://westcentralus.api.cognitive.microsoft.com/vision/v1.0");

            var name = File.ReadAllBytes(@"C:\Temp\Images\google.png");

            using (var stream = new MemoryStream(name))
            {
                var result = client.RecognizeTextAsync(stream, "pt", true).Result;

                var words = from r in result.Regions
                            from l in r.Lines
                            from w in l.Words
                            select w.Text;

                var text = string.Join(" ", words.ToArray());
                
                var storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=labelloaderproductimages;AccountKey=nrjPqOqdt3DRA5vj69qkcEOkeIYI64hnkuYfRvfo9LsBFZTanLoRt9gCYDQx7K2k9fxPao4+OagInG67Pdbt+Q==;EndpointSuffix=core.windows.net");

                var queueClient = storageAccount.CreateCloudQueueClient();

                var queue = queueClient.GetQueueReference("product-images");

                queue.CreateIfNotExistsAsync();

                queue.AddMessageAsync(new CloudQueueMessage(JsonConvert.SerializeObject(new { name, text })));
            }
        }
    }
}
