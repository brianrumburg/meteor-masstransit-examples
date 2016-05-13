
using System;

namespace PingMassTransit
{
    public interface IPing
    {
        string SomeString { get; set; }
        int SomeInteger { get; set; }
        float SomeDecimal { get; set; }
        DateTime SomeDate { get; set; }
    }

    public class Ping : IPing
    {
        public string PingField { get; set; }
        public string SomeString { get; set; }
        public int SomeInteger { get; set; }
        public float SomeDecimal { get; set; }
        public DateTime SomeDate { get; set; }
    }

   
}
