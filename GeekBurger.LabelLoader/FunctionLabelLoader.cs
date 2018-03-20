using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.ProjectOxford.Vision;
using System.Text;
using System.Configuration;

namespace GeekBurger.LabelLoader
{
    public static class FunctionLabelLoader
    {
        private static ServiceBusConfiguration serviceBusConfiguration;

        const string VisionApiKey = "acff22f5f9b541c8a6849e4239c66744";
        const string QueueName = "product-images";

        [FunctionName("FunctionLabelloader")]
        public async static void Run([BlobTrigger("product-images/{name}", Connection = "DefaultEndpointsProtocol=https;AccountName=labelloaderproductimages;AccountKey=nrjPqOqdt3DRA5vj69qkcEOkeIYI64hnkuYfRvfo9LsBFZTanLoRt9gCYDQx7K2k9fxPao4+OagInG67Pdbt+Q==;EndpointSuffix=core.windows.net")]Stream myBlob, string name, TraceWriter log)
        {
            log.Info($"GEEKBURGER: Function Label Loader");

            log.Info($"Lendo dados de configuração do Service Bus...");
            serviceBusConfiguration = new ServiceBusConfiguration();
            serviceBusConfiguration.ResourceGroup = ConfigurationManager.AppSettings["serviceBus:resourceGroup"];
            serviceBusConfiguration.NamespaceName = ConfigurationManager.AppSettings["serviceBus:namespaceName"];
            serviceBusConfiguration.ConnectionString = ConfigurationManager.AppSettings["serviceBus:connectionString"];
            serviceBusConfiguration.ClientId = ConfigurationManager.AppSettings["serviceBus:clientId"];
            serviceBusConfiguration.ClientSecret = ConfigurationManager.AppSettings["serviceBus:clientSecret"];
            serviceBusConfiguration.SubscriptionId = ConfigurationManager.AppSettings["serviceBus:subscriptionId"];
            serviceBusConfiguration.TenantId = ConfigurationManager.AppSettings["serviceBus:tenantId"];

            log.Info($"Processando imagem via serviço cognitivo...");
            var client = new VisionServiceClient(VisionApiKey);
            var result = await client.RecognizeTextAsync(myBlob, languageCode: "pt", detectOrientation: true);

            log.Info($"Enfileirando resultado...");
            var queueClient =  new QueueClient(serviceBusConfiguration.ConnectionString, QueueName);
            var jsonString = JsonConvert.SerializeObject(result);
            var message = new Message(Encoding.UTF8.GetBytes(jsonString));
            await queueClient.SendAsync(message);
            await queueClient.CloseAsync();
        }
    }
}
