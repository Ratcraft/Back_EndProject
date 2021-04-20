using System;
using System.ComponentModel.DataAnnotations;

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
    }
}