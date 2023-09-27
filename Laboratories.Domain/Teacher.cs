using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratories.Domain
{
    public class Teacher:BaseEntity
    {
        [Required]
        public string tech_arName { get; set; }
        public string tech_enName { get; set; }
        public string tech_phon { get; set; }
        public string tech_email { get; set; }
        public string tech_address { get; set; }
        public int? tech_schId { get; set; }

        [ForeignKey("tech_schId")]
        public School tech_School { get; set; }
        public ICollection<TeacherMovment> teacherMovments { get; set; }
        ICollection<Experiments> tech_experiments { get; set; }

    }
}
