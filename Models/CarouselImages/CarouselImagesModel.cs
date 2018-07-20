using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntranetApplication.Models.CarouselImages
{
    public class CarouselImage
    {
        public int ID { get; set; }
        public string DisplayName { get; set; }
        public string ImageLocation { get; set; }
        public bool IsEditable { get; set; }
    }
}
