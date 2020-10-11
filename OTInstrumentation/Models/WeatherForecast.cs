using System;
using System.ComponentModel.DataAnnotations;

namespace OTInstrumentation.Models
{
    public class WeatherForecast
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}
