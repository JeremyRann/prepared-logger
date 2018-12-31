using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;

namespace PreparedLogger.AdminConsole
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            // discover endpoints from metadata
            var disco = await DiscoveryClient.GetAsync("http://localhost:5001");
            if (disco.IsError)
            {
                Console.WriteLine($"Discovery client error:{Environment.NewLine + disco.Error}");
                return;
            }

            // request token
            // var tokenClient = new TokenClient(disco.TokenEndpoint, "AdminConsole", "secret");
            // var tokenResponse = await tokenClient.RequestClientCredentialsAsync("PreparedLogger");
            var tokenClient = new TokenClient(disco.TokenEndpoint, "ro.client", "secret");
            var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync("alice", "password", "PreparedLogger");


            if (tokenResponse.IsError)
            {
                Console.WriteLine($"Token client error:{Environment.NewLine + tokenResponse.Error}");
                return;
            }

            Console.WriteLine(tokenResponse.Json);

            // call api
            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);

            var response = await client.GetAsync("http://localhost:5000/api/identity");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error from backend:{Environment.NewLine + response.StatusCode}");
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }
        }
    }
}
