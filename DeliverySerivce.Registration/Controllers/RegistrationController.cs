using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DeliveryService.Registration.Services;
namespace DeliveryService.Registration.Controllers
{
    [Route("api/[controller]")]
    public class RegistrationController : Controller
    {
        private readonly IRabbitMqManager rabbitMqManager;

        public RegistrationController(IRabbitMqManager rabbitMqManager)
        {
            this.rabbitMqManager = rabbitMqManager;
        }

        [HttpGet]
        public string Get()
        {
            rabbitMqManager.ListenToRegisteredCommand();

            return "ListenToRegisteredCommand finished";
        }
    }
}
