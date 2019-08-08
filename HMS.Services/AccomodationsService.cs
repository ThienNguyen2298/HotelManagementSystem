using HMS.Data;
using HMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Services
{
    public class AccomodationsService
    {
        //Display list Accomodation s
        public IEnumerable<Accomodation> GetAllAccomodations()
        {
            var context = new HMSContext();
            return context.Accomodations.ToList();
        }
        //Display after searched
        public IEnumerable<Accomodation> SearchAccomodations(string searchTerm, int? accomodationPackageID, int page, int recordSize)
        {
            var context = new HMSContext();

            var accomodations = context.Accomodations.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                accomodations = accomodations.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()));
            }
            if (accomodationPackageID.HasValue && accomodationPackageID.Value > 0)
            {
                accomodations = accomodations.Where(x => x.AccomodationPackageID == accomodationPackageID.Value);
            }

            var skip = (page - 1) * recordSize;
            // skip  = (1    - 1) * 3 = 0 * 3 = 0
            // skip  = (2    - 1) * 3 = 1 * 3 = 3
            // skip  = (3    - 1) * 3 = 2 * 3 = 6

            return accomodations.OrderBy(x => x.AccomodationPackageID).Skip(skip).Take(recordSize).ToList();
        }
        public int SearchAccomodationsCount(string searchTerm, int? accomodationPackageID)
        {
            var context = new HMSContext();

            var accomodations = context.Accomodations.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                accomodations = accomodations.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()));
            }
            if (accomodationPackageID.HasValue && accomodationPackageID.Value > 0)
            {
                accomodations = accomodations.Where(x => x.AccomodationPackageID == accomodationPackageID.Value);
            }



            return accomodations.Count();
        }
        public Accomodation GetAccomodationByID(int ID)
        {
            using (var context = new HMSContext())
            {
                return context.Accomodations.Find(ID);
            }
        }
        //Function create
        public bool SaveAccomodation(Accomodation accomodation)
        {
            var context = new HMSContext();
            context.Accomodations.Add(accomodation);
            return context.SaveChanges() > 0;
        }
        //Function edit
        public bool UpdateAccomodation(Accomodation accomodation)
        {

            var context = new HMSContext();
            context.Entry(accomodation).State = System.Data.Entity.EntityState.Modified;
            return context.SaveChanges() > 0;
        }
        //Function delete
        public bool DeleteAccomodation(Accomodation accomodation)
        {
            var context = new HMSContext();
            context.Entry(accomodation).State = System.Data.Entity.EntityState.Deleted;
            return context.SaveChanges() > 0;
        }
    }
}
