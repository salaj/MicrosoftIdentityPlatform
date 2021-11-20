using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WebApi_OAuth_CallApp
{
    public class GraphClient : IGraphClient
    {
        private readonly HttpClient httpClient;
        private readonly IAccessTokenProvider tokenProvider;

        public GraphClient(HttpClient httpClient, IAccessTokenProvider tokenProvider)
        {
            this.httpClient = httpClient;
            this.tokenProvider = tokenProvider;
        }

        public async Task<string> GetUsers()
        {
            var token = await tokenProvider.GetAccessToken(new []{ "https://graph.microsoft.com/.default" });

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

            var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get,
                new Uri("https://graph.microsoft.com/v1.0/users")));

            return await response.Content.ReadAsStringAsync();
        }
    }
}
