using System.Threading.Tasks;

namespace WebApi_OAuth_CallApp
{
    public interface IGraphClient
    {
        Task<string> GetUsers();
    }
}