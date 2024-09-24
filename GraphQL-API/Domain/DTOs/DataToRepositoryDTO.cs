using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class DataToRepositoryDTO
    {
        public required string Columns { get; set; }
        public required string TableName { get; set; }
        public string? Where { get; set; }
        public dynamic? Parameters { get; set; }

    }
}
