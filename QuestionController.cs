using System;
using System.Collections.Generic;
using house_dashboard_server.Data.Factories;
using house_dashboard_server.Data.Models;
using house_dashboard_server.Models.GoogleApi;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace house_dashboard_server
{
    [ApiController]
    [Route("[controller]")]
    public class QuestionController : ControllerBase
    {
        [EnableCors("default-policy")]
        [HttpPost]
        public WebhookResponse Post(WebhookRequest request) 
            => new WebhookResponse()
            {
                fulfillmentMessages = new List<FulfillmentMessage>()
                {
                    new FulfillmentMessage()
                    {
                        text = new Text()
                        {
                            text = new List<String>
                            {
                                "The weather will be nice.",
                                "Unless it is not."
                            }
                        }
                    }
                }
            };
    }
}
