using System;
namespace DeliveryService.MessageContracts
{
    public static class RabbitMqConstants
    {
        //TODO: Move to env vars
        public const string RabbitMqUri = "amqp://rabbitmq:rabbitmq@rabbit:5672/";
        //public const string RabbitMqUri = "amqp://rabbitmq:rabbitmq@localhost:5672/";

        public const string JsonMimeType = "application/json";

        public const string RegisterOrderExchange = "registerorder.exchange";
        public const string RegisterOrderQueue = "registerorder.queue";

        public const string RegisteredExchange = "registered.exchange";
        public const string RegisteredNotificationQueue = "registered.notification.queue";
    }
}
