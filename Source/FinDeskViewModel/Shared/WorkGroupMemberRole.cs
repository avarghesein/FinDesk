using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Findesk.VM.Shared
{
    public class WorkGroupMemberRole
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("rle")]
        public WorkGroupRole Role { get; set; }
    };
}
