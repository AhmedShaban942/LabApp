using Laboratories.Application.Contracts;
using Laboratories.Domain;
using Laboratories.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Laboratories.Web.Controllers
{

    public class AccountController : Controller
    {
        private IUserRepository _userRepository;
        private IRepository<ScreenRole> _screenRoleRepository;
        private IRepository<UserSchools> _userSchoolsRepository;
        public AccountController(IUserRepository userRepository, IRepository<ScreenRole> screenRoleRepository, IRepository<UserSchools> userSchoolsRepository)
        {
            this._userRepository = userRepository;
            this._screenRoleRepository= screenRoleRepository;
            this._userSchoolsRepository = userSchoolsRepository;
        }

        // GET: User
      
        public ActionResult Index()
        {
            return View();

        }

        public ActionResult LogIn(string usr_arName, string usr_pass,int? yearData=2023)
        {
         
            try
            {
                User usr_login = new User();
                usr_login = _userRepository.SingleSearch(x=>(x.usr_arName== usr_arName ||x.usr_enName == usr_arName || x.usr_emial == usr_arName) & x.usr_pass== usr_pass);
                
                    if (usr_login == null || yearData<2022)
                {
                    ModelState.AddModelError(string.Empty, "invalid username or password or Year خطأ فى الادخال");
                    return View("Index");
                }
                else
                {
                    usr_login = _userRepository.GetUserWithSchoolById(usr_login.Id);
                    Session["UserInfo"] = usr_login;
                    Session["CurrentYear"] = yearData;
                    ScreenViewModel screenViewModel = new ScreenViewModel();
                    var Screenes =_screenRoleRepository.MultiSearch(s => s.role_id == usr_login.usr_roleId).ToList();
                    screenViewModel.ScreenesId = Screenes.Select(s => s.screen_id.Value).ToList();
                    if (usr_login.usr_roleId != null)
                    {
                        screenViewModel.role_id = usr_login.usr_roleId.Value;
                        Session["screenPremssion"] = screenViewModel;
                    }
                    else
                    {
                        Session["screenPremssion"] = null;
                        return RedirectToAction("Index", "Account");
                    }
                        

                   // Session["UserScoolId"] =_userSchoolsRepository.MultiSearch(u=>u.User_Id==usr_login.Id);
                   //Session["ScoolId"] = 1;
                   // Session["Department"] = SchoolDepartment.ابتدائى;
                   //var x = (SchoolDepartment)Session["Department"];
                    return RedirectToAction("SelectSchool", "Home");
                }
            }
            catch (System.Exception ex)
            {
                return RedirectToAction("Index", "Account");
            }
        }
    }
}