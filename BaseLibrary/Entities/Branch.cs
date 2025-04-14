using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibrary.Entities
{
    public class Branch:BaseEntity
    {
        //Relationshionship:Many to one
        public Department? Department { get; set; }
        public int DepartmentId { get; set; }

        //Relationship:One to many
       public List<Employees> ?Employees { get; set; }



       

   

    }
}
