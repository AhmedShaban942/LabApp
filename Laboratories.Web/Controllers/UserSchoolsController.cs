using Laboratories.Application.Contracts;
using Laboratories.Domain;
using Laboratories.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Laboratories.Web.Controllers
{
    [PermClass]
    public class UserSchoolsController : Controller
    {


            private IRepository<User> _userRepository;

            private IRepository<School> _schoolRepository;
        private IRepository<UserSchools> _userSchoolsRepository;
        public UserSchoolsController(IRepository<User> userRepository, IRepository<School> schoolRepository, IRepository<UserSchools> userSchoolsRepository)
            {
                this._userRepository = userRepository;
                this._schoolRepository = schoolRepository;
                this._userSchoolsRepository = userSchoolsRepository;
            }

            // GET: ScreenRoles.
            [ScreenPermissionFilter(screenId =3)]
            public ActionResult Index()
            {
                return View(_userRepository.GetAll());

            }


            // GET: ScreenRoles/Create
            [ScreenPermissionFilter(screenId = 3)]
            public ActionResult Create()
            {

                ViewBag.user_id = new SelectList(_userRepository.GetAll(), "Id", "usr_arName");
                ViewBag.school_id = new SelectList(_schoolRepository.GetAll(), "Id", "sch_arName");
                return View();
            }

        // POST: ScreenRoles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        public JsonResult SaveUserSchools([FromBody] int[] userSchools,[FromUri]int user_id)
        {
            try
            {
                List<UserSchools> _userSchools= new List<UserSchools>();
                UserSchools userSchool = new UserSchools();
                if (user_id != null)
                {
                    ICollection<UserSchools> old_userSchools = _userSchoolsRepository.MultiSearch(s => s.User_Id == user_id).ToList();
                    foreach (var item in old_userSchools)
                    {
                        _userSchoolsRepository.Delete(item.Id);
                    }
                    foreach (var item in userSchools)
                    {
                        userSchool.School_Id = item;
                        userSchool.User_Id = user_id;
                        _userSchoolsRepository.Insert(userSchool);
                    }
                    return Json("تم اضافة   المدارس للمستخدم", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }

        }



    }
}
