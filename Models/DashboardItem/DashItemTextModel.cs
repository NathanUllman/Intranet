using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace IntranetApplication.Models.DashboardItem
{
    public class DashItemText : DashItemBase
    {
        public string DisplayText { get; set; }

        public static explicit operator DashItemText(DashboardItem v)
        {
            return new DashItemText
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
