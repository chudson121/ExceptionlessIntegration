using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionlessIntegration
{
    public class ExceptionlessConfig
    {
        public string BaseUrl { get; set; }
        public string AuthToken { get; set; }
        public string AccountEmail { get; set; }
        public string AccountPassword { get; set; }

        public string OrganizationId { get; set; }
    }
}
