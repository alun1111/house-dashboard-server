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

        /// <summary>
        /// Header should contain datetime, <col1>, <col2>
        /// </summary>
        [Test]
        public void ReturnsHeader()
        {
            var input = new Dictionary<string, List<SnapshotItem>>();
            input.Add(SingleKey("16/03/2022 10:00:00"));

            var expected = new string[] { "DateTime", "Whitburn - Rainfall", "Whitburn - River Level" };

            var result = systemUnderTest.Flatten(input);
            var header = result.First();
            
            Assert.AreEqual(expected, header);
        }

        /// <summary>
        /// For one snapshot, should just have header and row with correct columns
        /// </summary>
        [Test]
        public void ReturnsFlatListFromSingleKey()
        {
            var input = new Dictionary<string, List<SnapshotItem>>();
            input.Add(SingleKey("16/03/2022 10:00:00"));

            var expected = new List<string[]>()
            {
                { new string[] { "DateTime", "Whitburn - Rainfall", "Whitburn - River Level" } },
                { new string[] { "16/03/2022 10:00:00", "1.2", "0.4" } }
            };

            var result = systemUnderTest.Flatten(input);

            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// For two snapshots, header and two rows with correct columns
        /// </summary>
        [Test]
        public void ReturnsFlatListFromMultiKey()
        {
            var input = new Dictionary<string, List<SnapshotItem>>()
            {
                SingleKey("16/03/2022 10:00:00"),
                SingleKey("17/03/2022 10:00:00")
            };

            var expected = new List<string[]>()
            {
                { new string[] { "DateTime", "Whitburn - Rainfall", "Whitburn - River Level" } },
                { new string[] { "16/03/2022 10:00:00", "1.2", "0.4" } },
                { new string[] { "17/03/2022 10:00:00", "1.2", "0.4" } }
            };

            var result = systemUnderTest.Flatten(input);

            Assert.AreEqual(expected, result);
        }
        
        /// <summary>
        /// When the second snapshot has different columns, it should put a blank field in the missing column
        /// </summary>
        [Test]
        public void ReturnsFlatListFromMultiKeyMissingColumns()
        {
            var input = new Dictionary<string, List<SnapshotItem>>()
            {
                SingleKey("16/03/2022 10:00:00", new List<(string, decimal)>()
                {
                    ("Test1", 10M),
                    ("Test2", 20M),
                }),
                SingleKey("17/03/2022 10:00:00", new List<(string, decimal)>()
                {
                    ("Test2", 10M),
                }),
            };

            var expected = new List<string[]>()
            {
                { new string[] { "DateTime", "Test1", "Test2" } },
                { new string[] { "16/03/2022 10:00:00", "10", "20" } },
                { new string[] { "17/03/2022 10:00:00", null, "10" } }
            };

            var result = systemUnderTest.Flatten(input);

            Assert.AreEqual(expected, result);
        }
        
        /// <summary>
        /// When the third snapshot has a new column, it should put a blank field in the missing columns for previous
        /// </summary>
        [Test]
        public void ReturnsFlatListFromMultiKeyMissingColumnsExpertMode()
        {
            var input = new Dictionary<string, List<SnapshotItem>>()
            {
                SingleKey("16/03/2022 10:00:00", new List<(string, decimal)>()
                {
                    ("Test1", 10M),
                    ("Test2", 20M),
                }),
                SingleKey("17/03/2022 10:00:00", new List<(string, decimal)>()
                {
                    ("Test2", 10M),
                }),
                SingleKey("18/03/2022 10:00:00", new List<(string, decimal)>()
                {
                    ("Test2", 5M),
                    ("Test3", 22M),
                }),
            };

            var expected = new List<string[]>()
            {
                { new string[] { "DateTime", "Test1", "Test2", "Test3" } },
                { new string[] { "16/03/2022 10:00:00", "10", "20", null } },
                { new string[] { "17/03/2022 10:00:00", null, "10", null } },
                { new string[] { "18/03/2022 10:00:00", null, "5", "22" } }
            };

            var result = systemUnderTest.Flatten(input);

            Assert.AreEqual(expected, result);
        }
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
        
        private static KeyValuePair<string, List<SnapshotItem>> SingleKey(string dateTime, List<(string, decimal)> values)
        {
            var snapshotValues = new List<SnapshotItem>();
            
            foreach (var item in values)
            {
                snapshotValues.Add(new SnapshotItem()
                {
                    Description = item.Item1, Value = item.Item2
                });
            }

            return new KeyValuePair<string, List<SnapshotItem>>( dateTime, snapshotValues);
        }
    }
}