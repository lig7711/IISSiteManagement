using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteManagement.SiteHelper
{
    public class Certificate
    {
        public string Name { get; set; }
        public byte[] Hash { get; set; }
        public string Thumbprint { get; set; }
    }
}