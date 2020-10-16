using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Amazon;

namespace HouseDashboardServer.Data
{
    public class DynamoTableQueryRunner : IDynamoTableQueryRunner
    {

        private readonly IFormatProvider _culture 
            = CultureInfo.CreateSpecificCulture("en-GB");

        public Task<List<Document>> QueryOnTimestampRange(string tableName,
                                                             string partionKey,
                                                             string partitionValue,
                                                             int days)
        {
            using var client = new AmazonDynamoDBClient(RegionEndpoint.EUWest1);
            
            var table = Table.LoadTable(client, tableName);

            var queryFilter = 
                new QueryFilter(partionKey, QueryOperator.Equal, partitionValue);

            var fromDateTime = DateTime.UtcNow
                .AddDays(-days)
                .ToString("yyyy-MM-dd HH:mm:ss", _culture);

            queryFilter.AddCondition("timestamp",
                QueryOperator.GreaterThanOrEqual,
                fromDateTime);

            var queryResult = table
                .Query(queryFilter)
                .GetRemainingAsync();

            return queryResult;
        }
    }
}
