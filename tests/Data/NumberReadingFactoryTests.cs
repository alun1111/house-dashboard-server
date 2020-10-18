using System;
using System.Collections.Generic;
using System.Linq;
using house_dashboard_server.Data;
using house_dashboard_server.Data.Interfaces;
using NUnit.Framework;

namespace tests.Data
{
    public class NumberReadingFactoryTests
    {
        private ReadingFactory systemUnderTest;

        [SetUp]
        public void Setup()
        {
            this.systemUnderTest = new ReadingFactory();
        }

        [Test]
        public void ReturnsOneReading()
        {
            string measurementName = "Inside Temperature";

            var singleResult 
                = new Measurement<decimal>(
                    new DateTime(2020, 01, 01)
                    , 1234567
                    , 45);
            
            var reducedScanResult = new List<IMeasurement<decimal>>()
            {
                singleResult
            };
            
            var result 
                = ReadingFactory.BuildReading(measurementName, reducedScanResult);
        
            Assert.AreEqual(singleResult, result.Current);
            Assert.AreEqual(measurementName, result.Name);
            Assert.AreEqual(0, result.Recent.Count);
        }
        
        [Test]
        public void ReturnsTwoReadingsInDateTimeOrder()
        {
            string measurementName = "Inside Temperature";

            var yesterday 
                = new Measurement<decimal>(
                    new DateTime(2020, 01, 01)
                    ,1577836800 
                    , 45);
            
            var today 
                = new Measurement<decimal>(
                    new DateTime(2020, 01, 02)
                    ,1577923200 
                    , 46);
            
            var reducedScanResult = new List<IMeasurement<decimal>>()
            {
                today,
                yesterday
            };
            
            var result 
                = ReadingFactory.BuildReading(measurementName, reducedScanResult);

            var recentResult = result.Recent.First();
        
            Assert.AreEqual(today, result.Current);
            Assert.AreEqual(yesterday.Value, recentResult.Value);
            Assert.AreEqual(yesterday.MeasurementTime, recentResult.MeasurementTime);
            Assert.AreEqual(yesterday.TimeIndex, recentResult.TimeIndex);
            Assert.AreEqual(measurementName, result.Name);
        }
        
    }
}