using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WebApi2OAuthCallApp
{
    public class GraphClient : IGraphClient
    {
        private readonly HttpClient httpClient;
        public GraphClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<string> GetUsers(string token)
        {

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

            var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get,
                new Uri("https://graph.microsoft.com/v1.0/users")));

            return await response.Content.ReadAsStringAsync();
        }
    }
}
