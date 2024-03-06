using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ReportService.API.Dtos.ReportDto;
using System.Text;

namespace ReportService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        IConnection connection;
        private readonly string gettingReady = "getting_ready_queue";
        private readonly string completed = "completed_queue";
        private readonly string reportCreatedExchange = "report_created_exchange_queue";
        IModel _channel;
        IModel channel=>_channel??(_channel=GetChannel());



        [HttpGet("setreports")]
        public async Task<IActionResult> SetReports()
        {
            if (connection == null || !connection.IsOpen)
                connection = GetConnection();

            // Kuyruk adı
            var queueName = "hotelsWithContactsCount";

            // Kuyruğu belirle
            channel.QueueDeclare(queue: queueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            // TaskCompletionSource kullanarak işlem tamamlandığında sinyal ver
            var taskCompletionSource = new TaskCompletionSource<List<ReportListDto>>();

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                var reportListDto = JsonConvert.DeserializeObject<List<ReportListDto>>(json);

                // Mesaj işlendikten sonra taskCompletionSource'e sonuç olarak rapor listesini ayarla
                taskCompletionSource.SetResult(reportListDto);

                // Mesaj işlendikten sonra onayla (acknowledge)
                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            channel.BasicConsume(queue: queueName,
                                 autoAck: false,
                                 consumer: consumer);

            // taskCompletionSource'in sonucunu bekleyin
            var result = await taskCompletionSource.Task;

            // Sonucu döndür
            return Ok(result);
        }


        private async Task<ActionResult> WriteToQueue(string queueName, ReportListDto reportListDto)
        {
            var messageArr=Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(reportListDto));

            channel.BasicPublish(reportCreatedExchange, queueName, null, messageArr);

            return Ok();

        }
        private IModel? GetChannel()
        {
            return connection.CreateModel();
        }
        private IConnection GetConnection()
        {
            var connectionFactory = new ConnectionFactory()
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672/")
            };

            return connectionFactory.CreateConnection();
        }
    }
}
