using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DotNetHttpClient.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HttpClientController : ControllerBase
    {
        static readonly HttpClient _client = new HttpClient();
        static readonly string _url = "https://www.metaweather.com/api/location/search/?query=frankfurt";

        public HttpClientController() { }

        [HttpGet]
        public async Task<string> Get()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _url);
            var response = await _client.SendAsync(request);

            if(response.IsSuccessStatusCode)
                return await response.Content.ReadAsStringAsync();
            else
                return "Some kind of error happaned...";
        }
    }
}
