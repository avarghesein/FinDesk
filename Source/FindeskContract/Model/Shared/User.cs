using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Findesk.Model.Shared
{
    public class User
    {
        public int? ID { get; set; }

        [Required(ErrorMessage = "First name must be non empty")]
        [MaxLength(50, ErrorMessage = "Maximum 50 Characters, allowed for first name")]
        public string FirstName { get; set; }

        [MaxLength(50, ErrorMessage = "Maximum 50 Characters, allowed for middle name")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Last name must be non empty")]
        [MaxLength(50, ErrorMessage = "Maximum 50 Characters, allowed for last name")]
        public string  LastName { get; set; }

        [Required(ErrorMessage = "Date of birth, should have a valid date")]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Address must be non empty")]
        [MaxLength(300, ErrorMessage = "Maximum 300 Characters, allowed for address")]
        public string Address { get; set; }

        [RegularExpression(@"^\+?[\d- ]+\d$", ErrorMessage = "Invalid Mobile number format")]
        [MaxLength(20, ErrorMessage = "Maximum 20 Characters, allowed for mobile")]
        public string Mobile { get; set; }

        [RegularExpression(@"^\+?[\d- ]+\d$", ErrorMessage = "Invalid Land Line format")]
        [MaxLength(20, ErrorMessage = "Maximum 20 Characters, allowed for Land Line")]
        public string LandLine { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format")]
        [MaxLength(100, ErrorMessage = "Maximum 100 Characters, allowed for email")]
        public string Email { get; set; }

        public BloodGroup BloodGroup { get; set; }

        public bool? RHPositive { get; set; }

        public Gender Gender { get; set; }

        public string ThumbnailID { get; set; }

        public virtual Document Thumbnail { get; set; }

        [Required]
        public int WorkGroupID { get; set; }

        public virtual WorkGroup WorkGroup { get; set; }
    };
};
