using Automation.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteManagement.Models
{
    public class SiteQueryDto : PagedListResult<SiteDto>
    {
        public string SiteName { get; set;  }
    }
}