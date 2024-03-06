
using Microsoft.AspNetCore.Connections;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ReportService.API.Dtos.ReportDto;
using System.Text;
using System.Threading.Channels;

namespace ReportService.API
{
    public class QueueHostedService : BackgroundService
    {
        private readonly ILogger<QueueHostedService> logger;
        static IConnection connection;
        private static readonly string hotelsInformation = "hotels_info_queue";
        private static readonly string reportsInformation = "reports_info_queue";
        private static readonly string reportsCreateExchange = "reports_created_exchange_queue";
        static IModel _channel;
        static IModel channel => _channel ?? (_channel = GetChannel());
        public QueueHostedService(ILogger<QueueHostedService> logger)
        {

            this.logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000);
                connection = GetConnection();

                channel.ExchangeDeclare(reportsCreateExchange, "direct");
                channel.QueueDeclare(queue: hotelsInformation,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false);
                //Bind işlemleri
                channel.QueueBind(hotelsInformation, reportsCreateExchange, hotelsInformation);

                channel.QueueDeclare(queue: reportsInformation,
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false);
                //Bind işlemleri
                channel.QueueBind(reportsInformation, reportsCreateExchange, reportsInformation);
                var consumerEvent = new EventingBasicConsumer(channel);
                List<ReportListDto> dataList = new List<ReportListDto>();
                consumerEvent.Received += (ch, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    dataList = JsonConvert.DeserializeObject<List<ReportListDto>>(message);

                    //List<ReportListDto> modelList  = JsonConvert.DeserializeObject<List<ReportListDto>>(Encoding.UTF8.GetString(ea.Body.ToArray()));



                    foreach (var data in dataList)
                    {
                        Console.WriteLine("Received data: {0}", data);
                    }

                    //modelJson = "selamiçeşme şubesi";
                    //WriteToQueue(reportsInformation, modelJson);
                };

                channel.BasicConsume(hotelsInformation, true, consumerEvent);

            }
        }

        private static IModel? GetChannel()
        {
            return connection.CreateModel();
        }
        private static IConnection GetConnection()
        {
            var connectionFactory = new ConnectionFactory()
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672/")
            };

            return connectionFactory.CreateConnection();
        }
        private void WriteToQueue(string queueName, object data)
        {
            var messageArr = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
            channel.BasicPublish(reportsCreateExchange, queueName, null, messageArr);
        }
    }
}
