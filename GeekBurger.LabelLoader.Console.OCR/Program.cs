using Microsoft.Azure.ServiceBus;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeekBurger.LabelLoader.Console.OCR
{
    class Program
    {
        static void Main(string[] args)
        {
            FileSystemWatcher watcher = new FileSystemWatcher(@"C:\Temp\Images\", "*.png");
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Changed += new FileSystemEventHandler(OnChange);
            watcher.EnableRaisingEvents = true;


            System.Console.ReadKey();
        }

        private static void OnChange(object sender, FileSystemEventArgs e)
        {

            var client = new VisionServiceClient("acff22f5f9b541c8a6849e4239c66744", "https://westcentralus.api.cognitive.microsoft.com/vision/v1.0");

            var file = File.ReadAllBytes(e.FullPath);

            Stream stream = new MemoryStream(file);

            OcrResults result = client.RecognizeTextAsync(stream, "pt", true).Result;

            var words = from r in result.Regions
                        from l in r.Lines
                        from w in l.Words
                        select w.Text;

            var output = string.Join(" ", words.ToArray());

            System.Console.WriteLine($"Complete output from OCR: {output}");


            var queueClient = new QueueClient("Endpoint=sb://geekburger.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=VrwaCn+4NbZkDFguQNGDCu2cMQ7IXyjOPLMto0HuE8Q=", "LabelLoader");

            var jsonString = JsonConvert.SerializeObject(result);

            var message = new Message(Encoding.UTF8.GetBytes(jsonString));

            queueClient.SendAsync(message).Wait();

            queueClient.CloseAsync().Wait();
        }
    }
}
