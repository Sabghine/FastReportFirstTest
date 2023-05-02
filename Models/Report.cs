using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FastReportTutorial.Models
{
    public class Report
    {
        public int ReportId { get; set; }
        public string ReportName { get; set; }
        public string ReportData { get; set; }
        public DateTime ReportDate { get; set; }
    }
}