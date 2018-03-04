using System;
namespace DeliveryService.MessageContracts
{
    public interface IOrderRegisteredEvent : IRegisterOrderCommand
    {
        int OrderId { get; }
    }
}
