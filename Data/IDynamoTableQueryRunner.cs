using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DocumentModel;

namespace HouseDashboardServer.Data
{
    public interface IDynamoTableQueryRunner
    {
        Task<List<Document>> QueryOnTimestampRange(string tableName,
            string partionKey,
            string partitionValue,
            int days);
    }
}