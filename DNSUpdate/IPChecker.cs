using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;

namespace PH.DNSUpdate
{
    public class IPChecker
    {
        public static HttpClient Client { get; } = new HttpClient();
        public static string IPCheckingAddress { get; } = "http://bot.whatismyipaddress.com/";
        public static string IP { get; private set; } = "0.0.0.0";

        public static async Task UpdateIP()
        {
            Client.BaseAddress = new Uri(IPCheckingAddress);
            var responce = await Client.SendAsync(new HttpRequestMessage());
            if (responce.IsSuccessStatusCode)
            {
                var result = await responce.Content.ReadAsStringAsync();
                IPAddress.TryParse(result, out IPAddress ip);
                IP = ip.ToString();
                return;
            }
            throw new IPUpdateException("Fail to update IPAddress") { ResponseMessage = responce };
        }
    }
    public class IPUpdateException : Exception
    {
        public HttpResponseMessage ResponseMessage { get; set; }
        public IPUpdateException(string message) : base(message) { }
    }
}
