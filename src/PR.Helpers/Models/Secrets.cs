using System;
using System.Collections.Generic;
using System.Text;

namespace PR.Helpers.Models
{
    public class Secrets
    {
        public string AppId { get; set; }
        public string Password { get; set; }

        public string TenantId { get; set; }

        public string PAT { get; set; }
    }
}
