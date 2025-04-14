using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibrary.Entities
{
    public class Employees : BaseEntity
    {
     
        [Required]
        public string? CivilId { get; set; }=string.Empty;
        [Required]
        public string? FUllName { get; set; } = string.Empty;
        [Required]
        public string? FileNumber { get; set; } = string.Empty;
        [Required]
        public string? JobName { get; set; }=string.Empty;
        [Required]
        public string? Address { get; set; } = string.Empty;
        [Required , DataType(DataType.PhoneNumber)]
        public string? TelephoneNumber { get; set; } = string.Empty;
        [Required]
        public string? Photo { get; set; } = string.Empty;
        public string? Others { get; set; } = string.Empty;
       

        //public Generaldepartment? Generaldepartment { get; set; }
        //public int GeneraldepartmentId {get;set;}
        //public Department? Department { get; set;}

        ////Relationship Many to One\\
        public Department? Department { get; set; }
        public int DepartmentId {get;set;}

        public Branch? Branch { get; set; }
        public int BranchId { get; set; }

        public Town? Towns { get; set; }
        public int TownId { get; set; }
    }
}
