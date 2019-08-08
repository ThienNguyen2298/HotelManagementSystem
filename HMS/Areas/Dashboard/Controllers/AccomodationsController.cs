using HMS.Areas.Dashboard.ViewModels;
using HMS.Entities;
using HMS.Services;
using HMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HMS.Areas.Dashboard.Controllers
{
    public class AccomodationsController : Controller
    {
        AccomodationPackagesService accomodationPackagesService = new AccomodationPackagesService();
        AccomodationsService accomodationsService = new AccomodationsService();
        // GET: Dashboard/Accomodations
        public ActionResult Index(string searchTerm, int? accomodationPackageID, int? page)
        {
            int recordSize = 3;
            page = page ?? 1;
            AccomodationsListingModels model = new AccomodationsListingModels();
            model.SearchTerm = searchTerm;
            model.Accomodations = accomodationsService.SearchAccomodations(searchTerm, accomodationPackageID, page.Value, recordSize);

            model.AccomodationPackageID = accomodationPackageID;
            model.AccomodationPackages = accomodationPackagesService.GetAllAccomodationPackages();

            var totalRecords = accomodationsService.SearchAccomodationsCount(searchTerm, accomodationPackageID);
            //thuộc tính phân trang
            model.Pager = new Pager(totalRecords, page, recordSize);

            return View(model);
        }
        [HttpGet]
        //ID được gán kiểu nullable - HttpGet là click vào lấy dữ liệu
        public ActionResult Action(int? ID)
        {
            AccomodationsActionModels model = new AccomodationsActionModels();

            if (ID.HasValue)//we are trying to edit a record
            {
                var accomodation = accomodationsService.GetAccomodationByID(ID.Value);
                model.ID = accomodation.ID;
                model.AccomodationPackageID = accomodation.AccomodationPackageID;
                model.Name = accomodation.Name;
                model.Description = accomodation.Description;
                
            }
            //else//we are trying to create a record
            //{
            model.AccomodationPackages = accomodationPackagesService.GetAllAccomodationPackages();
            //}
            return PartialView("_Action", model);
        }
        [HttpPost]
        //Handle Create & Edit -
        public JsonResult Action(AccomodationsActionModels model)
        {
            JsonResult json = new JsonResult();
            var result = false;
            if (model.ID > 0)//we are trying to edit a record
            {
                var accomodation = accomodationsService.GetAccomodationByID(model.ID);

                accomodation.AccomodationPackageID = model.AccomodationPackageID;
                accomodation.Name = model.Name;
                accomodation.Description = model.Description;
                

                result = accomodationsService.UpdateAccomodation(accomodation);
            } 
            else    //we are trying to create a record
            {
                Accomodation accomodation = new Accomodation();
                accomodation.Name = model.Name;
                accomodation.AccomodationPackageID = model.AccomodationPackageID;
                //accomodationPackage.AccomodationType = accomodationTypesService.GetAccomodationTypeByID(model.AccomodationTypeID);
                accomodation.Description = model.Description;

                result = accomodationsService.SaveAccomodation(accomodation);
            }

            if (result)
            {
                json.Data = new { Success = true };
            }
            else
            {
                json.Data = new { Success = false, Message = "Unable to perform action on Accomodations" };
            }

            return json;
        }
        [HttpGet]
        public ActionResult Delete(int ID)
        {
            AccomodationsActionModels model = new AccomodationsActionModels();
            var accomodation = accomodationsService.GetAccomodationByID(ID);
            model.ID = accomodation.ID;


            //else//we are trying to create a record
            //{

            //}
            return PartialView("_Delete", model);
        }
        [HttpPost]
        public JsonResult Delete(AccomodationPackagesActionModels model)
        {
            JsonResult json = new JsonResult();
            var result = false;

            var accomodationPackage = accomodationPackagesService.GetAccomodationPackageByID(model.ID);

            result = accomodationPackagesService.DeleteAccomodationPackage(accomodationPackage);

            if (result)
            {
                json.Data = new { Success = true };
            }
            else
            {
                json.Data = new { Success = false, Message = "Unable to perform action on Accomodations" };
            }

            return json;
        }
    }
}
