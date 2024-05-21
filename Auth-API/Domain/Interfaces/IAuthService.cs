using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IAuthService
    {
        JwtResponseModel GenerateToken(string BasicAuth);
        ValidatedJwtResponseModel VerifyToken(string Token);
    }
}
