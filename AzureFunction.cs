using CompetingConsumer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CompetingConsumer
{
    public class AzureFunction
    {
        private readonly IMessageConstruction _message;

        public AzureFunction(IMessageConstruction createMessage) => this._message = createMessage;

        [FunctionName("Sendtobus")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                // Create a new Order
                OrderApp o = new OrderApp();
                o.CreateOrders();

                // Apply Transformation
                MessageTransform m = new MessageTransform();

                // Send messages asynchronously
                await _message.CreateSendMessage(m);

                log.LogInformation("File completed processing");
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
            }

            return null;
        }
    }
}