using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Newtonsoft.Json;
using RestSharp;

namespace PH.DNSUpdate
{
    public class DNSUpdater
    {
        public static DnsApiProfile Profile { get; private set; }


        public static RestClient Client { get; } = new RestClient();
        public static string DnsApiBaseUri { get; } = "https://api.cloudflare.com/client/v4/";

        public static string ListDNSBasePath { get; } = @"zones/{0}/dns_records";
        public static string ListDNSPath { get => string.Format(ListDNSBasePath, ZoneID); }
        public static string UpdateDNSBasePath { get; } = @"zones/{0}/dns_records/{1}";
        public static string UpdateDNSPath { get => string.Format(UpdateDNSBasePath, ZoneID, DnsID); }

        public static string ZoneID { get => Profile.ZoneID; }
        public static string DnsID { get => Profile.DnsID; }

        public static string AuthKeyName { get; } = "X-Auth-Key";
        public static string AuthKey { get => Profile.AuthKey; }
        public static string AuthEMailName { get; } = "X-Auth-Email";
        public static string AuthEMail { get => Profile.AuthEMail; }

        public static int CallCount { get; private set; } = 0;
        public static int CallLimit { get; } = 1200;
        public static int TimeInterval { get; } = 300000;//5 Mins

        public static Timer CallCountResetTimer { get; private set; } = new Timer();

        public List<DnsRecord> Records { get; private set; }
        public DnsRecord UpdatedRecord { get; private set; }


        public DNSUpdater()
        {
            Profile = DnsApiProfile.Import();

            Client.BaseUrl = new Uri(DnsApiBaseUri);
            Client.AddDefaultHeader(AuthKeyName, AuthKey);
            Client.AddDefaultHeader(AuthEMailName, AuthEMail);

            CallCountResetTimer.Elapsed += CallCountResetTimer_Elapsed;
            CallCountResetTimer.AutoReset = true;
            CallCountResetTimer.Interval = TimeInterval;
            CallCountResetTimer.Start();

        }


        public bool GetDNSInfo()
        {
            while (CallCount >= CallLimit) { }
            try
            {
                CallCount++;
                RestRequest request = new RestRequest(ListDNSPath, Method.GET, DataFormat.Json);
                request.AddParameter("match", "any");
                request.AddParameter("type", "A, AAAA");
                var response = Client.Execute(request);
                if (response.IsSuccessful)
                {
                    var result = response.Content;
                    var modle = JsonConvert.DeserializeObject<ApiResult<List<DnsRecord>>>(result);
                    var records = modle.Result;
                    Records = records;

                    if (string.IsNullOrEmpty(Profile.PreferedDomain))
                    {
                        var temp = Records.Select(item => (Name: item.Name, Length: item.Name.Length)).ToList();
                            temp.Sort((x, y) =>
                             {
                                 if (x.Length == y.Length)
                                 { return 0; }
                                 else if (x.Length > y.Length)
                                 { return 1; }
                                 else
                                 { return -1; }
                             });
                        Profile.PreferedDomain = temp[0].Name;
                    }
                    for (int i = 0; i < records.Count; i++)
                    {
                        if (records[i].Name == Profile.PreferedDomain)
                        {
                            Profile.DnsID = records[i].ID;

                            return modle.Success;
                        }
                    }
                }
                return false;
            }
            finally
            {
                DnsApiProfile.Export(Profile);
            }

        }

        public bool UpdateDNS(string ip)
        {
            while (CallCount >= CallLimit) { }
            try
            {
                CallCount++;
                RestRequest request = new RestRequest(UpdateDNSPath, Method.PATCH, DataFormat.Json);
                var input = new DnsRecord()
                {
                    Content = ip
                };
                request.JsonSerializer = new RestSharp.Serializers.NewtonsoftJson.JsonNetSerializer();
                request.AddJsonBody(input);
                var response = Client.Execute(request);
                
                if (response.IsSuccessful)
                {
                    var result = response.Content;
                    var modle = JsonConvert.DeserializeObject<ApiResult<DnsRecord>>(result);
                    var record = modle.Result;
                    UpdatedRecord = record;
                    return modle.Success;
                }
                return false;
            }
            finally
            {
                DnsApiProfile.Export(Profile);
            }
        }




        private void CallCountResetTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ResetCallCount();
        }

        private void ResetCallCount()
        {
            CallCount = 0;
        }
    }
}
