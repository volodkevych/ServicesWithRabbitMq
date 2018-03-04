using System;
namespace DeliveryService.Registration.Services
{
    public interface IRabbitMqManager
    {
        void ListenToRegisteredCommand();
    }
}
