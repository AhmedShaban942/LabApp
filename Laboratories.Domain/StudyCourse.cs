using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratories.Domain
{

    public class StudyCourse
    {

        [Key]
        public int Id { get; set; }
        [Required]
        public string cors_arName { get; set; }
        public string cors_enName { get; set; }
        [Required]
        public Department cors_department { get; set; }

        public Level cors_level { get; set; } = 0;
        public Term cors_term { get; set; } = 0;  //الفصل الدراسى

        public double cors_degree { get; set; }
      
        public double cors_rate { get; set; }
    }
}
