using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratories.Domain
{
    public class ScreenRole
    {
        [Key]
        public int screen_role_id { get; set; }
        public Nullable<int> role_id { get; set; }
        public Nullable<int> screen_id { get; set; }


        public virtual UserRole Role { get; set; }
        public virtual Screen Screen { get; set; }
    }
}
