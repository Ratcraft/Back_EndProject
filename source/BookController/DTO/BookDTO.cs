using Models;

namespace DTO
{
    public class BookDTO
    {
        public decimal Book_price { get; set; }
        public string ISBN { get; set; }
        public int Book_id { get; set; }
        public string Book_name { get; set; }
        public string Book_description { get; set; }
    }
}