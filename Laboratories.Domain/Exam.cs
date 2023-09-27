using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratories.Domain
{
    public class Exam
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string exm_arName { get; set; }
        public string exm_enName { get; set; }

    }
}
