using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntranetApplication.Models.DashboardItem
{
    public class DashItemUpload : DashItemBase
    {
        public string ImageURI { get; set; }

        public static explicit operator DashItemUpload(DashboardItem v)
        {
            return new DashItemUpload()
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
    }

}
