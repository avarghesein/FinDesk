using Findesk.Model.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Findesk.Model.Policy
{
    public class Dependent
    {
        public int? ID { get; set; }

        [Required(ErrorMessage = "Relation should be set for the dependent")]
        public Relation Relation { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required(ErrorMessage = "There should be valid person, as dependent")]
        public virtual User User { get; set; }

        [Required]
        public int PolicyID { get; set; }

        [Required(ErrorMessage = "There should be valid policy, attached to the dependent")]
        public virtual Policy Policy { get; set; }
    };
};
