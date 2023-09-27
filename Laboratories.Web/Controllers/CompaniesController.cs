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
using static Laboratories.Domain.User;

namespace Laboratories.Web.Controllers
{
    [PermClass]
    public class CompaniesController : Controller
    {
        private IRepository<Company> _companyRepository;
        private IRepository<AppSeting> _appSetingRepository;
        public CompaniesController(IRepository<Company> companyRepository, IRepository<AppSeting> appSetingRepository)
        {
            this._companyRepository = companyRepository;
            this._appSetingRepository = appSetingRepository;

        }

        // GET: Companies
        [ScreenPermissionFilter(screenId = 7)]
        public ActionResult Index()
        {
            try
            {
                if (Session["UserInfo"] != null)
                {
                    User oUser = (User)Session["UserInfo"];
                    //if (oUser.usr_role == Role.مدير_نظام)
                    //{
                        return View(_companyRepository.GetAll());
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

        // GET: Companies/Details/5
        [ScreenPermissionFilter(screenId = 8)]
        public ActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Company company = _companyRepository.GetByNum(id);
                if (company == null)
                {
                    return HttpNotFound();
                }
                return View(company);
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Companies");
            }

        }

        // GET: Companies/Create
        [ScreenPermissionFilter(screenId = 9)]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,com_arName,com_enName,com_phon,com_email,com_address,IsAvtive")] Company company, HttpPostedFileBase com_image)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        company.AddedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd"));
                        if ((User)Session["UserInfo"] != null)
                        {

                            User oUser = (User)Session["UserInfo"];
                            //if (oUser.usr_role == Role.مدير_نظام)
                            //{
                                company.AddedBy = oUser.Id;
                            //}
                            //else
                            //    return RedirectToAction("Login", "Account");

                        }
                        else
                        {
                            return RedirectToAction("Login", "Account");
                        }
                        if (com_image != null)
                        {
                            if (!GeneralValdate.ValidateImage(com_image))
                            {

                                return View(company);
                            }

                            byte[] bytes = null;
                            using (var binaryReader = new BinaryReader(com_image.InputStream))
                            {
                                bytes = binaryReader.ReadBytes(com_image.ContentLength);
                            }
                            company.com_image = bytes;
                        }
                        if (_appSetingRepository.GetAll().Count > 0)
                        {
                            if (_appSetingRepository.GetAll().FirstOrDefault().companyNumber > _companyRepository.GetAll().Count)
                            {
                                _companyRepository.Insert(company);
                            }
                        }

                        return RedirectToAction("Index");
                    }
                    catch (Exception)
                    {

                        return View(company);
                    }

                }

                return View(company);
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Companies");
            }

        }

        // GET: Companies/Edit/5
        [ScreenPermissionFilter(screenId = 10)]
        public ActionResult Edit(int? id)
        {

            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Company company = _companyRepository.GetByNum(id);
                if (company == null)
                {
                    return HttpNotFound();
                }
                return View(company);
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Companies");
            }
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,com_arName,com_enName,com_phon,com_email,com_address,AddedDate,ModifiedDate,AddedBy,ModifiedBy,IsAvtive,com_image")] Company company, HttpPostedFileBase com_image)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        company.ModifiedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd"));

                        if ((User)Session["UserInfo"] != null)
                        {

                            User oUser = (User)Session["UserInfo"];
                            //if (oUser.usr_role == Role.مدير_نظام)
                            //{
                                company.ModifiedBy = oUser.Id;
                            //}
                            //else
                            //    return RedirectToAction("Login", "Account");

                        }
                        else
                        {
                            return RedirectToAction("Login", "Account");
                        }
                        if (com_image != null)
                        {
                            if (!GeneralValdate.ValidateImage(com_image))
                            {

                                return View(company);
                            }
                            byte[] bytes = null;
                            using (var binaryReader = new BinaryReader(com_image.InputStream))
                            {
                                bytes = binaryReader.ReadBytes(com_image.ContentLength);
                            }
                            company.com_image = bytes;
                        }
                        _companyRepository.Update(company);
                        return RedirectToAction("Index");
                    }
                    catch (Exception)
                    {

                        return View(company);
                    }

                }
                return View(company);
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Companies");
            }

        }

        // GET: Companies/Delete/5
        [ScreenPermissionFilter(screenId = 11)]
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Company company = _companyRepository.GetByNum(id);
                if (company == null)
                {
                    return HttpNotFound();
                }
                return View(company);
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Companies");
            }


        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                User oUser = (User)Session["UserInfo"];
                //if (oUser.usr_role == Role.مدير_نظام)
                //{
                    Company company = _companyRepository.GetByNum(id);
                    _companyRepository.Delete(id);
                    return RedirectToAction("Index");
                //}
                //else
                //    return RedirectToAction("Login", "Account");
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Companies");
            }


        }


    }
    }

