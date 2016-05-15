using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class AlertInfoListViewModel
    {
        public List<BrefAlertInfoView> BrefAlertInfoList { get; set; }

        public long Count { get; set; }
    }
}