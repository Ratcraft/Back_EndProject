namespace Models
{
    public class User
    {
        public string firstName {get; set; }
        public string lastName {get; set; }
        public string birthDate {get; set; }
        public string sex {get; set; }
        public string userName {get; set; }
        public string emailAdress {get; set; }
        private int id {get; set; }
        public string password {get; set; }
        private int levelAccess {get; set; }
        public string domain { get; set; }
    }
}