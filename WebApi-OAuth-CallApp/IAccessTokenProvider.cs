using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApiOAuthCallApp
{
    public interface IAccessTokenProvider
    {
        Task<string> GetAccessToken(IEnumerable<string> scopes);
    }
}