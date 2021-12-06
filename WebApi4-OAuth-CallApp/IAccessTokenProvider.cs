using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi4OAuthCallApp
{
    public interface IAccessTokenProvider
    {
        Task<string> GetAccessToken(string fromClientId, IEnumerable<string> scopes);
    }
}