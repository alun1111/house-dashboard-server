using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DocumentModel;

namespace house_dashboard_server.Data.Interfaces
{
    public interface IDynamoTableQueryRunner
    {
        Task<List<Document>> QueryOnTimestampRange(string tableName,
            string partionKey,
            string partitionValue,
            int days);
    }
}