using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HMS.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace HMS.Data
{
    public class HMSContext : IdentityDbContext<HMSUser>
    {
        public HMSContext() : base("HMSConnection")
        {
        }
        public static HMSContext Create()
        {
            return new HMSContext();
        }
        public DbSet<Accomodation> Accomodations { get; set; }
        public DbSet<AccomodationType> AccomodationTypes { get; set; }
        public DbSet<AccomodationPackage> AccomodationPackages { get; set; }
        public DbSet<Booking> Bookings { get; set; }
    }
}
