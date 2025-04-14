


using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.Entities
{
    public class Vacation:OtherBaseEntity
    {
        [Required]
        public DateTime StartDate { set; get; }
        [Required]
        public int NumbersOfDays { get; set; }
        public DateTime? EndDate => StartDate.AddDays(NumbersOfDays);


        public VacationType VacationType { get; set; }
        [Required]
        public int VacationTypeId { get; set; }
    }
}