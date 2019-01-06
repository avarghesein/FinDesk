using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Findesk.Model.Shared
{
    public class Document
    {
        [Required(ErrorMessage="Document ID must be non empty")]
        public string ID { get; set; }

        [Required(ErrorMessage = "Document name must be non empty")]
        [MaxLength(200, ErrorMessage = "Maximum 200 Characters, allowed for document name")]
        public string Name { get; set; }

        public long Size { get; set; }

        public string ContentType { get; set; }

        [MaxLength]
        public byte[] Content { get; set; }

        [Required]
        public int WorkGroupID { get; set; }

        public virtual WorkGroup WorkGroup { get; set; }

        public override bool Equals(object obj)
        {
            Document othDoc = obj as Document;
            if (othDoc == null)
            {
                return false;
            }

            return ID.Equals(othDoc.ID);
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }
    };
};
