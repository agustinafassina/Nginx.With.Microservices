using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiTwo.Controllers
{
    [ApiController]
    [Route("api/v2/[controller]")]
    public class CountriesController : ControllerBase
    {
        private const string SourceExternalService = "https://restcountries.com/v3.1/all?fields=name,flags";
        private readonly IHttpClientFactory _httpClientFactory;

        public CountriesController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var client = _httpClientFactory.CreateClient();

            try
            {
                using var response = await client.GetAsync(SourceExternalService);
                var body = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode(StatusCodes.Status502BadGateway,
                        new { error = "External service returned an error", status = (int)response.StatusCode });
                }

                var contentType = response.Content.Headers.ContentType?.ToString() ?? "application/json";
                return Content(body, contentType);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(StatusCodes.Status502BadGateway,
                    new { error = "External service unavailable", detail = ex.Message });
            }
        }
    }
}
