using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace house_dashboard_server.Data
{
    public class DynamoTableQueryRunner
    {
        public async Task<List<Document>> QueryDynamoDbTable(AmazonDynamoDBClient client,
                                                                     string tableName,
                                                                     string partionKey,
                                                                     string partitionValue)
        {
            var table = Table.LoadTable(client, tableName);

            var queryFilter = 
                new QueryFilter(partionKey, QueryOperator.Equal, partitionValue);

            queryFilter.AddCondition("timestamp",
                QueryOperator.GreaterThan,
                DateTime.UtcNow.AddDays(-1));

            var queryResult = await table
                .Query(queryFilter)
                .GetRemainingAsync()
                .ConfigureAwait(false);

            return queryResult;
        }
    }
}
