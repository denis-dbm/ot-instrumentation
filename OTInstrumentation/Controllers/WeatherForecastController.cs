using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OTInstrumentation.Models;
using OTInstrumentation.Repositories;
using OTInstrumentation.Services;
using System;
using System.Threading.Tasks;

namespace OTInstrumentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly WeatherForecastRepository _weatherForecastRepository;
        private readonly WeatherStackService _weatherStackService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, WeatherForecastRepository weatherForecastRepository, WeatherStackService weatherStackService)
        {
            _logger = logger;
            _weatherForecastRepository = weatherForecastRepository;
            _weatherStackService = weatherStackService;
        }

        [HttpGet]
        public IActionResult Get([FromQuery]DateTime instant)
        {
            var weatherForecast = _weatherForecastRepository.GetByInstant(instant);

            if (weatherForecast == null)
            {
                _logger.LogInformation("Forecast record not found at instant {0}", instant);
                return NotFound();
            }
            
            _logger.LogInformation("Forecast record found at instant {0}", instant);

            return Ok(weatherForecast);
        }

        [HttpGet("external-provider")]
        public async Task<IActionResult> GetFromForecastExternalProvider([FromQuery]string locationName)
        {
            return Ok(await _weatherStackService.GetTemperatureByLocation(locationName));
        }

        [HttpPost]
        public IActionResult Post([FromBody]WeatherForecast weatherForecast)
        {
            IActionResult response = _weatherForecastRepository.Add(weatherForecast) switch
            {
                true => CreatedAtAction(nameof(Get), new { instant = weatherForecast.Date }),
                _ => Conflict(new { 
                    Message = $"There is a forecast at instant {weatherForecast.Date}"
                })
            };
            
            _logger.LogInformation("Persistence operation returned {0}", response);

            return response;
        }
    }
}
