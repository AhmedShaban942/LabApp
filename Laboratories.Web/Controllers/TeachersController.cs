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

namespace Laboratories.Web.Controllers
{
    [PermClass]
    public class TeachersController : Controller
    {

        private IRepository<Teacher> _teacherRepository;
        private ISchoolRepository _schoolRepository;
        public TeachersController(IRepository<Teacher> teacherRepository, ISchoolRepository schoolRepository)
        {
            this._teacherRepository = teacherRepository;
            this._schoolRepository = schoolRepository;
        }
        // GET: Teachers
        [ScreenPermissionFilter(screenId = 22)]
        public ActionResult Index(int? page)
        {
            try
            {
                if (Session["UserInfo"] != null)
                {
                    var ModelList = _teacherRepository.GetAll().OrderBy(x => x.Id);
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

        // GET: Teachers/Details/5
        [ScreenPermissionFilter(screenId = 23)]
        public ActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Teacher teacher = _teacherRepository.GetByNum(id);

                if (teacher == null)
                {
                    return HttpNotFound();
                }
                return View(teacher);
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }


        }

        // GET: Teachers/Create
        [ScreenPermissionFilter(screenId = 24)]
        public ActionResult Create()
        {
            try
            {
                ViewBag.tech_schId = new SelectList(_schoolRepository.GetAll(), "Id", "sch_arName");
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,tech_arName,tech_enName,tech_phon,tech_email,tech_address,AddedDate,ModifiedDate,AddedBy,ModifiedBy,IsAvtive,tech_schId")] Teacher teacher)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    _teacherRepository.Insert(teacher);
                    return RedirectToAction("Index");
                }

                return View(teacher);
            }
            catch (Exception)
            {

                return RedirectToAction("Index");
            }

        }

        // GET: Teachers/Edit/5
        [ScreenPermissionFilter(screenId = 25)]
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Teacher teacher = _teacherRepository.GetByNum(id);
                if (teacher == null)
                {
                    return HttpNotFound();
                }
                ViewBag.tech_schId = new SelectList(_schoolRepository.GetAll(), "Id", "sch_arName", teacher.tech_schId);
                return View(teacher);
            }
            catch (Exception)
            {

                return RedirectToAction("Index");
            }

        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,tech_arName,tech_enName,tech_phon,tech_email,tech_address,AddedDate,ModifiedDate,AddedBy,ModifiedBy,IsAvtive,tech_schId")] Teacher teacher)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _teacherRepository.Update(teacher);
                    return RedirectToAction("Index");
                }

                return View(teacher);
            }
            catch (Exception)
            {

                return RedirectToAction("Index");
            }

        }

        // GET: Teachers/Delete/5
        [ScreenPermissionFilter(screenId = 26)]
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Teacher teacher = _teacherRepository.GetByNum(id);
                if (teacher == null)
                {
                    return HttpNotFound();
                }
                return View(teacher);
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }


        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                _teacherRepository.Delete(id);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                return RedirectToAction("Index");
            }


        }


    }
}
