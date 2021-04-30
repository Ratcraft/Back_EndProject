using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class ApplyingJob
    {
        [Key]
        public int id { get; set; }

        public int idApplicant { get; set; }

        public int idJobOffer { get; set; }

        public DateTime applyingDate { get; set; }

        public bool hired { get; set; }
    }
}
