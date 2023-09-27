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
    public class LaboratoryBasisController : Controller
    {
        private IItemRepository _itemRepository;
        private ISchoolRepository _schoolRepository;

        private IRepository<Unit> _unitRepository;
        public LaboratoryBasisController(IRepository<Unit> unitRepository, ISchoolRepository schoolRepository, IItemRepository itemRepository)
        {
            this._unitRepository = unitRepository;
            this._schoolRepository = schoolRepository;
            this._itemRepository = itemRepository;

        }
        [ScreenPermissionFilter(screenId = 42)]
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

                    int schoolId = 0;
                    schoolId = (int)Session["ScoolId"];

                    var model = from s in _itemRepository.GetAllItemWithUnit(x => x.itm_year == year && x.itm_schId == schoolId && x.itm_type == Domain.Type.اثاث_مختبر)
                                select s;

                    //Search and match data, if search string is not null or empty
                    if (!String.IsNullOrEmpty(searchString))
                    {
                        model = model.Where(s => (s.itm_schId == schoolId) && s.itm_arName.Contains(searchString)
                                               || s.itm_Unit.unt_arName.Contains(searchString));
                    }
                    var ModelList = model;
                    switch (sortOrder)
                    {
                        case "name_desc":
                            ModelList = model.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.itm_arName);
                            break;

                        default:
                            ModelList = model.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.itm_arName);
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

                return RedirectToAction("Index", "LaboratoryBasis");
            }




        }

        #region Primary
        [ScreenPermissionFilter(screenId = 42)]
        public ActionResult GetPrimaryLevel(string sortOrder, string currentFilter, string searchString, int? page)
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

                    int schoolId = 0;
                    schoolId = (int)Session["ScoolId"];

                    var model = from s in _itemRepository.GetAllItemWithUnit(x => x.itm_year == year && x.itm_department == Domain.Department.ابتدائى && x.itm_schId == schoolId && x.itm_type == Domain.Type.اثاث_مختبر)
                                select s;

                    //Search and match data, if search string is not null or empty
                    if (!String.IsNullOrEmpty(searchString))
                    {
                        model = model.Where(s => (s.itm_schId == schoolId) && s.itm_arName.Contains(searchString)
                                               || s.itm_Unit.unt_arName.Contains(searchString));
                    }
                    var ModelList = model;
                    switch (sortOrder)
                    {
                        case "name_desc":
                            ModelList = model.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.itm_arName);
                            break;

                        default:
                            ModelList = model.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.itm_arName);
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

                return RedirectToAction("Index", "LaboratoryBasis");
            }


        }


        #endregion

        #region Secondry
        [ScreenPermissionFilter(screenId = 42)]
        public ActionResult GetSecondryLevel(string sortOrder, string currentFilter, string searchString, int? page)
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

                    int schoolId = 0;
                    schoolId = (int)Session["ScoolId"];

                    var model = from s in _itemRepository.GetAllItemWithUnit(x => x.itm_year == year && x.itm_department == Domain.Department.متوسط && x.itm_schId == schoolId && x.itm_type == Domain.Type.اثاث_مختبر)
                                select s;

                    //Search and match data, if search string is not null or empty
                    if (!String.IsNullOrEmpty(searchString))
                    {
                        model = model.Where(s => (s.itm_schId == schoolId) && s.itm_arName.Contains(searchString)
                                               || s.itm_enName.Contains(searchString)
                                               || s.itm_Unit.unt_arName.Contains(searchString));
                    }
                    var ModelList = model;
                    switch (sortOrder)
                    {
                        case "name_desc":
                            ModelList = model.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.itm_arName);
                            break;

                        default:
                            ModelList = model.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.itm_arName);
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

                return RedirectToAction("Index", "LaboratoryBasis");
            }


        }


        #endregion

        #region High School 
        #region GetPhysicsHighLevel
        [ScreenPermissionFilter(screenId = 42)]
        public ActionResult GetPhysicsHighLevel(string sortOrder, string currentFilter, string searchString, int? page)
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

                    int schoolId = 0;
                    schoolId = (int)Session["ScoolId"];

                    var model = from s in _itemRepository.GetAllItemWithUnit(x => x.itm_year == year && x.itm_department == Domain.Department.ثانوى && x.itm_schId == schoolId && x.itm_type == Domain.Type.اثاث_مختبر && x.itm_level == Level.اول)
                                select s;

                    //Search and match data, if search string is not null or empty
                    if (!String.IsNullOrEmpty(searchString))
                    {
                        model = model.Where(s => (s.itm_schId == schoolId) && s.itm_arName.Contains(searchString)
                                               || s.itm_enName.Contains(searchString)
                                               || s.itm_Unit.unt_arName.Contains(searchString));
                    }
                    var ModelList = model;
                    switch (sortOrder)
                    {
                        case "name_desc":
                            ModelList = model.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.itm_arName);
                            break;

                        default:
                            ModelList = model.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.itm_arName);
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
                return RedirectToAction("Index", "LaboratoryBasis");
            }


        }


        #endregion
        #region GetChemistryHighLevel
        [ScreenPermissionFilter(screenId = 42)]
        public ActionResult GetChemistryHighLevel(string sortOrder, string currentFilter, string searchString, int? page)
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

                    int schoolId = 0;
                    schoolId = (int)Session["ScoolId"];

                    var model = from s in _itemRepository.GetAllItemWithUnit(x => x.itm_year == year && x.itm_department == Domain.Department.ثانوى && x.itm_schId == schoolId && x.itm_type == Domain.Type.اثاث_مختبر && x.itm_level == Level.ثانى)
                                select s;

                    //Search and match data, if search string is not null or empty
                    if (!String.IsNullOrEmpty(searchString))
                    {
                        model = model.Where(s => (s.itm_schId == schoolId) && s.itm_arName.Contains(searchString)
                                               || s.itm_enName.Contains(searchString)
                                               || s.itm_Unit.unt_arName.Contains(searchString));
                    }
                    var ModelList = model;
                    switch (sortOrder)
                    {
                        case "name_desc":
                            ModelList = model.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.itm_arName);
                            break;

                        default:
                            ModelList = model.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.itm_arName);
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
                return RedirectToAction("Index", "LaboratoryBasis");
            }


        }


        #endregion

        #region GetBiologyHighLevel
        [ScreenPermissionFilter(screenId = 42)]
        public ActionResult GetBiologyHighLevel(string sortOrder, string currentFilter, string searchString, int? page)
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


                    int schoolId = 0;
                    schoolId = (int)Session["ScoolId"];
                    var model = from s in _itemRepository.GetAllItemWithUnit(x => x.itm_year == year && x.itm_department == Domain.Department.ثانوى && x.itm_schId == schoolId && x.itm_type == Domain.Type.اثاث_مختبر && x.itm_level == Level.ثالث)
                                select s;

                    //Search and match data, if search string is not null or empty
                    if (!String.IsNullOrEmpty(searchString))
                    {
                        model = model.Where(s => (s.itm_schId == schoolId) && s.itm_arName.Contains(searchString)
                                               || s.itm_enName.Contains(searchString)
                                               || s.itm_Unit.unt_arName.Contains(searchString));
                    }
                    var ModelList = model;
                    switch (sortOrder)
                    {
                        case "name_desc":
                            ModelList = model.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.itm_arName);
                            break;

                        default:
                            ModelList = model.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.itm_arName);
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
                return RedirectToAction("Index", "LaboratoryBasis");
            }


        }


        #endregion
        #endregion


        // GET: Items/Details/5
        [ScreenPermissionFilter(screenId = 43)]
        public ActionResult Details(int id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Item item = _itemRepository.GetItemViewModel(id);
                if (item == null)
                {
                    return HttpNotFound();
                }
                return View(item);
            }
            catch (Exception)
            {


                return RedirectToAction("Index", "LaboratoryBasis");
            }

        }

        // GET: Items/Create
        [ScreenPermissionFilter(screenId = 44)]
        public ActionResult Create()
        {
            try
            {
                if ((User)Session["UserInfo"] != null)
                {
                    User oUser = (User)Session["UserInfo"];
                    int schoolId = 0;
                    schoolId = (int)Session["ScoolId"];
                    ViewBag.itm_schId = new SelectList(_schoolRepository.GetAll(), "Id", "sch_arName", schoolId);
                    ViewBag.itm_unitId = new SelectList(_unitRepository.GetAll().Where(x => x.Id == 3), "Id", "unt_arName");
                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "LaboratoryBasis");
            }

        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,itm_code,itm_level,itm_type,itm_department, itm_arName,itm_enName,itm_desc,itm_unitId,itm_sugQty,itm_presentQty,itm_isExisting,itm_availableMethod,itm_chapter,itm_term,itm_schId,AddedDate,ModifiedDate,AddedBy,ModifiedBy,IsAvtive,itm_ministerialNo,itm_ValidState,itm_completionYear,itm_excessiveQty, itm_note,itm_over,itm_validQty,itm_unValidQty,itm_overQty")] Item item, HttpPostedFileBase itm_image)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    item.AddedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd"));
                    if ((User)Session["UserInfo"] != null)
                    {
                        User oUser = (User)Session["UserInfo"];
                        //if (oUser.usr_role == Role.مدير_نظام)
                        int schoolId = 0;
                        schoolId = (int)Session["ScoolId"];
                        //{
                        item.AddedBy = oUser.Id;
                            item.itm_schId = schoolId;
                            item.itm_type = Domain.Type.اثاث_مختبر;
                            ViewBag.itm_schId = new SelectList(_schoolRepository.GetAll(), "Id", "sch_arName", schoolId);
                            ViewBag.itm_unitId = new SelectList(_unitRepository.GetAll().Where(x => x.Id == 3), "Id", "unt_arName");
                        //}
                        //else
                        //{
                        //    return RedirectToAction("Login", "Account");
                        //}
                    }
                    else
                    {
                        return RedirectToAction("Login", "Account");
                    }
                    if (itm_image != null)
                    {
                        if (!GeneralValdate.ValidateImage(itm_image))
                        {

                            return View(item);
                        }
                        byte[] bytes = null;
                        using (var binaryReader = new BinaryReader(itm_image.InputStream))
                        {
                            bytes = binaryReader.ReadBytes(itm_image.ContentLength);
                        }
                        item.itm_image = bytes;
                    }
                    _itemRepository.Insert(item);

                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {


                return View(item);
            }

            return View(item);



        }

        // GET: Items/Edit/5
        [ScreenPermissionFilter(screenId = 45)]
        public ActionResult Edit(int id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Item item = _itemRepository.GetItemViewModel(id);
                if (item == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    if ((User)Session["UserInfo"] != null)
                    {
                        User oUser = (User)Session["UserInfo"];
                        int schoolId = 0;
                        schoolId = (int)Session["ScoolId"];
                        ViewBag.itm_schId = new SelectList(_schoolRepository.GetAll(), "Id", "sch_arName", schoolId);
                        ViewBag.itm_unitId = new SelectList(_unitRepository.GetAll(), "Id", "unt_arName", item.itm_unitId);
                        return View(item);
                    }
                    else
                    {
                        return RedirectToAction("Login", "Account");
                    }
                }
            }
            catch (Exception)
            {


                return RedirectToAction("Index", "LaboratoryBasis");
            }


        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,itm_code,itm_level,itm_type,itm_department, itm_arName,itm_enName,itm_desc,itm_unitId,itm_sugQty,itm_presentQty,itm_isExisting,itm_availableMethod,itm_chapter,itm_term,itm_schId,AddedDate,ModifiedDate,AddedBy,ModifiedBy,IsAvtive,itm_ministerialNo,itm_ValidState,itm_completionYear,itm_excessiveQty, itm_note,itm_over,itm_validQty,itm_unValidQty,itm_overQty,itm_image")] Item item, HttpPostedFileBase itm_image)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    item.ModifiedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd"));

                    if ((User)Session["UserInfo"] != null)
                    {
                        User oUser = (User)Session["UserInfo"];
                        item.ModifiedBy = oUser.Id;
                        item.itm_schId = (int)Session["ScoolId"];
                        item.itm_type = Domain.Type.اثاث_مختبر;
                    }
                    else
                    {
                        return RedirectToAction("Login", "Account");
                    }
                    if (itm_image != null)
                    {
                        if (!GeneralValdate.ValidateImage(itm_image))
                        {

                            return View(item);
                        }
                        byte[] bytes = null;
                        using (var binaryReader = new BinaryReader(itm_image.InputStream))
                        {
                            bytes = binaryReader.ReadBytes(itm_image.ContentLength);
                        }
                        item.itm_image = bytes;
                    }

                    _itemRepository.Update(item);
                    return View("close");

                }
            }
            catch (Exception)
            {

                ViewBag.itm_schId = new SelectList(_schoolRepository.GetAll(), "Id", "sch_arName");
                ViewBag.itm_unitId = new SelectList(_unitRepository.GetAll(), "Id", "unt_arName");
                return View(item);
            }
            ViewBag.itm_schId = new SelectList(_schoolRepository.GetAll(), "Id", "sch_arName");
            ViewBag.itm_unitId = new SelectList(_unitRepository.GetAll(), "Id", "unt_arName");
            return View(item);
        }

        // GET: Items/Delete/5
        [ScreenPermissionFilter(screenId = 46)]
        public ActionResult Delete(int id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Item item = _itemRepository.GetItemViewModel(id);
                if (item == null)
                {
                    return HttpNotFound();
                }
                return View(item);
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "LaboratoryBasis");
            }

        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                User oUser = (User)Session["UserInfo"];
                //if (oUser.usr_role == Role.مدير_نظام)
                //{
                    _itemRepository.Delete(id);

                    return RedirectToAction("Index");
                //}
                //else
                //    return RedirectToAction("Login", "Account");
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "LaboratoryBasis");
            }


        }


    }
}