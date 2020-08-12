using CacheCow.Client;
using CacheCow.Client.RedisCacheStore;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace TimeClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //var client = new HttpClient();
            var client = ClientExtensions.CreateClient(new RedisStore("localhost:6379"));
            client.BaseAddress = new Uri("http://localhost:1337");

            while(true)
            {
                Console.WriteLine("Hit Enter to get the time (done to end)");
                var answer = Console.ReadLine();
                if (answer == "done") break;
                var response = await client.GetAsync("/time");
                Console.WriteLine(response.Headers.CacheControl.ToString());
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);

            }
        }
    }
}
