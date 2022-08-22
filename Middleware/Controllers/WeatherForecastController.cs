using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Docs.Samples;
using Microsoft.Extensions.Logging;
using Middleware.Attributes;
using Middleware.Filters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Middleware.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : Controller
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy",    "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }
        [HttpGet()]
        //[IgnoreStatusAttribute]
        //[ServiceFilter(typeof(LoggingFilter))]
        public IEnumerable<WeatherForecast> Get()
        {
            // new AccessViolationException("Violation Exception while accessing the resource.");
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("testFitler")]
        [SampleActionFilter()]
        [HasPermissionFilter("Staff")]

        public string FilterTest()
        {
            _logger.LogInformation($" Endpoint - {MethodBase.GetCurrentMethod()}");
            return $" Endpoint - {MethodBase.GetCurrentMethod()}";
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation($" Controller - {MethodBase.GetCurrentMethod()}");
            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation($" Controller - {MethodBase.GetCurrentMethod()}");
            base.OnActionExecuted(context);
        }

        [HttpGet("testPermission")]

        public string TestHasPermission()
        {
            _logger.LogInformation($" Endpoint - {MethodBase.GetCurrentMethod()}");
            return $" Endpoint - {MethodBase.GetCurrentMethod()}";
        }
    }
}
