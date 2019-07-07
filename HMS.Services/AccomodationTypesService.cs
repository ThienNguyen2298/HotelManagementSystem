﻿using HMS.Data;
using HMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Services
{
    public class AccomodationTypesService
    {
        //Display list Accomodation Types
        public IEnumerable<AccomodationType> GetAllAccomodationTypes()
        {
            var context = new HMSContext();
            return context.AccomodationTypes.ToList();
        }
        //Display after searched
        public IEnumerable<AccomodationType> SearchAccomodationTypes(string searchTerm)
        {
            var context = new HMSContext();
            var accomodationTypes = context.AccomodationTypes.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                accomodationTypes = accomodationTypes.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()));
            }
            return accomodationTypes.ToList();
        }
        public AccomodationType GetAccomodationTypeByID(int ID)
        {
            var context = new HMSContext();
            return context.AccomodationTypes.Find(ID);
        }
        //Function create
        public bool SaveAccomodationType(AccomodationType accomodationType)
        {
            var context = new HMSContext();
            context.AccomodationTypes.Add(accomodationType);
            return context.SaveChanges() > 0;
        }
        //Function edit
        public bool UpdateAccomodationType(AccomodationType accomodationType)
        {
            var context = new HMSContext();
            context.Entry(accomodationType).State = System.Data.Entity.EntityState.Modified;
            return context.SaveChanges() > 0;
        }
        //Function delete
        public bool DeleteAccomodationType(AccomodationType accomodationType)
        {
            var context = new HMSContext();
            context.Entry(accomodationType).State = System.Data.Entity.EntityState.Deleted;
            return context.SaveChanges() > 0;
        }
    }
}
