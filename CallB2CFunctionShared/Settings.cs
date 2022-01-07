using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallB2CFunctionShared
{
    [JsonObject("azure")]
    public class Settings
    {
        [JsonProperty("tenant")]
        public string? Tenant { get; set; }
        [JsonProperty("authority")]
        public string? Authority { get; set; }
        [JsonProperty("clientID")]
        public string? ClientID { get; set; }
        [JsonProperty("clientsecret")]
        public string? ClientSecret { get; set; }
        [JsonProperty("policySignUpSignIn")]
        public string? PolicySignUpSignIn { get; set; }
        [JsonProperty("Scopes")]
        public string? Scopes { get; set; }
    }
}