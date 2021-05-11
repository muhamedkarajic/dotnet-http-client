using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DotNetHttpClient.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HttpClientController : ControllerBase
    {
        static readonly HttpClient _client = new HttpClient();
        static readonly string _url = "https://www.metaweather.com/api/location/650272/";

        public HttpClientController() { }

        [HttpGet]
        public async Task<WeatherForecast> Get()
        {
            try
            {
                return await _client.GetFromJsonAsync<WeatherForecast>(_url);
            }
            catch (System.Exception ex)
            {
                throw new Exception($"Something went wrong. Error: {ex.Message}");
            }
        }
    }
}
