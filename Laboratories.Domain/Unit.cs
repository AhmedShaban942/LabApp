using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratories.Domain
{
  public  class Unit:BaseEntity
    {

        public string unt_arName { get; set; }
        public string unt_enName { get; set; }
        public ICollection<Item> unt_item { get; set; }
    }
}
