using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionlessIntegration
{

    public class TokenDto
    {
        public string id { get; set; }
        public object refresh { get; set; }
        public DateTime created_utc { get; set; }
        public DateTime modified_utc { get; set; }
        public object organization_id { get; set; }
        public string project_id { get; set; }
        public object application_id { get; set; }
        public object default_project_id { get; set; }
        public string[] scopes { get; set; }
        public string notes { get; set; }
    }

}
