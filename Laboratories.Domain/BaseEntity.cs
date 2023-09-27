using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratories.Domain
{
   public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public DateTime? AddedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? AddedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public bool IsAvtive { get; set; }
        public void get()
        {

        }
    }
}
