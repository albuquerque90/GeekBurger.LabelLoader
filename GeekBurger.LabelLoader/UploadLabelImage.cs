using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace GeekBurger.LabelLoader
{
    public static class UploadLabelImage
    {
        private const string TopicName = "LabelImage";
        private static IConfiguration _configuration;
        private static ServiceBusConfiguration serviceBusConfiguration;
        private const string SubscriptionName = "Los_Angeles_Pasadena_store";

        [FunctionName("UploadLabelImage")]
        public static void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, TraceWriter log)
        {
            //TODO: Refatorar validação da criação do Topic
            serviceBusConfiguration = _configuration.GetSection("serviceBus").Get<ServiceBusConfiguration>();

            var serviceBusNamespace = _configuration.GetServiceBusNamespace();

            var topic = serviceBusNamespace.Topics.GetByName(TopicName);

            if (!topic.Subscriptions.List()
                   .Any(subscription => subscription.Name
                       .Equals(SubscriptionName, StringComparison.InvariantCultureIgnoreCase)))
                topic.Subscriptions
                    .Define(SubscriptionName)
                    .Create();

            //TODO: Fazer o processamento das imagens
            //TODO: Enfieleirar JSON do resultado do processamento

            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
