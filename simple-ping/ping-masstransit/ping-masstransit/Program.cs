using System;
using MassTransit;
using System.Threading.Tasks;

namespace PingMassTransit
{
    class Program
    {
        static void Main(string[] args)
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(new Uri("rabbitmq://rabbitmq-test"), host => { });
                cfg.ReceiveEndpoint("ping-server-queue", e => {
                    e.Handler<IPing>(ctx => Task.Run(() => {
                        var msg = ctx.Message;
                        Console.WriteLine("IPing received!");
                        Console.WriteLine(msg.SomeString);
                        Console.WriteLine(msg.SomeInteger);
                        Console.WriteLine(msg.SomeDecimal);
                        Console.WriteLine("{0:O}", msg.SomeDate);

                        ctx.Respond(new Pong()
                        {
                            PongField = "From IPing Handler",
                            SomeString = msg.SomeString,
                            SomeInteger = msg.SomeInteger,
                            SomeDecimal = msg.SomeDecimal,
                            SomeDate = msg.SomeDate
                        });

                        ctx.Respond(new Pong2()
                        {
                            Pong2Field = "From IPing Handler",
                            SomeString = msg.SomeString,
                            SomeInteger = msg.SomeInteger,
                            SomeDecimal = msg.SomeDecimal,
                            SomeDate = msg.SomeDate
                        });
                    }));

                    e.Handler<Ping>(ctx => Task.Run(() => {
                        var msg = ctx.Message;
                        Console.WriteLine("Ping received!");
                        Console.WriteLine(msg.SomeString);
                        Console.WriteLine(msg.SomeInteger);
                        Console.WriteLine(msg.SomeDecimal);
                        Console.WriteLine("{0:O}", msg.SomeDate);
                        Console.WriteLine(msg.PingField);

                        ctx.Respond(new Pong()
                        {
                            PongField = "From Ping Handler",
                            SomeString = msg.SomeString,
                            SomeInteger = msg.SomeInteger,
                            SomeDecimal = msg.SomeDecimal,
                            SomeDate = msg.SomeDate
                        });
                    }));

                    //e.Handler<Pong>(ctx => Task.Run(() => {
                    //    var msg = ctx.Message;
                    //    Console.WriteLine("Pong received!");
                    //    Console.WriteLine(msg.SomeString);
                    //    Console.WriteLine(msg.SomeInteger);
                    //    Console.WriteLine(msg.SomeDecimal);
                    //    Console.WriteLine("{0:O}", msg.SomeDate);
                    //}));
                });
            });

            busControl.Start();

            //busControl.Publish(new Ping()
            //{
            //    SomeString = "111",
            //    SomeInteger = 222,
            //    SomeDecimal = (float)33.44,
            //    SomeDate = DateTime.Now,
            //    PingField = "PING"
            //});

            Console.WriteLine("Waiting for Ping...");
            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
            busControl.Stop();
        }
    }
}
