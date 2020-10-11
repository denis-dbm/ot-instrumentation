using System.Net.Http;
using System.Threading.Tasks;
using static System.String;

namespace OTInstrumentation.Services
{
    public class WeatherStackService
    {
        private readonly string _weatherStackServiceUri;
        private readonly HttpClient _httpClient;

        public WeatherStackService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _weatherStackServiceUri = "http://api.weatherstack.com/current?access_key=&query={0}";
        }

        public async Task<string> GetTemperatureByLocation(string locationName)
        {
            var response = await _httpClient.GetAsync(Format(_weatherStackServiceUri, locationName));

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadAsStringAsync();
        }
    }
}
