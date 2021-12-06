using System.Threading.Tasks;

namespace WebApi4OAuthCallApp
{
    public interface IGraphClient
    {
        Task<string> GetUsers();
    }
}