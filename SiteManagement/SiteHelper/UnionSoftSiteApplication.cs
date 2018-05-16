using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteManagement.SiteHelper
{
    public class UnionSoftSiteApplication
    {
        public string ApplicationPool { get; set; }
        public string Path { get; set; }
        public string DiskPath { get; set; }
        public bool IsApplication { get; set; }
    }
}