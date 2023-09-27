using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratories.Domain
{
    public class StudentGradesDet
    {

        [Key, Column(Order = 0)]
        public Int64 stdGD_studentGradesHedId { get; set; } = 1;
        [ForeignKey("stdGD_studentGradesHedId")]
        public StudentGradesHed stdGD_studentGradesHed { get; set; }

        [Key, Column(Order = 1)]
        public int stdGD_studentId { get; set; } = 1;
        [ForeignKey("stdGD_studentId")]
        public Student stdGD_Student { get; set; }


        public double stdGD_degree { get; set; }

        public double stdGD_rate { get; set; }
    }
}
