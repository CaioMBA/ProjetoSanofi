using System.Security.Claims;

namespace Domain.Models.Security
{
    public class ValidatedJwtResponseModel : DefaultResponseModel
    {
        public string? Token { get; set; }
        public IEnumerable<Claim>? Claims { get; set; }
    }
}
