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
using PagedList;
using static Laboratories.Domain.User;

namespace Laboratories.Web.Controllers
{
    [PermClass]
    public class UnitsController : Controller
    {
        private IRepository<Unit> _unitRepository;
        public UnitsController(IRepository<Unit> unitRepository)
        {
            this._unitRepository = unitRepository;

        }
        // GET: Units
        [ScreenPermissionFilter(screenId = 27)]
        public ActionResult Index(int? page)
        {
            try
            {
                if (Session["UserInfo"] != null)
                {
                    var ModelList = _unitRepository.GetAll().OrderBy(x => x.Id);
                    int pageSize = 20;
                    //set page to one is there is no value, ??  is called the null-coalescing operator.
                    int pageNumber = (page ?? 1);
                    //return the Model data with paged
                    return View(ModelList.ToPagedList(pageNumber, pageSize));
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

        // GET: Units/Details/5
        [ScreenPermissionFilter(screenId = 28)]
        public ActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Unit unit = _unitRepository.GetByNum(id);
                if (unit == null)
                {
                    return HttpNotFound();
                }
                return View(unit);
            }
            catch (Exception)
            {

                return RedirectToAction("Index");
            }

        }

        // GET: Units/Create
        [ScreenPermissionFilter(screenId = 29)]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Units/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,unt_arName,unt_enName,AddedDate,ModifiedDate,AddedBy,ModifiedBy,IsAvtive")] Unit unit)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    unit.AddedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd"));
                    if ((User)Session["UserInfo"] != null)
                    {
                        User oUser = (User)Session["UserInfo"];
                        unit.AddedBy = oUser.Id;
                    }
                    else
                    {
                        return RedirectToAction("Login", "Account");
                    }
                    _unitRepository.Insert(unit);
                    return RedirectToAction("Index");
                }

                return View(unit);
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }

        }

        // GET: Units/Edit/5
        [ScreenPermissionFilter(screenId = 30)]
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Unit unit = _unitRepository.GetByNum(id);
                if (unit == null)
                {
                    return HttpNotFound();
                }
                return View(unit);
            }
            catch (Exception)
            {

                return RedirectToAction("Index");
            }

        }

        // POST: Units/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,unt_arName,unt_enName,AddedDate,ModifiedDate,AddedBy,ModifiedBy,IsAvtive")] Unit unit)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    unit.ModifiedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd"));

                    if ((User)Session["UserInfo"] != null)
                    {
                        User oUser = (User)Session["UserInfo"];
                        unit.ModifiedBy = oUser.Id;
                    }
                    else
                    {
                        return RedirectToAction("Login", "Account");
                    }
                    _unitRepository.Update(unit);
                    return RedirectToAction("Index");
                }
                return View(unit);
            }
            catch (Exception)
            {

                return RedirectToAction("Index");
            }

        }

        // GET: Units/Delete/5
        [ScreenPermissionFilter(screenId = 31)]
        public ActionResult Delete(int? id)
        {

            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Unit unit = _unitRepository.GetByNum(id);
                if (unit == null)
                {
                    return HttpNotFound();
                }
                return View(unit);

            }
            catch (Exception)
            {

                return RedirectToAction("Index");
            }
        }

        // POST: Units/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            try
            {
                User oUser = (User)Session["UserInfo"];
                //if (oUser.usr_role == Role.مدير_نظام)
                //{
                    _unitRepository.Delete(id);
                    return RedirectToAction("Index");
                //}
                //else
                //    return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                var x = ex.Message;
                return RedirectToAction("Index");
            }
        }


    }
}
