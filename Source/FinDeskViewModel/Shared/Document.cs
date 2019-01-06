using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Findesk.VM.Shared
{
    public class Document
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("nme")]
        public string Name { get; set; }

        [JsonProperty("sze")]
        public long Size { get; set; }

        [JsonProperty("wgp")]
        public WorkGroup WorkGroup { get; set; }
    };
};
