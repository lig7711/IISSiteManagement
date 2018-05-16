using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteManagement.SiteHelper
{
    public class UnionSoftBinding
    {
        public string CertificateName { get; set; }
        public string CertificateThumbprint { get; set; }
        public string Protocol { get; set; }
        public string Hostname { get; set; }
        public int Port { get; set; }
        public string IpAddress { get; set; }
        public string ClickLink { get; set; }
    }
}