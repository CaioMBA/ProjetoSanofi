using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class CreateUserRequestModel
    {
        public required string Login { get; set; }
        public required string Name { get; set; }
        public required string Password { get; set; }
        public required string Document { get; set; }
        public required string Email { get; set; }
        public required DateTime Birthday { get; set; }
        public string? Photo { get; set; }
        public string? Phone { get; set; }
    }
}
