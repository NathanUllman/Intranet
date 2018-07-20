using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace IntranetApplication.Models.CarouselImages
{
    public class CarouselImagesContext : DbContext
    {
            public CarouselImagesContext(DbContextOptions<CarouselImagesContext> options)
                : base(options)
            {
            }

            public DbSet<CarouselImage> CarouselImages { get; set; }      
    }
}
