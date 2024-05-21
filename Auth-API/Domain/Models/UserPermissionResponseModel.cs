using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class UserPermissionResponseModel
    {
        public required string Permission { get; set; }
        public bool? Permitted { get; set; }
    }
}
