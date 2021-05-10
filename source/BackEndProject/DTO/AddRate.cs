using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DTO
{
    public class AddRate
    {
        [Required]
        [Range(0, 5)]
        public int User_Rate { get; set; }
        [Required]
        public int JobID {get;set;}
        public string comment {get;set;}
    }
}
