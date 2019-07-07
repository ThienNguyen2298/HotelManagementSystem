using HMS.Data;
using HMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Services
{
    public class AccomodationPackagesService
    {
        //Display list Accomodation Packages
        public IEnumerable<AccomodationPackage> GetAllAccomodationPackages()
        {
            var context = new HMSContext();
            return context.AccomodationPackages.ToList();
        }
        //Display after searched
        public IEnumerable<AccomodationPackage> SearchAccomodationPackages(string searchTerm)
        {
            var context = new HMSContext();
            var accomodationPackages = context.AccomodationPackages.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                accomodationPackages = accomodationPackages.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()));
            }
            return accomodationPackages.ToList();
        }
        public AccomodationPackage GetAccomodationPackageByID(int ID)
        {
            var context = new HMSContext();
            return context.AccomodationPackages.Find(ID);
        }
        //Function create
        public bool SaveAccomodationPackage(AccomodationPackage accomodationPackage)
        {
            var context = new HMSContext();
            context.AccomodationPackages.Add(accomodationPackage);
            return context.SaveChanges() > 0;
        }
        //Function edit
        public bool UpdateAccomodationPackage(AccomodationPackage accomodationPackage)
        {
            var context = new HMSContext();
            context.Entry(accomodationPackage).State = System.Data.Entity.EntityState.Modified;
            return context.SaveChanges() > 0;
        }
        //Function delete
        public bool DeleteAccomodationPackage(AccomodationPackage accomodationPackage)
        {
            var context = new HMSContext();
            context.Entry(accomodationPackage).State = System.Data.Entity.EntityState.Deleted;
            return context.SaveChanges() > 0;
        }
    }
}
