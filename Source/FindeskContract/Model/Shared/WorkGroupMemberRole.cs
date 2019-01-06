using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Findesk.Model.Shared
{
    public class WorkGroupMemberRole
    {
        public int ID { get; set; }

        [Required]
        public WorkGroupRole Role { get; set; }

        [Required]
        public int WorkGroupID { get; set; }

        public virtual WorkGroup WorkGroup { get; set; }
        
        [Required]
        public int WorkGroupMemberID { get; set; }

        public virtual WorkGroupMember WorkGroupMember { get; set; }
    };
};
