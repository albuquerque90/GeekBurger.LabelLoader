using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.ProjectOxford.Vision;
using System.Text;

namespace GeekBurger.LabelLoader
{
    public static class FunctionLabelLoader
    {
        private static IConfiguration _configuration;
        private static ServiceBusConfiguration serviceBusConfiguration;

        const string VisionApiKey = "acff22f5f9b541c8a6849e4239c66744";
        const string QueueName = "LabelLoader";

        [FunctionName("FunctionLabelloader ")]
        public async static void Run([BlobTrigger("LabelImage/{name}", Connection = "DefaultEndpointsProtocol=https;AccountName=labelloaderproductimages;AccountKey=nrjPqOqdt3DRA5vj69qkcEOkeIYI64hnkuYfRvfo9LsBFZTanLoRt9gCYDQx7K2k9fxPao4+OagInG67Pdbt+Q==;EndpointSuffix=core.windows.net")]Stream myBlob, string name, TraceWriter log)
        {
            log.Info($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

            serviceBusConfiguration = _configuration.GetSection("serviceBus").Get<ServiceBusConfiguration>();

            var client = new VisionServiceClient(VisionApiKey);

            var result = await client.RecognizeTextAsync(myBlob, languageCode: "pt", detectOrientation: true);

            var queueClient =  new QueueClient(serviceBusConfiguration.ConnectionString, QueueName);

            var jsonString = JsonConvert.SerializeObject(result);

            var message = new Message(Encoding.UTF8.GetBytes(jsonString));

            await queueClient.SendAsync(message);

            await queueClient.CloseAsync();
        }
    }
}
