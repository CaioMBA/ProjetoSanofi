using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class JwtResponseModel : DefaultResponseModel
    {
        public string? Token { get; set; }
        public DateTime? Expiration { get; set; }
        public UserInfoResponseModel? UserInfo { get; set; }
    }
}
