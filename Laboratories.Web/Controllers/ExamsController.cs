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
    public class ExamsController : Controller
    {
        private IRepository<Exam> _examRepository;

        public ExamsController(IRepository<Exam> examRepository)
        {
            this._examRepository = examRepository;
        }
        // GET: Courses
        [ScreenPermissionFilter(screenId = 89)]
        public ActionResult Index(int? page)
        {
            try
            {
                if (Session["UserInfo"] != null)
                {
                    var ModelList = _examRepository.GetAll().OrderBy(x => x.Id);
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
        // GET: Exams/Details/5
        [ScreenPermissionFilter(screenId = 90)]
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
                    Exam exam = _examRepository.GetByNum(id);
                    if (exam == null)
                    {
                        return HttpNotFound();
                    }
                    return View(exam);

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

        // GET: Exams/Create
        [ScreenPermissionFilter(screenId = 91)]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Exams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,exm_arName,exm_enName")] Exam exam)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    if (Session["UserInfo"] != null)
                    {
                        _examRepository.Insert(exam);
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

        // GET: Exams/Edit/5
        [ScreenPermissionFilter(screenId = 92)]
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Exam exam = _examRepository.GetByNum(id);
                if (exam == null)
                {
                    return HttpNotFound();
                }
                return View(exam);
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Exams");
            }
        }

        // POST: Exams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,exm_arName,exm_enName")] Exam exam)
        {
            try
            {
                if (Session["UserInfo"] != null)
                {

                    if (ModelState.IsValid)
                    {
                        _examRepository.Update(exam);

                        return RedirectToAction("Index");
                    }
                    return View(exam);
                }
                else
                    return RedirectToAction("Login", "Account");
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
        }

        // GET: Exams/Delete/5
        [ScreenPermissionFilter(screenId = 93)]
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
                    Exam exam = _examRepository.GetByNum(id);
                    if (exam == null)
                    {
                        return HttpNotFound();
                    }
                    return View(exam);
                }
                else
                    return RedirectToAction("Login", "Account");
            }
            catch (Exception)
            {

                return RedirectToAction("Index");
            }
        }

        // POST: Exams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                if (Session["UserInfo"] != null)
                {
                    _examRepository.Delete(id);
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
