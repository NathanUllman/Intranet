using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntranetApplication.Models.DashboardItem
{
    public class DashItemBase
    {
        public int DashboardItemID { get; set; }
        public int DashboardID { get; set; }
        public string Title { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int SortOrder { get; set; }
        public int DashboardTypeID { get; set; }
        public int DashboardItemStatusID { get; set; }
    }
}
