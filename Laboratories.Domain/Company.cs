using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Laboratories.Domain
{
   public  class Company:BaseEntity
    {
        [Required]
        public string com_arName { get; set; }

        public string  com_enName { get; set; } 
        public string com_phon { get; set; }
        public string com_email { get; set; }
        public string com_address { get; set; }
        public byte[] com_image { get; set; }
        public ICollection<Complex> com_Complices { get; set; }


    }
}
