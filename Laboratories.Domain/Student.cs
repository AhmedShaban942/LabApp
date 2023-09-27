using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratories.Domain
{
  public enum record
    {
        أ,
        ب,
        ج,
        د,
        ه,
        ط,
        و,
        ى

    }
    public class Student : BaseEntity
    {
        [Required]
        public Int64 std_IdentityNumber { get; set; }
        [Required]
        public string std_arName { get; set; }
        public string std_enName { get; set; }
        public string std_phon { get; set; }
        public string std_email { get; set; }
        public string std_address { get; set; }
        [Required]
        public Department std_department { get; set; }

        public Level std_level { get; set; } = 0;

        public record std_levelRecord { get; set; }
        public byte[] std_image { get; set; }
        public int? std_schId { get; set; }

        [ForeignKey("std_schId")]
        public School std_School { get; set; }

    }
}
