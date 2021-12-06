using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi4OAuthCallApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TokenAcquisitionController : ControllerBase
    {
        private readonly IAccessTokenProvider tokenProvider;
        private readonly IConfiguration configuration;

        public TokenAcquisitionController(IAccessTokenProvider tokenProvider, IConfiguration configuration)
        {
            this.tokenProvider = tokenProvider;
            this.configuration = configuration;
        }

        [HttpGet]
        [SwaggerOperation(Summary = nameof(GetToken), Description = "Method to retrieve token using client grant credentials flow.")]
        public async Task<IActionResult> GetToken()
        {
            var token = await tokenProvider.GetAccessToken(configuration.GetValue<string>("SwaggerUIClientId"), new[] { $"api://{configuration["AzureAd:ClientId"]}/.default" });
            return Ok(token);
        }
    }
}
