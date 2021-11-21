using System.Threading.Tasks;

namespace WebApiOAuthCallApp
{
    public interface IGraphClient
    {
        Task<string> GetUsers();
    }
}