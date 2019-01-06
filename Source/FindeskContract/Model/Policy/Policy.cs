using Findesk.Model.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Findesk.Model.Policy
{
    public class Policy
    {
        public int? ID { get; set; }

        [Required(ErrorMessage = "Policy number must be non empty")]
        [MaxLength(50, ErrorMessage = "Maximum 50 Characters, allowed for policy number")]
        public string Number { get; set; }

        [Required(ErrorMessage = "Policy name must be non empty")]
        [MaxLength(50, ErrorMessage = "Maximum 50 Characters, allowed for policy name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Date of purchase should be a valid date")]
        public DateTime? DateOfPurchase { get; set; }

        [Required(ErrorMessage = "Date of expiry should be a valid date")]
        public DateTime? DateOfExpiry { get; set; }

        [Required(ErrorMessage = "Sum assured should be a valid amout > 0")]
        public decimal? SumAssured { get; set; }

        [MaxLength(100, ErrorMessage = "Maximum 100 Characters, allowed for Chassis No")]
        public string ChassisNumber { get; set; }

        [MaxLength(100, ErrorMessage = "Maximum 100 Characters, allowed for Vehicle No")]
        public string VehicleNumber { get; set; }

        [Required(ErrorMessage = "Premium should be a valid amout > 0")]
        public decimal? Premium { get; set; }

        [MaxLength(500, ErrorMessage = "Maximum 100 Characters, allowed for Disease descriptions")]
        public string Disease { get; set; }

        [NotMapped]
        public User Insuree { get; set; }

        [Required(ErrorMessage = "Dependents must not be an empty list")]
        public virtual List<Dependent> Dependents { get; set; }

        public virtual List<Document> Documents { get; set; }

        [Required]
        public int WorkGroupID { get; set; }

        public virtual WorkGroup WorkGroup { get; set; }
    };
};
