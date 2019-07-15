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
    public class AccomodationPackagesController : Controller
    {
        AccomodationPackagesService accomodationPackagesService = new AccomodationPackagesService();
        AccomodationTypesService accomodationTypesService = new AccomodationTypesService();
        // GET: Dashboard/AccomodationPackages
        public ActionResult Index(string searchTerm, int? accomodationTypeID, int? page)
        {
            int recordSize = 3;
            page = page ?? 1;
            AccomodationPackagesListingModels model = new AccomodationPackagesListingModels();
            model.SearchTerm = searchTerm;
            model.AccomodationPackages = accomodationPackagesService.SearchAccomodationPackages(searchTerm, accomodationTypeID, page.Value, recordSize);

            model.AccomodationTypeID = accomodationTypeID;
            model.AccomodationTypes = accomodationTypesService.GetAllAccomodationTypes();

            var totalRecords = accomodationPackagesService.SearchAccomodationPackagesCount(searchTerm, accomodationTypeID);
            //thuộc tính phân trang
            model.Pager = new Pager(totalRecords, page, recordSize);

            return View(model);
        }
        [HttpGet]
        //ID được gán kiểu nullable - HttpGet là click vào lấy dữ liệu
        public ActionResult Action(int? ID)
        {
            AccomodationPackagesActionModels model = new AccomodationPackagesActionModels();
            
            if (ID.HasValue)//we are trying to edit a record
            {
                var accomodationPackage = accomodationPackagesService.GetAccomodationPackageByID(ID.Value);
                model.ID = accomodationPackage.ID;
                model.AccomodationTypeID = accomodationPackage.AccomodationTypeID;
                model.Name = accomodationPackage.Name;
                model.NoOfRoom = accomodationPackage.NoOfRoom;
                model.FeePerNight = accomodationPackage.FeePerNight;
            }
            //else//we are trying to create a record
            //{
            model.AccomodationTypes = accomodationTypesService.GetAllAccomodationTypes();
            //}
            return PartialView("_Action", model);
        }
        [HttpPost]
        //Handle Create & Edit
        public JsonResult Action(AccomodationPackagesActionModels model)
        {
            JsonResult json = new JsonResult();
            var result = false;
            if (model.ID > 0)//we are trying to edit a record
            {
                var accomodationPackage = accomodationPackagesService.GetAccomodationPackageByID(model.ID);

                accomodationPackage.AccomodationTypeID = model.AccomodationTypeID;
                accomodationPackage.Name = model.Name;
                accomodationPackage.NoOfRoom = model.NoOfRoom;
                accomodationPackage.FeePerNight = model.FeePerNight;

                result = accomodationPackagesService.UpdateAccomodationPackage(accomodationPackage);
            }
            else    //we are trying to create a record
            {
                AccomodationPackage accomodationPackage = new AccomodationPackage();
                accomodationPackage.Name = model.Name;
                accomodationPackage.AccomodationTypeID = model.AccomodationTypeID;
                //accomodationPackage.AccomodationType = accomodationTypesService.GetAccomodationTypeByID(model.AccomodationTypeID);
                accomodationPackage.NoOfRoom = model.NoOfRoom;
                accomodationPackage.FeePerNight = model.FeePerNight;

                result = accomodationPackagesService.SaveAccomodationPackage(accomodationPackage);
            }

            if (result)
            {
                json.Data = new { Success = true };
            }
            else
            {
                json.Data = new { Success = false, Message = "Unable to perform action on Accomodation Packages" };
            }

            return json;
        }
        [HttpGet]
        public ActionResult Delete(int ID)
        {
            AccomodationPackagesActionModels model = new AccomodationPackagesActionModels();
            var accomodationPackage = accomodationPackagesService.GetAccomodationPackageByID(ID);
            model.ID = accomodationPackage.ID;


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
                json.Data = new { Success = false, Message = "Unable to perform action on Accomodation Packages" };
            }

            return json;
        }
    }
}