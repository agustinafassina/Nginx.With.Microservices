using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiOne.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PetsController : ControllerBase
    {
        private const string SourceExternalService = "https://dog.ceo/api/breeds/image/random/3";
        private readonly IHttpClientFactory _httpClientFactory;

        public PetsController(IHttpClientFactory httpClientFactory)
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
