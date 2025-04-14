using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibrary.Entities
{
   public class Generaldepartment:BaseEntity
    {
        //one to many  relatioship with department
        public List<Department>?Departments { get; set; }
    }
}
