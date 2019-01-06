using Findesk.VM.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Findesk.VM.Policy
{
    public class Policy
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("num")]
        public string Number { get; set; }

        [JsonProperty("nme")]
        public string Name { get; set; }

        [JsonProperty("dop")]
        public DateTime? DateOfPurchase { get; set; }

        [JsonProperty("doe")]
        public DateTime? DateOfExpiry { get; set; }

        [JsonProperty("sum")]
        public decimal? SumAssured { get; set; }

        [JsonProperty("chsNum")]
        public string ChassisNumber { get; set; }

        [JsonProperty("vehNum")]
        public string VehicleNumber { get; set; }

        [JsonProperty("prm")]
        public decimal? Premium { get; set; }

        [JsonProperty("dise")]
        public string Disease { get; set; }

        [JsonProperty("usr")]
        public User Insuree { get; set; }

        [JsonProperty("deps")]
        public List<Dependent> Dependents { get; set; }

        [JsonProperty("docs")]
        public List<Document> Documents { get; set; }

        [JsonProperty("wgp")]
        public WorkGroup WorkGroup { get; set; }
    };
};
