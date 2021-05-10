using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Employer
    {
        [Key]
        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string birthDate { get; set; }
        public string sex { get; set; }
        public string userName { get; set; }
        public string emailAdress { get; set; }
        public string password { get; set; }
        public string levelAccess { get; set; }
        public string domain { get; set; }
        public DateTime created { get; set; }
        public DateTime lastModified { get; set; }
        public bool isbanned {get;set;}
        public string company {get; set;}
        public string job {get; set;}
        //public List<int> offers { get; set; }
    }
}