using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class User
    {
        [Key]
        public int id {get; set; }
        public string firstName {get; set; }
        public string lastName {get; set; }
        public string birthDate {get; set; }
        public string sex {get; set; }
        public string userName {get; set; }
        public string emailAdress {get; set; }
        public string password {get; set; }
        public string passwordHash { get; set; }
        public int levelAccess {get; set; }
        public string domain { get; set; }
    }
}