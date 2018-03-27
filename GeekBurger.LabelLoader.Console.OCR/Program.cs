using Microsoft.ProjectOxford.Vision;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using S = System;

namespace GeekBurger.LabelLoader.Console.OCR
{
    class Program
    {
        static void Main(string[] args)
        {
            FileSystemWatcher watcher = new FileSystemWatcher(@"C:\Temp\Images\", "*.png");
            watcher.NotifyFilter =  NotifyFilters.FileName;
            watcher.Created += Watcher_Created;
            watcher.EnableRaisingEvents = true;
            
            S.Console.ReadKey();
        }

        private static void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            var client = new VisionServiceClient("acff22f5f9b541c8a6849e4239c66744", "https://westcentralus.api.cognitive.microsoft.com/vision/v1.0");

            var name = File.ReadAllBytes(e.FullPath);

            using (var stream = new MemoryStream(name))
            {
                var result = client.RecognizeTextAsync(stream, "pt", true).Result;

                var words = from r in result.Regions
                            from l in r.Lines
                            from w in l.Words
                            select w.Text;

                var text = string.Join(" ", words.ToArray());

                S.Console.WriteLine($"Complete output from OCR {e.FullPath} : {text}");

                var storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=labelloaderproductimages;AccountKey=nrjPqOqdt3DRA5vj69qkcEOkeIYI64hnkuYfRvfo9LsBFZTanLoRt9gCYDQx7K2k9fxPao4+OagInG67Pdbt+Q==;EndpointSuffix=core.windows.net");

                var queueClient = storageAccount.CreateCloudQueueClient();

                var queue = queueClient.GetQueueReference("product-images");

                queue.CreateIfNotExists();

                queue.AddMessage(new CloudQueueMessage(JsonConvert.SerializeObject(new { name, text })));
            }
        }
    }
}
