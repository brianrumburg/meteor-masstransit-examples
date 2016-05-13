using System;

namespace PingMassTransit
{
    internal interface IPong
    {
        String SomeString { get; set; }
        int SomeInteger { get; set; }
        float SomeDecimal { get; set; }
        DateTime SomeDate { get; set; }
    }

    public class Pong : IPong
    {
        public string PongField { get; set; }
        public String SomeString { get; set; }
        public int SomeInteger { get; set; }
        public float SomeDecimal { get; set; }
        public DateTime SomeDate { get; set; }
    }

    public class Pong2 : IPong
    {
        public string Pong2Field { get; set; }
        public String SomeString { get; set; }
        public int SomeInteger { get; set; }
        public float SomeDecimal { get; set; }
        public DateTime SomeDate { get; set; }
    }

}
