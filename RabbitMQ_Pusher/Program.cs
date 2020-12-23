using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;

namespace RabbitMQ_Pusher
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Pusher Running");
            BasicNews news = new BasicNews
            {
                Title = "Meteoroloji'den yoğun kar uyarısı",
                Category = "TÜRKİYE",
                Description = "Doğu Karadeniz'in kıyı illerinde bugün gök gürültülü sağanak, yüksek kesimlerinde yoğun kar yağışı bekleniyor",
                Slug = "meteorolojiden-yogun-kar-uyarisi-2294103",
                Url = "/meteorolojiden-yogun-kar-uyarisi-2294103",
                Id = 2294103
            };


            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            using (IConnection connection = factory.CreateConnection())
            using (IModel channel = connection.CreateModel())
            {

                channel.QueueDeclare(queue: $"news",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

                string message = JsonConvert.SerializeObject(news);
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "",
                             routingKey: $"news",
                             basicProperties: null,
                             body: body);
            }

            Console.WriteLine("İlgili haber gönderildi");
            Console.ReadLine();

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
