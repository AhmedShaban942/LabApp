using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratories.Domain
{
    public class User
    {
        //public enum Role
        //{
        //    مدير_نظام,
        //    معلم_معلمة  ,
        //    مشرف_مشرفة
           
        //}
        public int Id { get; set; }
        public int usr_num { get; set; }
        public string usr_arName { get; set; }
        public string usr_enName { get; set; }
        //public Role usr_role { get; set; }
        public string usr_pass { get; set; }
        public string usr_mobile { get; set; }
        public string usr_emial { get; set; }
        public string usr_address { get; set; }
        public byte[] usr_image { get; set; }
        //public int? usr_schId { get; set; }

        //[ForeignKey("usr_schId")]
        //public School usr_School { get; set; }
        public int? usr_roleId { get; set; }
        [ForeignKey("usr_roleId")]
        public virtual UserRole usr_Role { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public  ICollection<UserSchools> UserSchools { get; set; }
    }
}
