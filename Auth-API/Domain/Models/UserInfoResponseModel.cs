using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class UserInfoResponseModel
    {
        public long UserID { get; set; }
        public required string Name { get; set; }
        public required string Login { get; set; }
        public DateTime DateCreation { get; set; }
        public bool Active { get; set; }
    }
}
