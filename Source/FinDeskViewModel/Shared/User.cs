using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Findesk.VM.Shared
{
    public class User
    {
        [JsonProperty("id")]
        public int? ID { get; set; }

        [JsonProperty("fnm")]
        public string FirstName { get; set; }

        [JsonProperty("mnm")]
        public string MiddleName { get; set; }

        [JsonProperty("lnm")]
        public string  LastName { get; set; }

        [JsonProperty("dob")]
        public DateTime? DateOfBirth { get; set; }

        [JsonProperty("adr")]
        public string Address { get; set; }

        [JsonProperty("mob")]
        public string Mobile { get; set; }

        [JsonProperty("lnd")]
        public string LandLine { get; set; }

        [JsonProperty("eml")]
        public string Email { get; set; }

        [JsonProperty("bgp")]
        public BloodGroup BloodGroup { get; set; }

        [JsonProperty("rhf")]
        public bool? RHPositive { get; set; }

        [JsonProperty("gen")]
        public Gender Gender { get; set; }

        [JsonProperty("thmb")]
        public Document Thumbnail { get; set; }

        [JsonProperty("wgp")]
        public WorkGroup WorkGroup { get; set; }
    };
};
