using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratories.Domain
{
    public class StudentGradesHed
    {
        [Key]
        public Int64 stdGH_Id { get; set; }
        public int stdGH_schId { get; set; } = 1;

        [ForeignKey("stdGH_schId")]
        public School stdGH_School { get; set; }

        public int stdGH_studyCourseId { get; set; } = 1;

        [ForeignKey("stdGH_studyCourseId")]
        public StudyCourse stdGH_StudyCourse { get; set; }

        public int stdGH_teacherId { get; set; } = 1;

        [ForeignKey("stdGH_teacherId")]
        public Teacher stdGH_Teacher { get; set; }

        public int stdGH_examId { get; set; } = 1;

        [ForeignKey("stdGH_examId")]
        public Exam stdGH_Exams { get; set; }

        public string stdGH_Date { get; set; }
        public Term stdGH_term { get; set; }
        public Level stdGH_level { get; set; }
        public record stdGH_stdRecord { get; set; }

        public ICollection<StudentGradesDet> StudentGradesDet { get; set; }




    }
}
