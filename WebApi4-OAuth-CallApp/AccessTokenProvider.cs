using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;

namespace WebApi4OAuthCallApp
{
    public class AccessTokenProvider : IAccessTokenProvider
    {
        private readonly IEnumerable<IConfidentialClientApplication> confidentialClientApplications;
        private readonly ILogger<AccessTokenProvider> logger;

        public AccessTokenProvider(IEnumerable<IConfidentialClientApplication> confidentialClientApplications,
            ILogger<AccessTokenProvider> logger)
        {
            this.confidentialClientApplications = confidentialClientApplications;
            this.logger = logger;
        }

        public async Task<string> GetAccessToken(string fromClientId, IEnumerable<string> scopes)
        {
            AuthenticationResult result = null;
            try
            {
                result = await confidentialClientApplications.First(x => x.AppConfig.ClientId == fromClientId).AcquireTokenForClient(scopes)
                    .ExecuteAsync();
            }
            catch (MsalServiceException e)
            {
                // AADSTS70011
                // Invalid scope. The scope has to be of the form "https://resourceurl/.default"
                // Mitigation: this is a dev issue. Change the scope to be as expected
                logger.LogError($"{e.GetType().Name} occurred with message {e.Message}");
                throw;
            }

            return result.AccessToken;
        }
    }
}
