using System;
using DeliveryService.MessageContracts;
namespace DeliveryService.Registration.Web.Services
{
    public interface IRabbitMqManager
    {
        void SendRegisterOrderCommand(IRegisterOrderCommand registerOrderCommand);
    }
}
