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
        private readonly IHttpClientFactory _clientFactory;
        static readonly string _url = "location/650272/";

        public HttpClientController(IHttpClientFactory clientFactory) { 
            _clientFactory = clientFactory;
        }

        [HttpGet]
        public async Task<WeatherForecast> Get()
        {
            try
            {
                var client = _clientFactory.CreateClient("meta");
                return await client.GetFromJsonAsync<WeatherForecast>(_url);
            } 
            catch (System.Exception ex)
            {
                throw new Exception($"Something went wrong. Error: {ex.Message}");
            }
        }
    }
}
