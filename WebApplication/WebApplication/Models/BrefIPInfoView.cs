using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class BrefIPInfoView
    {
        public string IP { get; set; }

        public string Region { get; set; }

        public string Model { get; set; }

        public string LostTime { get; set; }

        public string RecoveryTime { get; set; }
    }
}