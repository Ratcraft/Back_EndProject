using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Models;

namespace Models
{
    public class Joboffer
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string duration { get; set; }
        public DateTime dateCreated { get; set; }
        public string bossusername { get; set; }
        public DateTime deadline { get; set; }

        //public List<int> applying_people { get; set; }
        //public List<int> working_people { get; set; }
    }
}