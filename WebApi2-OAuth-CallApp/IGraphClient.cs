using System.Threading.Tasks;

namespace WebApi2OAuthCallApp
{
    public interface IGraphClient
    {
        Task<string> GetUsers(string token);
    }
}