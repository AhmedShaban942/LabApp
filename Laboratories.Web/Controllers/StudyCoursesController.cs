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
    public class StudyCoursesController : Controller
    {
        private IRepository<StudyCourse> _studyCourseRepository;
        public StudyCoursesController(IRepository<StudyCourse> studyCourseRepository)
        {
            this._studyCourseRepository = studyCourseRepository;
        }
        // GET: StudyCourses
        [ScreenPermissionFilter(screenId = 106)]
        public ActionResult Index(int? page)
        {
            try
            {
                if (Session["UserInfo"] != null)
                {
                    var ModelList = _studyCourseRepository.GetAll().OrderBy(x => x.Id);
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

        // GET: StudyCourses/Details/5
        [ScreenPermissionFilter(screenId = 107)]
        public ActionResult Details(int id)
        {
            try
            {
                if (Session["UserInfo"] != null)
                {

                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    StudyCourse course = _studyCourseRepository.GetByNum(id);
                    if (course == null)
                    {
                        return HttpNotFound();
                    }
                    return View(course);

                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
        }

        // GET: StudyCourses/Create3.
        [ScreenPermissionFilter(screenId = 108)]
        public ActionResult Create()
        {
            return View();
        }

        // POST: StudyCourses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,cors_arName,cors_enName,cors_department,cors_level,cors_term,cors_degree,cors_rate")] StudyCourse studyCourse)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    if (Session["UserInfo"] != null)
                    {
                        _studyCourseRepository.Insert(studyCourse);
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        return RedirectToAction("Login", "Account");
                    }

                }
                else
                    return RedirectToAction("Index");



            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
        }

        // GET: StudyCourses/Edit/5
        [ScreenPermissionFilter(screenId = 109)]
        public ActionResult Edit(int id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                StudyCourse course = _studyCourseRepository.GetByNum(id);
                if (course == null)
                {
                    return HttpNotFound();
                }
                return View(course);
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "StudyCourses");
            }
        }

        // POST: StudyCourses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,cors_arName,cors_enName,cors_department,cors_level,cors_term,cors_degree,cors_rate")] StudyCourse studyCourse)
        {
            try
            {
                if (Session["UserInfo"] != null)
                {

                    if (ModelState.IsValid)
                    {
                        _studyCourseRepository.Update(studyCourse);

                        return RedirectToAction("Index");
                    }
                    return View(studyCourse);
                }
                else
                    return RedirectToAction("Login", "Account");
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
        }

        // GET: StudyCourses/Delete/5
        [ScreenPermissionFilter(screenId = 110)]
        public ActionResult Delete(int id)
        {
            try
            {
                if (Session["UserInfo"] != null)
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    StudyCourse course = _studyCourseRepository.GetByNum(id);
                    if (course == null)
                    {
                        return HttpNotFound();
                    }
                    return View(course);
                }
                else
                    return RedirectToAction("Login", "Account");
            }
            catch (Exception)
            {

                return RedirectToAction("Index");
            }
        }

        // POST: StudyCourses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                if (Session["UserInfo"] != null)
                {
                    _studyCourseRepository.Delete(id);
                    return RedirectToAction("Index");
                }
                else
                    return RedirectToAction("Login", "Account");
            }
            catch (Exception)
            {

                return RedirectToAction("Index");
            }
        }

 
    }
}
