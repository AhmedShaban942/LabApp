using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratories.Domain
{
    public class Course
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string cors_arName { get; set; }
        public string cors_enName { get; set; }
        public int cors_exprNum { get; set; }
        public Term cors_term { get; set; } = 0;  //الفصل الدراسى
        public Level cors_level { get; set; } = 0;
        ICollection<Experiments> cors_experiments { get; set; }
        public Department cors_department { get; set; } = 0;
    }
}
