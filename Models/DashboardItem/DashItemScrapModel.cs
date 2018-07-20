using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntranetApplication.Models.DashboardItem
{
    public class DashItemScrap :  DashItemBase
    {
        public string SourceURL { get; set; }
        public string LogonUser { get; set; }
        public string LogonPwd { get; set; }
        public string ClickThruURL { get; set; }
        public string CssSelector { get; set; }
        public string ImageURI { get; set; }

        public static explicit operator DashItemScrap(DashboardItem v)
        {
            return new DashItemScrap()
            {
                DashboardID = v.DashboardID,
                DashboardItemID = v.DashboardItemID,
                DashboardItemStatusID = v.DashboardItemStatusID,
                DashboardTypeID = v.DashboardTypeID,             
                EndDateTime = v.EndDateTime,
                SortOrder = v.SortOrder,
                StartDateTime = v.StartDateTime,
                Title = v.Title,

                SourceURL = v.SourceURL,
                LogonUser = v.LogonUser,
                LogonPwd = v.LogonPwd,
                ClickThruURL = v.ClickThruURL,
                CssSelector = v.CssSelector,
                ImageURI = v.ImageURI

            };

        }
    }
}
