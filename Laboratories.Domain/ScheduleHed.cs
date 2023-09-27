using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratories.Domain
{
    public enum Chapter
    {
        الاول,
        الثانى,
        الثالث
    }
    public class ScheduleHed
    {
        [Key]
        public int schd_Id { get; set; }
        public int? schd_lessonCount { get; set; }
        public int schd_teachId { get; set; }

        [ForeignKey("schd_teachId")]
        public Teacher schd_teacher { get; set; }
        public Chapter schd_chapter { get; set; }
        public ICollection<ScheduleDet> scheduleDet{ get; set; }

    }
}
