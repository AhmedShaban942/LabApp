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
using Laboratories.Persistence.Repositories;
using Laboratories.Web.Models;
using PagedList;
using static Laboratories.Domain.User;

namespace Laboratories.Web.Controllers
{
    [PermClass]
    public class ComplexesController : Controller
    {
        private IRepository<AppSeting> _appSetingRepository;
        private IComplexRepository _complexRepository;
        private IRepository<Company> _companyRepository;
        public ComplexesController(IComplexRepository complexRepository, IRepository<Company> companyRepository, IRepository<AppSeting> appSetingRepository)
        {
            this._complexRepository = complexRepository;
            this._companyRepository = companyRepository;
            this._appSetingRepository = appSetingRepository;
        }

        // GET: Complexes
        [ScreenPermissionFilter(screenId = 12)]
        public ActionResult Index(int? page)
        {
            try
            {
                if (Session["UserInfo"] != null)
                {

                    User oUser = (User)Session["UserInfo"];
                    //if (oUser.usr_role == Role.مدير_نظام)
                    //{
                        var ModelList = _complexRepository.GetAllComplexWithCompany().OrderBy(x => x.Id);
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

        // GET: Complexes/Details/5
        [ScreenPermissionFilter(screenId = 13)]
        public ActionResult Details(int id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Complex complex = _complexRepository.GetComplexWithCompanyById(id);

                if (complex == null)
                {
                    return HttpNotFound();
                }
                return View(complex);
            }
            catch (Exception)
            {

                return RedirectToAction("index");
            }

        }

        // GET: Complexes/Create
        [ScreenPermissionFilter(screenId = 14)]
        public ActionResult Create()
        {
            try
            {
                ViewBag.comp_com_id = new SelectList(_companyRepository.GetAll(), "Id", "com_arName");
                return View();
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Complexes");
            }

        }

        // POST: Complexes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,comp_arName,comp_enName,comp_phon,comp_email,comp_address,comp_com_id,AddedDate,ModifiedDate,AddedBy,ModifiedBy,IsAvtive")] Complex complex, HttpPostedFileBase comp_image)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    complex.AddedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd"));
                    if ((User)Session["UserInfo"] != null)
                    {
                        User oUser = (User)Session["UserInfo"];
                        //if (oUser.usr_role == Role.مدير_نظام)
                        //{
                            complex.AddedBy = oUser.Id;
                        //}
                        //else
                        //    return RedirectToAction("Login", "Account");

                    }
                    else
                    {
                        return RedirectToAction("Login", "Account");
                    }
                    if (comp_image != null)
                    {
                        if (!GeneralValdate.ValidateImage(comp_image))
                        {

                            return View(complex);
                        }
                        byte[] bytes = null;
                        using (var binaryReader = new BinaryReader(comp_image.InputStream))
                        {
                            bytes = binaryReader.ReadBytes(comp_image.ContentLength);
                        }
                        complex.comp_image = bytes;
                    }
                    if (_appSetingRepository.GetAll().Count > 0)
                    {
                        if (_appSetingRepository.GetAll().FirstOrDefault().compelexNumber > _complexRepository.GetAll().Count)
                        {
                            _complexRepository.Insert(complex);
                        }
                    }


                    return RedirectToAction("Index");
                }
                catch (Exception)
                {

                    ViewBag.comp_com_id = new SelectList(_companyRepository.GetAll(), "Id", "com_arName", complex.comp_com_id);
                    return View(complex);
                }

            }

            ViewBag.comp_com_id = new SelectList(_companyRepository.GetAll(), "Id", "com_arName", complex.comp_com_id);
            return View(complex);
        }

        // GET: Complexes/Edit/5
        [ScreenPermissionFilter(screenId = 15)]
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Complex complex = _complexRepository.GetByNum(id);
                if (complex == null)
                {
                    return HttpNotFound();
                }
                ViewBag.comp_com_id = new SelectList(_companyRepository.GetAll(), "Id", "com_arName", complex.comp_com_id);
                return View(complex);
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Complexes");
            }

        }

        // POST: Complexes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,comp_arName,comp_enName,comp_phon,comp_email,comp_address,comp_com_id,AddedDate,ModifiedDate,AddedBy,ModifiedBy,IsAvtive,comp_image")] Complex complex, HttpPostedFileBase comp_image)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    complex.ModifiedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd"));

                    if ((User)Session["UserInfo"] != null)
                    {
                        User oUser = (User)Session["UserInfo"];
                        //if (oUser.usr_role == Role.مدير_نظام)
                        //{
                            complex.ModifiedBy = oUser.Id;
                        //}
                        //else
                        //    return RedirectToAction("Login", "Account");

                    }
                    else
                    {
                        return RedirectToAction("Login", "Account");
                    }
                    if (comp_image != null)
                    {
                        if (!GeneralValdate.ValidateImage(comp_image))
                        {

                            return View(complex);
                        }
                        byte[] bytes = null;
                        using (var binaryReader = new BinaryReader(comp_image.InputStream))
                        {
                            bytes = binaryReader.ReadBytes(comp_image.ContentLength);
                        }
                        complex.comp_image = bytes;
                    }
                    _complexRepository.Update(complex); ;
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {

                    ViewBag.comp_com_id = new SelectList(_companyRepository.GetAll(), "Id", "com_arName", complex.comp_com_id);
                    return View(complex);
                }

            }
            ViewBag.comp_com_id = new SelectList(_companyRepository.GetAll(), "Id", "com_arName", complex.comp_com_id);
            return View(complex);
        }

        // GET: Complexes/Delete/5
        [ScreenPermissionFilter(screenId = 16)]
        public ActionResult Delete(int id)
        {


            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Complex complex = _complexRepository.GetComplexWithCompanyById(id);
                if (complex == null)
                {
                    return HttpNotFound();
                }
                return View(complex);
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Complexes");
            }
        }

        // POST: Complexes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                User oUser = (User)Session["UserInfo"];
                //if (oUser.usr_role == Role.مدير_نظام)
                //{
                    _complexRepository.Delete(id);

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
