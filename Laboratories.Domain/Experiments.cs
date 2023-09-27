using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratories.Domain
{
    public enum State
    {
        نفذ,
        لم_ينفذ,
        تحت_التنفيذ
    }
    public class Experiments : BaseEntity
    {

        public string expr_arName { get; set; }
        public string expr_enName { get; set; }

        public string expr_tpye { get; set; }
        public int expr_page { get; set; }
        public int expr_chapter { get; set; }

        public int expr_corsId { get; set; }

        [ForeignKey("expr_corsId")]
        public Course expr_course { get; set; }

        public string expr_tools { get; set; }

        public State expr_state { get; set; }
        public int expr_teachId { get; set; }

        [ForeignKey("expr_teachId")]
        public Teacher expr_teacher { get; set; }
        // public string expr_teacherName { get; set; }

        public string expr_teacherSignature { get; set; }
        public int expr_year { get; set; } = 2023;
        public string expr_ExecutionDate { get; set; }

        //public Department expr_department { get; set; }
    }
}
