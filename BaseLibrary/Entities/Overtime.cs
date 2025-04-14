using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibrary.Entities
{
    public class OverTime : OtherBaseEntity
    {
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }

        public int NumbersOfDays => (EndDate - StartDate).Days;

        //Many To One Relationship with VocationType

        public OvertimeType ? OvertimeType { get; set; }
        [Required]
        public int OvertimeTypeId { get; set; }




    }
}