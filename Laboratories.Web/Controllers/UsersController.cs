using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Laboratories.Application.Contracts;
using Laboratories.Domain;
using Laboratories.Persistence;
using Laboratories.Web.Models;
using PagedList;
using static Laboratories.Domain.User;

namespace Laboratories.Web.Controllers
{
    [PermClass]
    public class UsersController : Controller
    {

        private IUserRepository _userRepository;
        private ISchoolRepository _schoolRepository;
        private IRepository<AppSeting> _appSetingRepository;
        private IRepository<UserRole> _userRoleRepository;
        public UsersController(IUserRepository userRepository, ISchoolRepository schoolRepository, IRepository<AppSeting> appSetingRepository, IRepository<UserRole> userRoleRepository)
        {

            this._userRepository = userRepository;
            this._schoolRepository = schoolRepository;
            this._appSetingRepository = appSetingRepository;
            this._userRoleRepository = userRoleRepository;
        }
        // GET: Users
        [ScreenPermissionFilter(screenId = 2)]
        public ActionResult Index(int? page)
        {
            try
            {
                if (Session["UserInfo"] != null)
                {
                    User oUser = (User)Session["UserInfo"];
                    //if (oUser.usr_role == Role.مدير_نظام)
                    //{
                        var ModelList = _userRepository.GetAllUserWithSchool().OrderBy(x => x.Id);
                        int pageSize = 20;
                        //set page to one is there is no value, ??  is called the null-coalescing operator.
                        int pageNumber = (page ?? 1);
                        //return the Model data with paged
                        return View(ModelList.ToPagedList(pageNumber, pageSize));
                    //}
                    //else
                    //    return RedirectToAction("Login", "Account");
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Home");
            }


        }

        // GET: Users/Details/5
        [ScreenPermissionFilter(screenId = 3)]
        public ActionResult Details(int id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                User user = _userRepository.GetUserWithSchoolById(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user);
            }
            catch (Exception)
            {

                return RedirectToAction("Index");
            }

        }

        // GET: Users/Create
        [ScreenPermissionFilter(screenId = 4)]
        public ActionResult Create()
        {
            try
            {
                ViewBag.usr_schId = new SelectList(_schoolRepository.GetAll(), "Id", "sch_arName");
                ViewBag.usr_roleId = new SelectList(_userRoleRepository.GetAll(), "role_id", "role_name");
                return View();
            }
            catch (Exception)
            {


                return RedirectToAction("Index");
            }

        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,usr_arName,usr_enName,usr_Role,usr_pass,usr_schId,usr_roleId")] User user, HttpPostedFileBase usr_image)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (usr_image != null)
                    {
                        byte[] bytes = null;
                        using (var binaryReader = new BinaryReader(usr_image.InputStream))
                        {
                            bytes = binaryReader.ReadBytes(usr_image.ContentLength);
                        }
                        user.usr_image = bytes;
                    }
                    if (_appSetingRepository.GetAll().Count > 0)
                    {
                        if (_appSetingRepository.GetAll().FirstOrDefault().userNumber > _userRepository.GetAll().Count)
                        {

                            if (user.usr_arName == "مدير النظام")
                            {
                                return RedirectToAction("Index", "Home");

                            }
                            if (string.IsNullOrEmpty(user.usr_pass) || string.IsNullOrEmpty(user.usr_arName))
                            {
                                return RedirectToAction("Index");
                            }
                            _userRepository.Insert(user);
                        }
                    }

                    return RedirectToAction("Index");
                }

                return View(user);
            }
            catch (Exception)
            {


                return RedirectToAction("Index");
            }

        }

        // GET: Users/Edit/5
        [ScreenPermissionFilter(screenId = 5)]
        public ActionResult Edit(int? id)
        {
            try
            {
                User oUser = (User)Session["UserInfo"];
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                User user = _userRepository.GetByNum(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                if (user.usr_arName == "مدير النظام" && oUser.usr_arName != "مدير النظام")
                {
                    return RedirectToAction("Index", "Home");

                }
                ViewBag.usr_schId = new SelectList(_schoolRepository.GetAll(), "Id", "sch_arName", (int)Session["ScoolId"]);
                ViewBag.usr_roleId = new SelectList(_userRoleRepository.GetAll(), "role_id", "role_name",user.usr_roleId);
                return View(user);
            }
            catch (Exception)
            {

                return RedirectToAction("Index");
            }

        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,usr_arName,usr_enName,usr_roleId,usr_pass,usr_schId,usr_image")] User user, HttpPostedFileBase usr_image)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (usr_image != null)
                    {
                        byte[] bytes = null;
                        using (var binaryReader = new BinaryReader(usr_image.InputStream))
                        {
                            bytes = binaryReader.ReadBytes(usr_image.ContentLength);
                        }
                        user.usr_image = bytes;
                    }
                    if (string.IsNullOrEmpty(user.usr_pass) || string.IsNullOrEmpty(user.usr_arName))
                    {
                        return RedirectToAction("Index");
                    }
                    _userRepository.Update(user);
                    return RedirectToAction("Index");
                }


                ViewBag.usr_schId = new SelectList(_schoolRepository.GetAll(), "Id", "sch_arName");
                ViewBag.usr_roleId = new SelectList(_userRoleRepository.GetAll(), "role_id", "role_name");
                return View(user);
            }
            catch (Exception)
            {


                return RedirectToAction("Index");
            }

        }

        // GET: Users/Delete/5
        [ScreenPermissionFilter(screenId = 6)]
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                User user = _userRepository.GetByNum(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                User oUser = (User)Session["UserInfo"];
                if (user.usr_arName == "مدير النظام" && oUser.usr_arName != "مدير النظام")
                {
                    return RedirectToAction("Index", "Home");

                }
                ViewBag.usr_schId = new SelectList(_schoolRepository.GetAll(), "Id", "sch_arName");
                ViewBag.usr_roleId = new SelectList(_userRoleRepository.GetAll(), "role_id", "role_name");
                return View(user);
            }
            catch (Exception)
            {


                return RedirectToAction("Index");
            }

        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                _userRepository.Delete(id);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                return RedirectToAction("Index");
            }

        }


    }
}
