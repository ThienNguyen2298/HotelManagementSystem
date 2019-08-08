using HMS.Areas.Dashboard.ViewModels;

using HMS.Entities;
using HMS.Services;
using HMS.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HMS.Areas.Dashboard.Controllers
{
    public class UsersController : Controller
    {
   
        //

        private HMSSignInManager _signInManager;
        private HMSUserManager _userManager;

        public UsersController()
        {
        }

        public UsersController(HMSUserManager userManager, HMSSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public HMSSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<HMSSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public HMSUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<HMSUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Dashboard/Users
        AccomodationPackagesService accomodationPackagesService = new AccomodationPackagesService();
        AccomodationsService accomodationsService = new AccomodationsService();
        // GET: Dashboard/Accomodations
        public ActionResult Index(string searchTerm, string roleID, int? page)
        {
            int recordSize = 1;
            page = page ?? 1;
            //var abc = _signInManager.AuthenticationManager;
            UsersListingModels model = new UsersListingModels();
            model.SearchTerm = searchTerm;

            model.RoleID = roleID;
            //model.Roles = accomodationPackagesService.GetAllAccomodationPackages();

            model.Users = SearchUsers(searchTerm,page.Value,recordSize);



            var totalRecords = SearchUsersCount(searchTerm);
            //thuộc tính phân trang
            model.Pager = new Pager(totalRecords, page, recordSize);

            return View(model);
        }
        public IEnumerable<HMSUser> SearchUsers(string searchTerm, int page, int recordSize)
        {
            

            var users = UserManager.Users.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                users = users.Where(x => x.Email.ToLower().Contains(searchTerm.ToLower()));
            }
            

            var skip = (page - 1) * recordSize;
            // skip  = (1    - 1) * 3 = 0 * 3 = 0
            // skip  = (2    - 1) * 3 = 1 * 3 = 3
            // skip  = (3    - 1) * 3 = 2 * 3 = 6

            return users.OrderBy(x => x.Email).Skip(skip).Take(recordSize).ToList();
        }
        public int SearchUsersCount(string searchTerm)
        {


            var users = UserManager.Users.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                users = users.Where(x => x.Email.ToLower().Contains(searchTerm.ToLower()));
            }
            

            return users.Count();
        }
        [HttpGet]
        //ID được gán kiểu nullable - HttpGet là click vào lấy dữ liệu
        public async Task<ActionResult> Action(string ID)
        {
            UserActionModels model = new UserActionModels();

            if (!string.IsNullOrEmpty(ID))//we are trying to edit a record
            {
                var user = await UserManager.FindByIdAsync(ID);
                model.ID = user.Id;
                model.FullName = user.FullName;
                model.Email = user.Email;
                model.Username = user.UserName;
                model.Country = user.Country;
                model.City = user.City;
                model.Address = user.Address;

            }
            
            return PartialView("_Action", model);
        }
        [HttpPost]
        //Handle Create & Edit -
        public async Task<JsonResult> Action(UserActionModels model)
        {
            JsonResult json = new JsonResult();
            IdentityResult result = null;
            if (!string.IsNullOrEmpty(model.ID))//we are trying to edit a record
            {
                var user = await UserManager.FindByIdAsync(model.ID);

                //user.Id = model.ID;
                user.FullName = model.FullName;
                user.Email = model.Email;
                user.UserName = model.Username;

                user.Country = model.Country;
                user.City = model.City;
                user.Address = model.Address;


                result = await UserManager.UpdateAsync(user);
                
            }
            else    //we are trying to create a record
            {
                var user = new HMSUser();

                //user.Id = model.ID;
                user.FullName = model.FullName;
                user.Email = model.Email;
                user.UserName = model.Username;

                user.Country = model.Country;
                user.City = model.City;
                user.Address = model.Address;


                result = await UserManager.CreateAsync(user);
            }

            json.Data = new { Success = false, Message = string.Join(", ",result.Errors) };

            return json;
        }
        [HttpGet]
        public async Task<ActionResult> Delete(string ID)
        {
            UserActionModels model = new UserActionModels();
            var user = await UserManager.FindByIdAsync(ID);
            model.ID = user.Id;



            //else//we are trying to create a record
            //{

            //}
            return PartialView("_Delete", model);
        }
        [HttpPost]
        public async Task<JsonResult> Delete(UserActionModels model)
        {
            JsonResult json = new JsonResult();
            

            IdentityResult result = null;
            if (!string.IsNullOrEmpty(model.ID))//we are trying to delete a record
            {
                var user = await UserManager.FindByIdAsync(model.ID);

                result = await UserManager.DeleteAsync(user);
                json.Data = new { Success = result.Succeeded, Message = string.Join(", ", result.Errors) };
            }
            else
            {
                json.Data = new { Success = result.Succeeded, Message = "Invalid User" };
            }
            
            return json;
        }
    }
}