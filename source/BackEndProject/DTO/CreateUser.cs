using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class CreateUser
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
    }
}