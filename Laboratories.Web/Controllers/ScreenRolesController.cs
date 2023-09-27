using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Laboratories.Application.Contracts;
using Laboratories.Domain;
using Laboratories.Persistence;
using Laboratories.Web.Models;

namespace Laboratories.Web.Controllers
{
    [PermClass]
    public class ScreenRolesController : Controller
    {

        private IRepository<UserRole> _userRoleRepository;

        private IRepository<Screen> _screenRepository;
        private IRepository<ScreenRole> _screenRolesRepository;
        public ScreenRolesController(IRepository<UserRole> userRoleRepository, IRepository<Screen> screenRepository, IRepository<ScreenRole> screenRolesRepository)
        {
            this._userRoleRepository = userRoleRepository;
            this._screenRepository = screenRepository;
            this._screenRolesRepository = screenRolesRepository;


        }

        // GET: ScreenRoles.
        [ScreenPermissionFilter(screenId = 94)]
        public ActionResult Index()
        {
            return View(_userRoleRepository.GetAll());

        }


        // GET: ScreenRoles/Create
        [ScreenPermissionFilter(screenId = 94)]
        public ActionResult Create()
        {
         
            ViewBag.role_id = new SelectList(_userRoleRepository.GetAll(), "role_id", "role_name");
            ViewBag.screen_id = new SelectList(_screenRepository.GetAll(), "screen_id", "screen_name");
            return View();
        }

        // POST: ScreenRoles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        public JsonResult SaveScreenRoles([FromBody] int[] screens,[FromUri]int role_id)
        {
            try
            {
                List<ScreenRole> screenRoles = new List<ScreenRole>();
                ScreenRole screenRole = new ScreenRole();
                if (role_id != null)
                {
                    ICollection<ScreenRole> old_roles = _screenRolesRepository.GetAll().Where(s => s.role_id == role_id).ToList();
                    foreach (var item in old_roles)
                    {
                        _screenRolesRepository.Delete(item.screen_role_id);
                    }
                    foreach (var item in screens)
                    {
                        screenRole.role_id = role_id;
                        screenRole.screen_id = item;
                        _screenRolesRepository.Insert(screenRole);
                    }
                  return Json("تم اضافة   الصلاحيات", JsonRequestBehavior.AllowGet);
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
