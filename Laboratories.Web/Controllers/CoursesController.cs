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
    public class CoursesController : Controller
    {
        private IRepository<Course> _courseRepository;

        public CoursesController(IRepository<Course> courseRepository)
        {
            this._courseRepository = courseRepository;
        }
        // GET: Courses
        [ScreenPermissionFilter(screenId =32)]
        public ActionResult Index(int? page)
        {
            try
            {
                if (Session["UserInfo"] != null)
                {
                    var ModelList = _courseRepository.GetAll().OrderBy(x => x.Id);
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

        // GET: Courses/Details/5
        [ScreenPermissionFilter(screenId = 33)]
        public ActionResult Details(int? id)
        {
            try
            {
                if (Session["UserInfo"] != null)
                {

                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    Course course = _courseRepository.GetByNum(id);
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

        // GET: Courses/Create
        [ScreenPermissionFilter(screenId = 34)]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,cors_arName,cors_enName,cors_department,cors_exprNum,cors_term,cors_level")] Course course)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    if (Session["UserInfo"] != null)
                    {
                        _courseRepository.Insert(course);
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

        // GET: Courses/Edit/5
        [ScreenPermissionFilter(screenId = 35)]
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Course course = _courseRepository.GetByNum(id);
                if (course == null)
                {
                    return HttpNotFound();
                }
                return View(course);
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Courses");
            }

        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,cors_arName,cors_enName,cors_department,cors_exprNum,cors_term,cors_level")] Course course)
        {
            try
            {
                if (Session["UserInfo"] != null)
                {

                    if (ModelState.IsValid)
                    {
                        _courseRepository.Update(course);

                        return RedirectToAction("Index");
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

        // GET: Courses/Delete/5
        [ScreenPermissionFilter(screenId = 36)]
        public ActionResult Delete(int? id)
        {
            try
            {
                if (Session["UserInfo"] != null)
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    Course course = _courseRepository.GetByNum(id);
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

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                if (Session["UserInfo"] != null)
                {
                    _courseRepository.Delete(id);
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
