using System.Collections.Generic;
using Models;

namespace DTO
{
    public class UserDetailsDTO : User
    {
        public List<int> applying_offer {get;set;}
    }
}