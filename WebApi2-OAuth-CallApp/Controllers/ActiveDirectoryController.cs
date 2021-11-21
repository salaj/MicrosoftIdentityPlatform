using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi2OAuthCallApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ActiveDirectoryController : ControllerBase
    {
        private readonly IGraphClient _graphServiceClient;

        public ActiveDirectoryController(IGraphClient graphServiceClient)
        {
            _graphServiceClient = graphServiceClient;
        }

        [HttpGet]
        [Route("users")]
        [SwaggerOperation(Summary = nameof(GetUsers), Description = "Method to retrieve users' names and emails from jksa-test-tenant")]
        public async Task<IActionResult> GetUsers()
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var usersJson = JObject.Parse(await _graphServiceClient.GetUsers(token));
            var users = JsonConvert.DeserializeObject<List<User>>(usersJson["value"].ToString(), new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });

            return Ok(users);
        }
    }
}
