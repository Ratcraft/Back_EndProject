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
        public int Id { get; set; }
        [Required]
        [Range(0,5)]
        public int Rate { get; set; }
    }
}
