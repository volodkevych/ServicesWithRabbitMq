using System;
using DeliveryService.MessageContracts;
namespace DeliveryService.Notification
{
    public class OrderRegisteredConsumer
    {
        public void Consume(IOrderRegisteredEvent registeredEvent)
        {
            //Send notification to user
            Console.WriteLine("Customer notification sent: Order id " +
                              $"{registeredEvent.OrderId} registered");
        }
    }
}
