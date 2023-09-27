using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Laboratories.Application.Contracts;
using Laboratories.Domain;
using Laboratories.Persistence;
using Laboratories.Web.Models;
using static Laboratories.Domain.User;

namespace Laboratories.Web.Controllers
{
    [PermClass]
    public class AppSetingsController : Controller
    {

        private IRepository<AppSeting> _appSetingRepository;
        public AppSetingsController(IRepository<AppSeting> appSetingRepository)
        {
            this._appSetingRepository = appSetingRepository;

        }
        // GET: AppSetings
        public ActionResult Index(string pass)
        {
            if (pass=="1109")
            {
                if (Session["UserInfo"] != null)
                {

                    User oUser = (User)Session["UserInfo"];
                    //if (oUser.usr_role == Role.مدير_نظام)
                    //{
                        return View(_appSetingRepository.GetAll());
                    //}
                    //else
                    //    return RedirectToAction("Login", "Account");

                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
              
            }
            else
                return RedirectToAction("Login", "Account");
        }

        // GET: AppSetings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppSeting appSeting = _appSetingRepository.GetByNum(id);
            if (appSeting == null)
            {
                return HttpNotFound();
            }
            return View(appSeting);
        }

        // GET: AppSetings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AppSetings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,companyNumber,compelexNumber,schoolNumber,userNumber,AddedDate,ModifiedDate,AddedBy,ModifiedBy,IsAvtive")] AppSeting appSeting)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    appSeting.AddedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd"));
                    if ((User)Session["UserInfo"] != null)
                    {

                        User oUser = (User)Session["UserInfo"];
                        //if (oUser.usr_role == Role.مدير_نظام)
                        //{
                            appSeting.AddedBy = oUser.Id;
                        //}
                        //else
                        //    return RedirectToAction("Login", "Account");

                    }
                    else
                    {
                        return RedirectToAction("Login", "Account");
                    }

                    if (_appSetingRepository.GetAll().Count==0)
                    {
                        _appSetingRepository.Insert(appSeting);
                    }
              
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {

                    return View(appSeting);
                }

            }

            return View(appSeting);
        }

        // GET: AppSetings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppSeting appSeting = _appSetingRepository.GetByNum(id);
            if (appSeting == null)
            {
                return HttpNotFound();
            }
            return View(appSeting);
        }

        // POST: AppSetings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,companyNumber,compelexNumber,schoolNumber,userNumber,AddedDate,ModifiedDate,AddedBy,ModifiedBy,IsAvtive")] AppSeting appSeting)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    appSeting.ModifiedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd"));

                    if ((User)Session["UserInfo"] != null)
                    {
                        User oUser = (User)Session["UserInfo"];
                        //if (oUser.usr_role == Role.مدير_نظام)
                        //{
                            appSeting.ModifiedBy = oUser.Id;
                        //}
                        //else
                        //    return RedirectToAction("Login", "Account");

                    }
                    else
                    {
                        return RedirectToAction("Login", "Account");
                    }
                 
                    _appSetingRepository.Update(appSeting);
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {

                    return View(appSeting);
                }

            }
            return View(appSeting);
        }

        // GET: AppSetings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppSeting appSeting = _appSetingRepository.GetByNum(id);
            if (appSeting == null)
            {
                return HttpNotFound();
            }
            return View(appSeting);
        }

        // POST: AppSetings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User oUser = (User)Session["UserInfo"];
            //if (oUser.usr_role == Role.مدير_نظام)
            //{
                AppSeting appSeting = _appSetingRepository.GetByNum(id);
                if (_appSetingRepository.GetAll().Count>1)
                {
                    _appSetingRepository.Delete(id);
                }
         
                return RedirectToAction("Index");
            //}
            //else
            //    return RedirectToAction("Login", "Account");
        }

  
    }
}
