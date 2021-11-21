using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi3OAuthCallApp.Controllers
{
    [Authorize]
    [RequiredScope("access_as_user")]
    [ApiController]
    [Route("[controller]")]
    public class ActiveDirectoryController : ControllerBase
    {
        private readonly GraphServiceClient _graphServiceClient;

        public ActiveDirectoryController(GraphServiceClient graphServiceClient)
        {
            _graphServiceClient = graphServiceClient;
        }

        [HttpGet]
        [Route("users")]
        [SwaggerOperation(Summary = nameof(GetUsers), Description = "Method to retrieve users' names and emails from jksa-test-tenant")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _graphServiceClient.Users.Request().GetAsync();

            var enumerable = users.Select(x => new User()
            {
                Email = x.Mail,
                Name = x.DisplayName
            });

            return Ok(enumerable);
            //return Ok();
        }
    }
}
