
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Laboratories.Web.ViewModel
{
    public class ScheduleViewModel
    {
        public string schd_scool { get; set; }
        public string schd_teacher { get; set; }
        public string schd_laboratoryRecordName { get; set; }
        public string schd_department { get; set; }
        public string schd_chapter { get; set; }

       
        public string schd_day { get; set; }

        public string schd_lessonNum1 { get; set; }
        public string schd_subject1 { get; set; }

        public string schd_lessonNum2 { get; set; }
        public string schd_subject2 { get; set; }
        public string schd_lessonNum3 { get; set; }
        public string schd_subject3 { get; set; }
        public string schd_lessonNum4 { get; set; }
        public string schd_subject4 { get; set; }
        public string schd_lessonNum5 { get; set; }
        public string schd_subject5 { get; set; }
        public int schd_lessonCount { get; set; }


        public int schd_lessonCountActual { get; set; }

        public int schd_weekNumber { get; set; }

        public int schd_monthNumberr { get; set; }

        public string mov_note { get; set; }
        public double mov_percent { get; set; }
    }
}