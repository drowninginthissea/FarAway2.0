using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace FarAwayClient.Services
{
    public class Geocoder
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public Geocoder(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["YandexMaps:ApiKey"];
        }
        public async Task<(double Longitude, double Latitude)> GetCoordinatesAsync(string address)
        {
            string response = await _httpClient.GetStringAsync($"https://geocode-maps.yandex.ru/1.x/?apikey={_apiKey}&geocode={address}&format=json");
            JObject json = JObject.Parse(response);
            string[] positions = json["response"]["GeoObjectCollection"]["featureMember"][0]["GeoObject"]["Point"]["pos"].ToString().Split(' ');
            double longitude = double.Parse(positions[0], CultureInfo.InvariantCulture);
            double latitude = double.Parse(positions[1], CultureInfo.InvariantCulture);
            return (longitude, latitude);
        }
    }
}
