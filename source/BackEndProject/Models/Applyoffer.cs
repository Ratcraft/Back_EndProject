using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Applyoffer
    {
        [Key]
        public int id { get; set; }
        public int userid { get; set; }
        public int jobid { get; set; }
    }
}