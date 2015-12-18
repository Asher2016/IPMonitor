using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class MonitorRecordListViewModel
    {
        public List<BrefIPInfoView> BrefIPInfoList { get; set; }

        public long Count { get; set; }
    }
}