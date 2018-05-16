using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteManagement.SiteHelper
{
    public class IisSite
    {
        public int IisId { get; set; }
        public string Name { get; set; }
        public string ApplicationPool { get; set; }
        public string SitePath { get; set; }
        public object SiteState { get; set; }
        public string LogFileDirectory { get; set; }
        public List<UnionSoftBinding> Bindings { get; set; }
        public List<UnionSoftSiteApplication> Applications { get; set; }
    }
}