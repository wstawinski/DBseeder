using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBseeder.Models
{
    class ApiResponse
    {
        public string Status { get; set; }
        public int TotalResults { get; set; }
        public List<ApiResponseArticle> Articles { get; set; }
    }
}
