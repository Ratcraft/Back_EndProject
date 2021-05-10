using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class CreateEmployer
    {
        [Required]
        public string firstName {get; set; }
        [Required]
        public string lastName {get; set; }
        [Required]
        public string birthDate {get; set; }
        [Required]
        public string sex {get; set; }
        [Required]
        public string userName {get; set; }
        [Required]
        public string emailAdress {get; set; }
        [Required]
        public string password {get; set; }
        [Required]
        public string domain { get; set;}
        [Required]
        public string company {get;set;}
        [Required]
        public string job {get;set;}
    }
}