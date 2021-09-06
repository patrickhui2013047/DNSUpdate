using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.DNSUpdate
{
    [JsonObject]
    public class ApiResult<TData> 
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("errors")]
        public List<object> Errors { get; set; }

        [JsonProperty("messages")]
        public List<object> Messages { get; set; }

        [JsonProperty("result")]
        public TData Result { get; set; }
    }

    [JsonObject]
    public class DnsRecord
    {
        [JsonProperty("id")]
        public string ID { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("content")]
        public string Content { get; set; }
        [JsonProperty("proxiable")]
        public bool Proxiable { get; set; }
        [JsonProperty("proxied")]
        public bool Proxied { get; set; }
        [JsonProperty("ttl")]
        public string TTL { get; set; }
        [JsonProperty("locked")]
        public bool Locked { get; set; }
        [JsonProperty("zone_id")]
        public string ZoneID { get; set; }
        [JsonProperty("zone_name")]
        public string ZoneName { get; set; }
        [JsonProperty("created_on")]
        public string CreatedOn { get; set; }
        [JsonProperty("modified_on")]
        public string ModifiedOn { get; set; }
        [JsonProperty("data")]
        public object Data { get; set; }
        [JsonProperty("meta")]
        public object Meta { get; set; }
    }
}
