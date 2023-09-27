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
    public class StudentsController : Controller
    {
        private IStudentRepository _studentRepository;
        private ISchoolRepository _schoolRepository;
   
        public StudentsController(IStudentRepository studentRepository, ISchoolRepository schoolRepository)
        {
            this._studentRepository = studentRepository;
            this._schoolRepository = schoolRepository;

        }
        // GET: 
        [ScreenPermissionFilter(screenId = 101)]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            try
            {
                if ((User)Session["UserInfo"] != null)
                {
                    User oUser = (User)Session["UserInfo"];
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



                    var model = from s in _studentRepository.GetAllStudentWithSchool() select s;

                    //Search and match data, if search string is not null or empty
                    if (!String.IsNullOrEmpty(searchString))
                    {
                        model = model.Where(s => s.std_arName.Contains(searchString));
                    }
                    var ModelList = model;
                    switch (sortOrder)
                    {
                        case "name_desc":
                            ModelList = model.OrderBy(x => x.std_arName);
                            break;

                        default:
                            ModelList = model.OrderBy(x => x.std_arName);
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

        // GET: Students/Details/5
        [ScreenPermissionFilter(screenId = 102)]
        public ActionResult Details(int id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Student student = _studentRepository.GetStudentWithSchoolById(id);
                if (student == null)
                {
                    return HttpNotFound();
                }
                return View(student);
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
        }

        // GET: Students/Create
        [ScreenPermissionFilter(screenId = 103)]
        public ActionResult Create()
        {
      
            try
            {
                ViewBag.std_schId = new SelectList(_schoolRepository.GetAll(), "Id", "sch_arName");
                return View();

            }
            catch (Exception)
            {

                return RedirectToAction("Index");
            }
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,std_arName,std_enName,std_phon,std_email,std_address,std_department,std_level,std_levelRecord,std_schId,std_IdentityNumber")] Student student, HttpPostedFileBase std_image)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var count = _studentRepository.SingleSearch(x => x.std_IdentityNumber == student.std_IdentityNumber);
                    int length = Convert.ToString(student.std_IdentityNumber).Length;
                    if (student.std_IdentityNumber > 0 && count == null && length == 10)
                    {
                        try
                        {
                            student.AddedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd"));
                            if ((User)Session["UserInfo"] != null)
                            {
                                User oUser = (User)Session["UserInfo"];

                                student.AddedBy = oUser.Id;


                            }
                            else
                            {
                                return RedirectToAction("Login", "Account");
                            }
                            if (std_image != null)
                            {
                                if (!GeneralValdate.ValidateImage(std_image))
                                {

                                    return View(student);
                                }
                                byte[] bytes = null;
                                using (var binaryReader = new BinaryReader(std_image.InputStream))
                                {
                                    bytes = binaryReader.ReadBytes(std_image.ContentLength);
                                }
                                student.std_image = bytes;
                            }

                            _studentRepository.Insert(student);



                            return RedirectToAction("Index");
                        }
                        catch (Exception)
                        {

                            ViewBag.std_schId = new SelectList(_schoolRepository.GetAll(), "Id", "sch_arName");
                            return View(student);
                        }
                    }


                }

                ViewBag.std_schId = new SelectList(_schoolRepository.GetAll(), "Id", "sch_arName");
                return View(student);
            }
            catch (Exception)
            {

                return RedirectToAction("Index");
            }

        }

        // GET: Students/Edit/5
        [ScreenPermissionFilter(screenId = 104)]
        public ActionResult Edit(int id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Student student = _studentRepository.GetByNum(id);
                if (student == null)
                {
                    return HttpNotFound();
                }
                ViewBag.std_schId = new SelectList(_schoolRepository.GetAll(), "Id", "sch_arName");
                return View(student);
            }
            catch (Exception)
            {

                return RedirectToAction("Index");
            }
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,std_arName,std_enName,std_phon,std_email,std_address,std_department,std_level,std_levelRecord,std_schId,std_IdentityNumber")] Student student, HttpPostedFileBase std_image)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        student.ModifiedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd"));

                        if ((User)Session["UserInfo"] != null)
                        {
                            User oUser = (User)Session["UserInfo"];

                            student.ModifiedBy = oUser.Id;


                        }
                        else
                        {
                            return RedirectToAction("Login", "Account");
                        }
                        if (std_image != null)
                        {
                            if (!GeneralValdate.ValidateImage(std_image))
                            {

                                return View(student);
                            }
                            byte[] bytes = null;
                            using (var binaryReader = new BinaryReader(std_image.InputStream))
                            {
                                bytes = binaryReader.ReadBytes(std_image.ContentLength);
                            }
                            student.std_image = bytes;
                        }
                        _studentRepository.Update(student); ;
                        return RedirectToAction("Index");
                    }
                    catch (Exception)
                    {

                        ViewBag.std_schId = new SelectList(_schoolRepository.GetAll(), "Id", "sch_arName");
                        return View(student);
                    }

                }
                ViewBag.std_schId = new SelectList(_schoolRepository.GetAll(), "Id", "sch_arName");
                return View(student);
            }
            catch (Exception)
            {

                return RedirectToAction("Index");
            }
    
        }

        // GET: Students/Delete/5
        [ScreenPermissionFilter(screenId = 105)]
        public ActionResult Delete(int id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Student student = _studentRepository.GetStudentWithSchoolById(id);
                if (student == null)
                {
                    return HttpNotFound();
                }
                return View(student);
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Complexes");
            }
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                User oUser = (User)Session["UserInfo"];
                //if (oUser.usr_role == Role.مدير_نظام)
                //{
                    _studentRepository.Delete(id);

                    return RedirectToAction("Index");
                //}
                //else
                //    return RedirectToAction("Login", "Account");
            }
            catch (Exception)
            {

                return RedirectToAction("index");
            }
        }

      
    }
}
