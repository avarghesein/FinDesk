using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Findesk.VM.Shared
{
    public class WorkGroupMember
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("lgnId")]
        public string LoginID { get; set; }

        [JsonProperty("nme")]
        public string Name { get; set; }

        [JsonProperty("wgpRls")]
        public List<WorkGroupMemberRole> WorkGroupMemberRoles { get; set; }
    };
};
