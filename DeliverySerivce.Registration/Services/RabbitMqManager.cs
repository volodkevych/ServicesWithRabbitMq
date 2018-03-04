using System;
using RabbitMQ.Client;
using DeliveryService.MessageContracts;
using System.Text;
using Newtonsoft.Json;
using DeliveryService.Registration.Messages;
namespace DeliveryService.Registration.Services
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

        public void ListenToRegisteredCommand()
        {
            channel.QueueDeclare(
                queue: RabbitMqConstants.RegisterOrderQueue,
                durable: false, exclusive: false,
                autoDelete: false, arguments: null);

            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var currentItem = 0;
            var batchSize = 5;
            while (currentItem++ < batchSize)
            {
                var result = channel.BasicGet(
                    queue: RabbitMqConstants.RegisterOrderQueue,
                    autoAck: false);

                if (result == null)
                    break;
                
                Consume(result);

                channel.BasicAck(
                    deliveryTag: result.DeliveryTag,
                    multiple: false);
            }
        }

        private void Consume(BasicGetResult result)
        {
            var properties = result.BasicProperties;
            if (properties.ContentType != RabbitMqConstants.JsonMimeType)
                throw new ArgumentException($"Can't handle content type {properties.ContentType}");

            var body = result.Body;
            var message = Encoding.UTF8.GetString(body);
            var command = JsonConvert.DeserializeObject<RegisterOrderCommand>(message);

            //TODO: Store order registration and get Id
            var id = 1;

            Console.WriteLine($"Registering order with id {id}");

            var orderRegisteredEvent = new OrderRegisteredEvent(command, id);
            SendOrderRegisteredEvent(orderRegisteredEvent);

            Console.WriteLine($"Order with id {id} registered");
        }

        public void SendOrderRegisteredEvent(IOrderRegisteredEvent orderRegisteredEvent)
        {
            channel.ExchangeDeclare(
                exchange: RabbitMqConstants.RegisteredExchange,
                type: ExchangeType.Fanout);
            channel.QueueDeclare(
                queue: RabbitMqConstants.RegisteredNotificationQueue,
                durable: false, exclusive: false,
                autoDelete: false, arguments: null);
            channel.QueueBind(
                queue: RabbitMqConstants.RegisteredNotificationQueue,
                exchange: RabbitMqConstants.RegisteredExchange,
                routingKey: "");

            var command = JsonConvert.SerializeObject(orderRegisteredEvent);

            var messageProperties = channel.CreateBasicProperties();
            messageProperties.ContentType = RabbitMqConstants.JsonMimeType;

            var body = Encoding.UTF8.GetBytes(command);

            channel.BasicPublish(
                exchange: RabbitMqConstants.RegisteredExchange,
                routingKey: "",
                basicProperties: messageProperties,
                body: body);
        }

        public void Dispose()
        {
            if (!channel.IsClosed)
                channel.Close();
        }
    }
}
