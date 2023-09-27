using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratories.Domain
{
    public enum LessonNum
    {
        فارغ=0, 
        الاولى = 1,
        الثانية = 2,
        الثالثة = 3,
        الرابعة = 4,
        الخامسة = 5,
        السادسة = 6,
    }
    public enum Days
    {
  
        الاحد =1,
        الاثنين = 2,
        الثلاثاء = 3,
        الاربعاء = 4,
        الخميس = 5
    }
    public enum Subject
    {
        فارغ = 0,
        علوم = 1,
        فيزياء = 2,
        كيمياء = 3,
        احياء = 4,
    }
    public class ScheduleDet
    {
        [Key, Column(Order = 0)]
        public int schd_Id { get; set; }  //=teatcher

        [Key, Column(Order = 1)]
        public Days schd_day { get; set; }
   
        public LessonNum schd_lessonNum1 { get; set; }
        public Subject schd_subject1 { get; set; }

        public LessonNum schd_lessonNum2 { get; set; }
        public Subject schd_subject2 { get; set; }
        public LessonNum schd_lessonNum3 { get; set; }
        public Subject schd_subject3 { get; set; }
        public LessonNum schd_lessonNum4 { get; set; }
        public Subject schd_subject4 { get; set; }
        public LessonNum schd_lessonNum5 { get; set; }
        public Subject schd_subject5 { get; set; }

        public int schd_scheduleHedId { get; set; } = 1;

        [ForeignKey("schd_scheduleHedId")]
        public ScheduleHed schd_scheduleHed { get; set; }


    }
}
