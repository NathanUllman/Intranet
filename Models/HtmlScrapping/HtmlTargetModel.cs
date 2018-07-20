using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;

namespace IntranetApplication.Models.HtmlScrapping
{
    public class HtmlTarget
    {
        public int ID { get; set; }
        public string CssSelector { get; set; }
        public string Url { get; set; }
        public string StorageLocation { get; set; }
        public int? CroppingHeight { get; set; }
        public int? CroppingWidth { get; set; }


        // ToDo: check for security risk with these values
        public string Username { get; set; }
        public string Password { get; set; }


    }
}
