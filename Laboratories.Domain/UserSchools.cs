using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratories.Domain
{
   public class UserSchools
    {

        public int Id { get; set; }
        public Nullable<int> User_Id { get; set; }
        public Nullable<int> School_Id { get; set; }

        [ForeignKey("School_Id")]
        public virtual School School { get; set; }
        [ForeignKey("User_Id")]
        public virtual User User { get; set; }
    }
}
