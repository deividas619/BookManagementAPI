using Microsoft.AspNetCore.Mvc;

namespace APICaller.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CallerController : ControllerBase
    {
        private readonly ILogger<CallerController> _logger;

        public CallerController(ILogger<CallerController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetBook")]
        public async Task<string> Get([FromQuery(Name = "Title")] string title)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("ApiKey", "a4bf5258-2a3d-4e24-9168-5e10611bbd26");
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://localhost:44338/Book/GetBook?title={title}")
            };

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();

            return body;
        }
    }
}
