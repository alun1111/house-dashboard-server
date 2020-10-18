using System;
using house_dashboard_server.Data.Interfaces;

namespace house_dashboard_server.Data
{
    /// <summary>
    /// A measurement is a a value (T) at a point of time
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Measurement<T> : IMeasurement<T>
    {
        public Measurement(DateTime measurementTime, long timeIndex, T value)
        {
            MeasurementTime = measurementTime;
            TimeIndex = timeIndex;
            Value = value;
        }

        public DateTime MeasurementTime { get; }
        
        public long TimeIndex { get; }
        
        public T Value { get; }
    }
}
