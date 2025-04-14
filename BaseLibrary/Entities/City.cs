using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibrary.Entities
{
    public class City:BaseEntity
    {
        //Many relationship to one with
        public Country? Country { get; set; }
        public int CountryId { get; set; }

        //Many to One relationship with Town


            public List<Town>? Towns { get; set; }
       

        
        


    }
}
