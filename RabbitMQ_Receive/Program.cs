using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace RabbitMQ_Receive
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Receive running");

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (IConnection connection = factory.CreateConnection())
            using (IModel channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "news",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Received {0}", message);
                };
                channel.BasicConsume(queue: "news",autoAck:true,
                                     consumer: consumer);

                Console.ReadLine();
            }
        }

        class BasicNews
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Url { get; set; }
            public string Description { get; set; }
            public string Slug { get; set; }
            public string Category { get; set; }
        }
    }
}
