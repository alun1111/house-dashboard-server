using System.Collections.Generic;

namespace house_dashboard_server.Models.GoogleApi
{
    public class WebhookResponse
    {
        public List<FulfillmentMessage> fulfillmentMessages { get; set; }
    }
}