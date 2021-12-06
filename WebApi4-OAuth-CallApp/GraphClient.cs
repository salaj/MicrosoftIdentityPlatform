using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace WebApi4OAuthCallApp
{
    public class GraphClient : IGraphClient
    {
        private readonly HttpClient httpClient;
        private readonly IAccessTokenProvider tokenProvider;
        private readonly IConfiguration configuration;

        public GraphClient(HttpClient httpClient, IAccessTokenProvider tokenProvider, IConfiguration configuration)
        {
            this.httpClient = httpClient;
            this.tokenProvider = tokenProvider;
            this.configuration = configuration;
        }

        public async Task<string> GetUsers()
        {
            var token = await tokenProvider.GetAccessToken(configuration.GetValue<string>("AzureAd:ClientId"), new []{ "https://graph.microsoft.com/.default" });

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

            var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get,
                new Uri("https://graph.microsoft.com/v1.0/users")));

            return await response.Content.ReadAsStringAsync();
        }
    }
}
