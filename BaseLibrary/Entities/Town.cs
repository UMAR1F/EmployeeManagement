using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibrary.Entities
{
   public class Town : BaseEntity
    {
       
        //Relationship:One to Many Employee
        public List<Employees>? Employees { get; set; }

        //Many to One relationship with City
   

        public City? City { get; set; }
        public int CityId { get; set; }
    }
}
