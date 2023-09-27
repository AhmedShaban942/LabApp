using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Laboratories.Domain
{
    public class Screen
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Screen()
        {
            this.ScreenRole = new HashSet<ScreenRole>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int screen_id { get; set; }
        public string screen_name { get; set; }
        public string screen_enName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ScreenRole> ScreenRole { get; set; }
    }
}
