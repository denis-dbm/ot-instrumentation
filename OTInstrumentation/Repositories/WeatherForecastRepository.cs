using OTInstrumentation.Models;
using System;
using System.Collections.Concurrent;

namespace OTInstrumentation.Repositories
{
    public class WeatherForecastRepository
    {
        private static readonly ConcurrentDictionary<DateTime, WeatherForecast> _weatherForecasts = new ConcurrentDictionary<DateTime, WeatherForecast>();

        public WeatherForecast GetByInstant(DateTime instant)
        {
            _weatherForecasts.TryGetValue(instant, out var weatherForecast);
            return weatherForecast;
        }

        public bool Add(WeatherForecast weatherForecast)
        {
            if (weatherForecast == null)
                throw new ArgumentNullException(nameof(weatherForecast));

            return _weatherForecasts.TryAdd(weatherForecast.Date, weatherForecast);
        }
    }
}
