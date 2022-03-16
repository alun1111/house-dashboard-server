using System.Collections.Generic;
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
        public void ReturnsFlatListFromSingleKey()
        {
            var input = new Dictionary<string, List<SnapshotItem>>()
            {
                {
                    "16/03/2022 10:00:00",
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
                    }
                }
            };

            var expected = new List<object[]>()
            {
                { new object[] { "DateTime", "Whitburn - Rainfall", "Whitburn - River Level" } },
                { new object[] { "16/03/2022 10:00:00", 1.2M, 0.4M } }
            };

            var result = systemUnderTest.Flatten(input);

            Assert.AreEqual(expected, result);
        }
    }
}