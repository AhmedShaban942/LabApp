using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratories.Domain
{
   public class Complex:BaseEntity
    {
        [Required]
        public string comp_arName { get; set; }
        public string comp_enName { get; set; }
        public string comp_phon { get; set; }
        public string comp_email { get; set; }
        public string comp_address { get; set; }
        public byte[] comp_image { get; set; }
        public int comp_com_id { get; set; }

        [ForeignKey("comp_com_id")]
        public Company comp_company { get; set; }
        public ICollection<School> comp_Schools { get; set; }
    }
}
