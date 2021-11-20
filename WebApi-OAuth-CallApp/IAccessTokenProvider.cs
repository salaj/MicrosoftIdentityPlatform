using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi_OAuth_CallApp
{
    public interface IAccessTokenProvider
    {
        Task<string> GetAccessToken(IEnumerable<string> scopes);
    }
}