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
    public class TeacherMovmentsController : Controller
    {

        private ITeacherMovmentRepository _teacherMovmentRepository;
        private IRepository<Teacher> _teacherRepository;
        private IScheduleHedRepository _scheduleHedRepository;
        public TeacherMovmentsController(IScheduleHedRepository scheduleHedRepository, ITeacherMovmentRepository teacherMovmentRepository, IRepository<Teacher> teacherRepository)
        {
            this._teacherRepository = teacherRepository;
            this._teacherMovmentRepository = teacherMovmentRepository;
            this._scheduleHedRepository = scheduleHedRepository;
        }
        [ScreenPermissionFilter(screenId = 57)]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    ViewBag.CurrentSort = sortOrder;
                    ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";


                    if (searchString != null)
                    {
                        page = 1;
                    }
                    else
                    {
                        searchString = currentFilter;
                    }

                    ViewBag.CurrentFilter = searchString;

                    var model = _teacherMovmentRepository.GetAllTeacherMovmentWithTeacher();

                    var ModelList = model;
                    switch (sortOrder)
                    {
                        case "name_desc":
                            ModelList = model.OrderBy(x => x.mov_monthNumberr).ThenBy(s => s.Id);
                            break;

                        default:
                            ModelList = model.OrderBy(x => x.mov_monthNumberr).ThenBy(s => s.Id);
                            break;
                    }


                    //indicates the size of list
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



        // GET: TeacherMovments/Details/5
        [ScreenPermissionFilter(screenId = 58)]
        public ActionResult Details(int id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                TeacherMovment teacherMovment = _teacherMovmentRepository.GetTeacherMovmentWithTeacherById(id);
                if (teacherMovment == null)
                {
                    return HttpNotFound();
                }
                return View(teacherMovment);
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }

        }

        // GET: TeacherMovments/Create
        [ScreenPermissionFilter(screenId = 59)]
        public ActionResult Create()
        {
            try
            {
                ViewBag.mov_techId = new SelectList(_teacherRepository.GetAll(), "Id", "tech_arName");
                return View();
            }
            catch (Exception)
            {

                return RedirectToAction("Index");
            }

        }

        // POST: TeacherMovments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,mov_lessonCount,mov_lessonCountActual,mov_weekNumber,mov_monthNumberr,mov_techId,mov_note")] TeacherMovment teacherMovment)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int count = (int)_scheduleHedRepository.GetAll().Where(x => x.schd_teachId == teacherMovment.mov_techId).FirstOrDefault().schd_lessonCount;
                    teacherMovment.mov_lessonCount = count;
                    if (count >= teacherMovment.mov_lessonCountActual)
                    {
                        _teacherMovmentRepository.Insert(teacherMovment);
                    }

                    return RedirectToAction("Index");
                }

                ViewBag.mov_techId = new SelectList(_teacherRepository.GetAll(), "Id", "tech_arName");
                return View(teacherMovment);
            }
            catch (Exception)
            {

                return RedirectToAction("Index");
            }

        }

        // GET: TeacherMovments/Edit/5
        [ScreenPermissionFilter(screenId = 60)]
        public ActionResult Edit(int id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                TeacherMovment teacherMovment = _teacherMovmentRepository.GetTeacherMovmentWithTeacherById(id);
                if (teacherMovment == null)
                {
                    return HttpNotFound();
                }
                ViewBag.mov_techId = new SelectList(_teacherRepository.GetAll(), "Id", "tech_arName", teacherMovment.mov_techId);
                return View(teacherMovment);
            }
            catch (Exception)
            {

                return RedirectToAction("Index");
            }

        }

        // POST: TeacherMovments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,mov_lessonCount,mov_lessonCountActual,mov_weekNumber,mov_monthNumberr,mov_techId,mov_note")] TeacherMovment teacherMovment)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int count = (int)_scheduleHedRepository.GetAll().Where(x => x.schd_teachId == teacherMovment.mov_techId).FirstOrDefault().schd_lessonCount;
                    if (count >= teacherMovment.mov_lessonCountActual)
                    {
                        _teacherMovmentRepository.Update(teacherMovment);
                    }

                    return RedirectToAction("Index");
                }
                ViewBag.mov_techId = new SelectList(_teacherRepository.GetAll(), "Id", "tech_arName");
                return View(teacherMovment);
            }
            catch (Exception ex)
            {

                return RedirectToAction("Index");
            }

        }

        // GET: TeacherMovments/Delete/5
        [ScreenPermissionFilter(screenId = 61)]
        public ActionResult Delete(int id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                TeacherMovment teacherMovment = _teacherMovmentRepository.GetTeacherMovmentWithTeacherById(id);
                if (teacherMovment == null)
                {
                    return HttpNotFound();
                }
                return View(teacherMovment);
            }
            catch (Exception)
            {

                return RedirectToAction("Index");
            }

        }

        // POST: TeacherMovments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            try
            {

                TeacherMovment teacherMovment = _teacherMovmentRepository.GetTeacherMovmentWithTeacherById(id);
                _teacherMovmentRepository.DeleteUsingMultiKey(teacherMovment);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                return RedirectToAction("Index");
            }
        }


    }
}
