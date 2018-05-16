using Automation.Common;
using SiteManagement.SiteHelper;
using System.Collections.Generic;

namespace SiteManagement.Models
{
    public class SiteDto 
    {
        public int SiteID { get; set; }
        public string SiteName { get; set; }
        public string SiteFullPath { get; set; }
        public string SiteStatus { get; set; }
        public List<UnionSoftBinding> BindingUrls { get; set; }
    }
}