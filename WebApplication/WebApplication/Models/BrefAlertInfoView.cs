using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class BrefAlertInfoView
    {
        public string IP { get; set; }

        public string Region { get; set; }

        public string Model { get; set; }

        public string FirstLostTime { get; set; }

        public string SecondLostTime { get; set; }

        public string IsSend { get; set; }
    }
}