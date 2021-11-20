using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;

namespace WebApi_OAuth_CallApp
{
    public class AccessTokenProvider : IAccessTokenProvider
    {
        private readonly IConfidentialClientApplication confidentialClientApplication;
        private readonly ILogger<AccessTokenProvider> logger;

        public AccessTokenProvider(IConfidentialClientApplication confidentialClientApplication,
            ILogger<AccessTokenProvider> logger)
        {
            this.confidentialClientApplication = confidentialClientApplication;
            this.logger = logger;
        }

        public async Task<string> GetAccessToken(IEnumerable<string> scopes)
        {
            AuthenticationResult result = null;
            try
            {
                result = await confidentialClientApplication.AcquireTokenForClient(scopes)
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
