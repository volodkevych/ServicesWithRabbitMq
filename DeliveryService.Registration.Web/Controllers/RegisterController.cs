using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DeliveryService.Registration.Web.Messages;
using DeliveryService.Registration.Web.ViewModels;

namespace DeliveryService.Registration.Web.Controllers
{
    public class RegisterController : Controller
    {
        public IActionResult RegisterOrder()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterOrder(OrderViewModel model)
        {
            var registerOrderCommand = new RegisterOrderCommand(model);

           //Send RegisterOrderCommand
            using (var rabbitMqManager = new RabbitMqManager())
            {
                rabbitMqManager.SendRegisterOrderCommand(registerOrderCommand);
            }

            return View("Thanks");
        }
    }
}
