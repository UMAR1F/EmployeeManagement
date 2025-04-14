using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibrary.Entities
{
    public class Department:BaseEntity
    {
        public Generaldepartment? Generaldepartment { get; set; }
        public int GeneraldepartmentiD { get; set; }

      public   List<Branch> ?Branches { get; set; }
    }
}
