using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Findesk.VM.Shared
{
    public class WorkGroup
    {
        [JsonProperty("id")]
        public int? ID { get; set; }

        [JsonProperty("nme")]
        public string Name { get; set; }

        [JsonProperty("mods")]
        public List<Module> Modules { get; set; }

        [JsonProperty("mems")]
        public List<WorkGroupMemberRole> WorkGroupMemberRoles { get; set; }
    };
};
