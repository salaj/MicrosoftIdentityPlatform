using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi_OAuth_CallApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ActiveDirectoryController : ControllerBase
    {
        private readonly IGraphClient graphClient;

        public ActiveDirectoryController(IGraphClient graphClient)
        {
             this.graphClient = graphClient;
        }

        [HttpPost]
        [Route("users")]
        [SwaggerOperation(Summary = nameof(GetUsers), Description = "Method to retrieve users' names and emails from jksa-test-tenant")]
        public async Task<IActionResult> GetUsers()
        {
            var usersJson = JObject.Parse(await graphClient.GetUsers());
            var users = JsonConvert.DeserializeObject<List<User>>(usersJson["value"].ToString(), new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });

            return Ok(users);
        }
    }
}
