using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace IntranetApplication.Models.HtmlScrapping
{
    public class HtmlTargetContext : DbContext
    {
        public HtmlTargetContext(DbContextOptions<HtmlTargetContext> options)
            : base(options)
        {
        }

        public DbSet<HtmlTarget> HtmlTargets { get; set; }
    }
}

