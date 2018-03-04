using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DeliveryService.Registration.Web.Messages;
using DeliveryService.Registration.Web.Services;
using DeliveryService.Registration.Web.ViewModels;

namespace DeliveryService.Registration.Web.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IRabbitMqManager rabbitMqManager;

        public RegisterController(IRabbitMqManager rabbitMqManager)
        {
            this.rabbitMqManager = rabbitMqManager;
        }

        public IActionResult RegisterOrder()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterOrder(OrderViewModel model)
        {
            var registerOrderCommand = new RegisterOrderCommand(model);

            rabbitMqManager.SendRegisterOrderCommand(registerOrderCommand);

            return View("Thanks");
        }
    }
}
