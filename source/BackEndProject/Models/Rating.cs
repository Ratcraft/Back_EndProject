using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Rating
    {
        [Key]
        public int id { get; set; }
        public int Rate { get; set; }
        public int jobId { get; set; }
        public string comment{ get; set; }
    }
}
