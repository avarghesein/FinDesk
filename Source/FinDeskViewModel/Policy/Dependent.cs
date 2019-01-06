using Findesk.VM.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Findesk.VM.Policy
{
    public class Dependent
    {
        [JsonProperty("id")]
        public int? ID { get; set; }

        [JsonProperty("rln")]
        public Relation Relation { get; set; }

        [JsonProperty("usr")]
        public User User { get; set; }
    };
};
