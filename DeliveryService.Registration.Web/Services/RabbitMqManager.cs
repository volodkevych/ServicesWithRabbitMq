using System;
using DeliveryService.MessageContracts;
using RabbitMQ.Client;
using Newtonsoft.Json;
using System.Text;

namespace DeliveryService.Registration.Web.Services
{
    public class RabbitMqManager : IDisposable, IRabbitMqManager
    {
        private readonly IModel channel;

        public RabbitMqManager()
        {
            var connectionFactory = new ConnectionFactory
            {
                Uri = new Uri(RabbitMqConstants.RabbitMqUri)
            };
            var connection = connectionFactory.CreateConnection();
            channel = connection.CreateModel();
            connection.AutoClose = true;
        }

        public void SendRegisterOrderCommand(IRegisterOrderCommand registerOrderCommand)
        {
            channel.ExchangeDeclare(
                exchange: RabbitMqConstants.RegisterOrderExchange,
                type: ExchangeType.Direct);
            channel.QueueDeclare(
                queue: RabbitMqConstants.RegisterOrderQueue,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            channel.QueueBind(
                queue: RabbitMqConstants.RegisterOrderQueue,
                exchange: RabbitMqConstants.RegisterOrderExchange,
                routingKey: string.Empty);

            var command = JsonConvert.SerializeObject(registerOrderCommand);

            var messageProps = channel.CreateBasicProperties();
            messageProps.ContentType = RabbitMqConstants.JsonMimeType;

            var body = Encoding.UTF8.GetBytes(command);

            channel.BasicPublish(
                exchange: RabbitMqConstants.RegisterOrderExchange,
                routingKey: string.Empty,
                basicProperties: messageProps,
                body: body);
        }

        public void Dispose()
        {
            if (!channel.IsClosed)
                channel.Close();
        }
    }
}
