using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace IntranetApplication.Models
{
    public class Dashboard
    {
        public int DashboardID { get; set; }
        public string DashboardTitle { get; set; }
        public int SortOrder { get; set; }
        public int DashboardStatusID { get; set; }


        public Dashboard Create(IDataRecord record)
        {
            return new Dashboard
            {
                DashboardID = (int)record["DashboardID"],
                DashboardTitle = record["DashboardTitle"].ToString(),
                SortOrder = (int)record["SortOrder"],
                DashboardStatusID = (int)record["DashboardStatusID"]
            };
        }

    }

}
