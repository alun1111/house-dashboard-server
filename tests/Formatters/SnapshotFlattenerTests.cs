using System.Collections.Generic;
using System.Linq;
using Amazon.Runtime.Internal.Transform;
using house_dashboard_server.Data.Models;
using house_dashboard_server.Formatters;
using NUnit.Framework;

namespace tests.Formatters
{
    public class SnapshotFlattenerTests
    {
        private SnapshotFlattener systemUnderTest;

        [SetUp]
        public void Setup()
        {
            this.systemUnderTest = new SnapshotFlattener();
        }

        [Test]
        public void ReturnsHeader()
        {
            var input = new Dictionary<string, List<SnapshotItem>>();
            input.Add(SingleKey("16/03/2022 10:00:00"));

            var expected = new object[] { "DateTime", "Whitburn - Rainfall", "Whitburn - River Level" };

            var result = systemUnderTest.Flatten(input);
            var header = result.First();
            
            Assert.AreEqual(expected, header);
        }

        [Test]
        public void ReturnsFlatListFromSingleKey()
        {
            var input = new Dictionary<string, List<SnapshotItem>>();
            input.Add(SingleKey("16/03/2022 10:00:00"));

            var expected = new List<object[]>()
            {
                { new object[] { "DateTime", "Whitburn - Rainfall", "Whitburn - River Level" } },
                { new object[] { "16/03/2022 10:00:00", 1.2M, 0.4M } }
            };

            var result = systemUnderTest.Flatten(input);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ReturnsFlatListFromMultiKey()
        {
            var input = new Dictionary<string, List<SnapshotItem>>()
            {
                SingleKey("16/03/2022 10:00:00"),
                SingleKey("17/03/2022 10:00:00")
            };

            var expected = new List<object[]>()
            {
                { new object[] { "DateTime", "Whitburn - Rainfall", "Whitburn - River Level" } },
                { new object[] { "16/03/2022 10:00:00", 1.2M, 0.4M } },
                { new object[] { "17/03/2022 10:00:00", 1.2M, 0.4M } }
            };

            var result = systemUnderTest.Flatten(input);

            Assert.AreEqual(expected, result);
        }
        
        // Next tests: handle snapshots with different columns (i.e. ordering of values under columns)
        
        private static KeyValuePair<string, List<SnapshotItem>> SingleKey(string dateTime)
        {
            return new KeyValuePair<string, List<SnapshotItem>>(
                dateTime,
                new List<SnapshotItem>()
                {
                    new()
                    {
                        Description = "Whitburn - Rainfall",
                        Value = 1.2M
                    },
                    new()
                    {
                        Description = "Whitburn - River Level",
                        Value = 0.4M
                    }
                });
        }
    }
}