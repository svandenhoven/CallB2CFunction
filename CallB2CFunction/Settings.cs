using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallB2CFunction
{
    [JsonObject("b2c")]
    public class Settings
    {
        [JsonProperty("tenant")]
        public string? Tenant { get; set; }
        [JsonProperty("azureADB2CHostname")]
        public string? AzureADB2CHostname { get; set; }
        [JsonProperty("clientID")]
        public string? ClientID { get; set; }
        [JsonProperty("policySignUpSignIn")]
        public string? PolicySignUpSignIn { get; set; }
        [JsonProperty("Scopes")]
        public string? Scopes { get; set; }
    }
}
