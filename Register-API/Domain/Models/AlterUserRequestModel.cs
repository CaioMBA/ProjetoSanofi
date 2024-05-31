using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class AlterUserRequestModel
    {
        public required long UserID { get; set; }
        public string? Name { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Document { get; set; }
        public DateTime? Birthday { get; set; }
        public string? Photo { get; set; }
        public bool Active { get; set; }
    }
}
