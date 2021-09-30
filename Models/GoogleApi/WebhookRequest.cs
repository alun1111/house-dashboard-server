namespace house_dashboard_server.Models.GoogleApi
{
    public class WebhookRequest
    {
        public string responseId { get; set; }
        public string session { get; set; }
        public QueryResult queryResult { get; set; }
        public OriginalDetectIntentRequest originalDetectIntentRequest { get; set; }
    }
}