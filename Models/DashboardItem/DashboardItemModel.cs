using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using IntranetApplication.Models.DashboardItem;

namespace IntranetApplication.Models.DashboardItem
{
    public class DashboardItem : DashItemBase
    {
        public string SourceURL { get; set; }
        public string LogonUser { get; set; }
        public string LogonPwd { get; set; }
        public string ImageURI { get; set; }
        public string CssSelector { get; set; }
        public string ClickThruURL { get; set; }
        public string DisplayText { get; set; }

        public DashboardItem Create(IDataRecord record)
        {

            return new DashboardItem
            {
                DashboardItemID = (int) record["DashboardItemID"],
                DashboardID =  (int) record["DashboardID"],
                Title = record["Title"].ToString(),
                SourceURL = record["SourceURL"].ToString(),
                LogonUser = record["LogonUser"].ToString(),
                LogonPwd = record["LogonPwd"].ToString(),
                CssSelector = record["CssSelector"].ToString(),
                ImageURI = record["ImageURI"].ToString(),
                ClickThruURL = record["ClickThruURL"].ToString(),
                StartDateTime = (DateTime)record["StartDateTime"],
                EndDateTime =  (DateTime)record["EndDateTime"],
                SortOrder = (int) record["SortOrder"],
                DashboardTypeID = (int) record["DashboardTypeID"],
                DashboardItemStatusID =  (int) record["DashboardItemStatusID"],
                DisplayText = record["DisplayText"].ToString(),

            };
        }

        public static explicit operator DashboardItem(DashItemScrap v) // Casting
        {
            return new DashboardItem()
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
        public static explicit operator DashboardItem(DashItemUpload v)
        {
            return new DashboardItem()
            {
                DashboardID = v.DashboardID,
                DashboardItemID = v.DashboardItemID,
                DashboardItemStatusID = v.DashboardItemStatusID,
                DashboardTypeID = v.DashboardTypeID,
                EndDateTime = v.EndDateTime,
                SortOrder = v.SortOrder,
                StartDateTime = v.StartDateTime,
                Title = v.Title,

                ImageURI = v.ImageURI

            };

        }

        public static explicit operator DashboardItem(DashItemText v)
        {
            return new DashboardItem
            {
                DashboardID = v.DashboardID,
                DashboardItemID = v.DashboardItemID,
                DashboardItemStatusID = v.DashboardItemStatusID,
                DashboardTypeID = v.DashboardTypeID,
                EndDateTime = v.EndDateTime,
                SortOrder = v.SortOrder,
                StartDateTime = v.StartDateTime,
                Title = v.Title,

                DisplayText = v.DisplayText
            };

        }

    }
}
