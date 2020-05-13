using System;
using System.Collections.Generic;
using System.Linq;
using HouseDashboardServer.Data;
using NUnit.Framework;

namespace tests.Data
{
    public class NumberReadingFactoryTests
    {
        private NumberReadingFactory systemUnderTest;

        [SetUp]
        public void Setup()
        {
            this.systemUnderTest = new NumberReadingFactory();
        }

        [Test]
        public void ReturnsOneReading()
        {
            string measurementName = "Inside Temperature";

            var singleResult 
                = new DynamoDbItem<decimal>(
                    new DateTime(2020, 01, 01)
                    , 1234567
                    , 45);
            
            var reducedScanResult = new List<DynamoDbItem<decimal>>()
            {
                singleResult
            };
            
            var result 
                = this.systemUnderTest.BuildReading(measurementName, reducedScanResult);
        
            Assert.AreEqual(singleResult, result.Current);
            Assert.AreEqual(measurementName, result.Name);
            Assert.AreEqual(0, result.Recent.Count);
        }
        
        [Test]
        public void ReturnsTwoReadingsInDateTimeOrder()
        {
            string measurementName = "Inside Temperature";

            var yesterday 
                = new DynamoDbItem<decimal>(
                    new DateTime(2020, 01, 01)
                    ,1577836800 
                    , 45);
            
            var today 
                = new DynamoDbItem<decimal>(
                    new DateTime(2020, 01, 02)
                    ,1577923200 
                    , 46);
            
            var reducedScanResult = new List<DynamoDbItem<decimal>>()
            {
                today,
                yesterday
            };
            
            var result 
                = this.systemUnderTest.BuildReading(measurementName, reducedScanResult);

            var recentResult = result.Recent.First();
        
            Assert.AreEqual(today, result.Current);
            Assert.AreEqual(yesterday.Value, recentResult.Value);
            Assert.AreEqual(yesterday.MeasurementTime, recentResult.MeasurementTime);
            Assert.AreEqual(yesterday.TimeIndex, recentResult.TimeIndex);
            Assert.AreEqual(measurementName, result.Name);
        }
        
    }
}