using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Laboratories.Domain
{
    public class TeacherMovment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? mov_lessonCount { get; set; }
        public int? mov_lessonCountActual { get; set; }
        [Key, Column(Order = 0)]
        public int mov_weekNumber { get; set; }
        [Key, Column(Order = 1)]
        public int mov_monthNumberr { get; set; }
        [Key, Column(Order = 2)]
        public int mov_techId { get; set; } = 1;

        [ForeignKey("mov_techId")]
        public Teacher mov_Teacher { get; set; }

        public string mov_note { get; set; }

    }
}
