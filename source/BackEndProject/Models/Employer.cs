using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Employer : User
    {
        [Key]
        public string Company {get; set;}
        public string Job {get; set;}
        //public List<int> offers { get; set; }
    }
}