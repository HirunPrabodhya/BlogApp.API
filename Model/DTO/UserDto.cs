using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class UserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Bio { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int UserTypeId { get; set; } 
    }
}
