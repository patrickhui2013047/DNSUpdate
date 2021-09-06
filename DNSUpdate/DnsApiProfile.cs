using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.DNSUpdate
{
    [JsonObject]
    public class DnsApiProfile
    {
        private const string defaultPath = "./dnsUpdater.conf";

        [JsonProperty, JsonRequired]
        public string ZoneID { get; set; } = "";
        [JsonProperty, JsonRequired]
        public string DnsID { get; set; } = "";
        [JsonProperty, JsonRequired]
        public string AuthKey { get; set; } = "";
        [JsonProperty, JsonRequired]
        public string AuthEMail { get; set; } = "";
        [JsonProperty, JsonRequired]
        public string PreferedDomain { get; set; } = "";


        public static void CreateTemplate()
        {
            Export(new DnsApiProfile());
        }

        public static string ToJson(DnsApiProfile profile)
        {
            return JsonConvert.SerializeObject(profile);
        }
        public static DnsApiProfile Parse(string json)
        {
            return JsonConvert.DeserializeObject<DnsApiProfile>(json);
        }
        public static void Export(DnsApiProfile profile, string path = defaultPath)
        {
            using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                stream.Write(UTF8Encoding.UTF8.GetBytes(ToJson(profile)));
                stream.Flush();
                stream.Close();
            }
        }
        public static DnsApiProfile Import(string path = defaultPath)
        {
            try
            {
                var profile = Parse(File.ReadAllText(path));
                if (profile == null)
                {
                    throw new Exception();
                }
                return profile;
            }
            catch (Exception e)
            {
                CreateTemplate();
                return Import();
            }
        }
    }


}
