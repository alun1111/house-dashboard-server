using System.Collections.Generic;

namespace house_dashboard_server.Models.GoogleApi
{
    public class QueryResult
    {
        public string queryText { get; set; }
        public Parameters parameters { get; set; }
        public bool allRequiredParamsPresent { get; set; }
        public string fulfillmentText { get; set; }
        public List<FulfillmentMessage> fulfillmentMessages { get; set; }
        public List<OutputContext> outputContexts { get; set; }
        public Intent intent { get; set; }
        public int intentDetectionConfidence { get; set; }
        public DiagnosticInfo diagnosticInfo { get; set; }
        public string languageCode { get; set; }
    }
}