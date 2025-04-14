using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibrary.Entities
{
   public class VacationType:BaseEntity
    {
        //Many to one relationship with vacation

        public List<Vacation>? Vacations { get; set; }
    }
}
