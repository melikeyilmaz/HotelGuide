using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ReportService.API.Dtos.ReportDto;
using ReportService.Data.Context;
using ReportService.Data.Entities;
using System.Text;

namespace ReportService.API
{
    public class QueueHostedService : BackgroundService
    {
        private readonly ILogger<QueueHostedService> logger;
        private readonly IServiceProvider _serviceProvider;

        static IConnection connection;
        private static readonly string hotelsInformation = "hotels_info_queue";
        private static readonly string reportsInformation = "reports_info_queue";
        private static readonly string reportsCreateExchange = "reports_created_exchange_queue";
        static IModel _channel;
        static IModel channel => _channel ?? (_channel = GetChannel());
        public QueueHostedService(ILogger<QueueHostedService> logger, IServiceProvider serviceProvider)
        {

            this.logger = logger;
            _serviceProvider = serviceProvider;
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
                consumerEvent.Received += async (ch, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    List<ReportListDto> dataList;

                    dataList = JsonConvert.DeserializeObject<List<ReportListDto>>(message);
                    List<Report> reportsToAddToQueue = new List<Report>();
                    using var scope = _serviceProvider.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<ReportContext>();
                    foreach (var data in dataList)
                    {
                        var report = new Report
                        {
                            CreatedDate = DateTime.UtcNow,
                            HotelCount = data.HotelCount,
                            Location = data.Location,
                            ContactCount = data.ContactCount,
                            Status = "Tamamlandı"

                        };



                        // DbContext'i kapsamdan al


                        dbContext.Reports.Add(report);
                        reportsToAddToQueue.Add(report);

                    }
                    await dbContext.SaveChangesAsync();
                    WriteToQueue(reportsInformation, reportsToAddToQueue);
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
