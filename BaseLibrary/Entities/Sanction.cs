




using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.Entities
{
    public class Sanction:OtherBaseEntity
    {
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string punishment { get; set; } = string.Empty;

        [Required]
        public DateTime PunishmentDate { get; set; }

        //Many To One Relationship with VocationType

        public SanctionType ? SanctionType { get; set; }
       
    }
}