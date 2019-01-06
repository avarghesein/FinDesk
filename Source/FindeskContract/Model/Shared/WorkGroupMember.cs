using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Findesk.Model.Shared
{
    public class WorkGroupMember
    {
        public int ID { get; set; }

        [Required]
        public string LoginID { get; set; }

        public string Name { get; set; }

        public List<WorkGroupMemberRole> WorkGroupMemberRoles { get; set; }
    };
};
