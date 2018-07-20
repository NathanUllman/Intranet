using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace IntranetApplication.Models.NewHire
{
    public class NewHireContext : DbContext
    {
        public NewHireContext(DbContextOptions<NewHireContext> options)
            : base(options)
        {
        }
        public DbSet<NewHire> NewHires { get; set; }
    }
}