using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Graph;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using WebApp_OpenIDConnect_DelegateUserToCallApp.Models;

namespace WebApp_OpenIDConnect_DelegateUserToCallApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly GraphServiceClient _graphServiceClient;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger,
                          GraphServiceClient graphServiceClient, IConfiguration configuration)
        {
             _logger = logger;
            _graphServiceClient = graphServiceClient;
            _configuration = configuration;
        }


        [AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
        [HttpPost]
        public async Task<ActionResult> Send(MessageModel model)
        {
            if (ModelState.IsValid)
            {
                var chatMessage = new ChatMessage
                {
                    Body = new ItemBody
                    {
                        ContentType = BodyType.Html,
                        Content = model.Message
                    }
                };
                await _graphServiceClient.Teams[_configuration.GetValue<string>("TeamId")].Channels[_configuration.GetValue<string>("ChannelId")].Messages[_configuration.GetValue<string>("MessageId")].Replies
                    .Request()
                    .AddAsync(chatMessage);

            }

            return View("Index", model);
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
