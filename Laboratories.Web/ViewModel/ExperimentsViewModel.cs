using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Laboratories.Web.ViewModel
{
 
    public class ExperimentsViewModel 
    {
        public string expr_arName { get; set; }
        public string expr_enName { get; set; }

        public string expr_tpye { get; set; }
        public int expr_page { get; set; }
        public int expr_chapter { get; set; }

        public string expr_course { get; set; }

        public string expr_tools { get; set; }

        public string expr_state { get; set; }

        public string expr_teacherName { get; set; }

        public string expr_teacherSignature { get; set; }
        public int expr_year { get; set; } = 2023;

        public string expr_ExecutionDate { get; set; }

    }
}