using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class ApplyingDTO
    {
        [Required]
        public int idApplicant { get; set; }

        [Required]
        public int idJobOffer { get; set; }
    }
}
