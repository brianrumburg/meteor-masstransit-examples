using System;
using MassTransit;

namespace PingMassTransit
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceBusFactory.New(sbc =>
            {
                sbc.UseRabbitMq();
                sbc.ReceiveFrom("rabbitmq://localhost/ping-masstransit");
                sbc.Subscribe(subs =>
                {
                    subs.Handler<Ping>((ctx, msg) =>
                    {
                        Console.WriteLine("Ping received!");
                        Console.WriteLine(msg.SomeString);
                        Console.WriteLine(msg.SomeInteger);
                        Console.WriteLine(msg.SomeDecimal);
                        Console.WriteLine("{0:O}", msg.SomeDate);

                        ctx.Respond(new Pong()
                        {
                            SomeString = msg.SomeString,
                            SomeInteger = msg.SomeInteger,
                            SomeDecimal = msg.SomeDecimal,
                            SomeDate = msg.SomeDate
                        });
                    });
                });

                Console.WriteLine("Waiting for Ping...");
                Console.WriteLine("Press Ctrl+C to exit.");
            });
        }
    }
}
