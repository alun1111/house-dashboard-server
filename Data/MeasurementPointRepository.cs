using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using house_dashboard_server.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace house_dashboard_server.Data
{
    public class MeasurementPointRepository
    {
        public MeasurementPoint GetMeasurementPoint()
        {
            var client = new AmazonDynamoDBClient(RegionEndpoint.EUWest1);
            
            var table = Table.LoadTable(client, "Weather");

            Search recentMeasurements = table.Query("MeasurementTime", 
                new QueryFilter("MeasurementTime", 
                    QueryOperator.GreaterThan, 
                    DateTime.Now.AddDays(-1)
                    )
                );

            var resultsList = recentMeasurements
                .GetRemainingAsync();

            // DO STUFF

            return new MeasurementPoint()
            {
                MeasurementTime = DateTime.Now,
                Measurements = new List<Measurement>() 
                {
                    new Measurement
                    {
                        Name = "OutsideTemperature",
                        Current = 45M,
                        Recent = new List<decimal>() { 40M, 40M, 40M, 41M, 41M, 42M }
                    }
                }
            };

        }
    }
}
