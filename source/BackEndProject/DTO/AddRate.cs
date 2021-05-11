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
        public string usermail {get;set;}
        [Required]
        [Range(0, 20)]
        public int Rate { get; set; }
        [Required]
        public int jobId { get; set; }
        public string comment{ get; set; }
    }
}
