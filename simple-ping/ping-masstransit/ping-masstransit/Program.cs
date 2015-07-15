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
                        ctx.Respond(new Pong() { DateTime = DateTime.Now });
                    });
                });

                Console.WriteLine("Waiting for Ping...");
                Console.WriteLine("Press Ctrl+C to exit.");
            });
        }
    }
}
