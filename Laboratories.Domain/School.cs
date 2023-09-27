using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratories.Domain
{
    public enum SchoolType
    {
        بنين,
        بنات
    }
    public enum SchoolDepartment
    {
        ابتدائى,
        متوسط,
        ثانوى
    }
    public class School:BaseEntity
    {
       
        public string sch_arName { get; set; }
        public string sch_enName { get; set; }
        public string sch_phon { get; set; }
        public string sch_email { get; set; }
        public string sch_address { get; set; }
        public byte[] sch_image { get; set; }
        public SchoolType sch_type { get; set; }
        public SchoolDepartment sch_department { get; set; }
        public int sch_comp_id { get; set; }
        [ForeignKey("sch_comp_id")]
        public Complex sch_complex { get; set; }

        public string sch_laboratoryRecordName { get; set; }

        public ICollection<Item> sch_items { get; set; }
        //public virtual ICollection<User> sch_users { get; set; }
        public ICollection<Teacher> sch_teachers { get; set; }
        public ICollection<Student> sch_students { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public  ICollection<UserSchools> UserSchools { get; set; }


    }
}
