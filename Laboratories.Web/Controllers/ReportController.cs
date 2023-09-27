using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using Laboratories.Application.Contracts;
using Laboratories.Domain;
using Laboratories.Web.Models;
using Laboratories.Web.ViewModel;
using PagedList;
namespace Laboratories.Web.Controllers
{
    [PermClass]
    public class ReportController : Controller
    {
        private IItemRepository _itemRepository;
        private IComplexRepository _complexRepository;
        private IRepository<Company> _companyRepository;
        private ISchoolRepository _schoolRepository;
        private IRepository<Teacher> _teacherRepository;
      
   
        public ReportController(IItemRepository itemRepository, IComplexRepository complexRepository, IRepository<Company> companyRepository, ISchoolRepository schoolRepository, IRepository<Teacher> teacherRepository)
        {

            this._itemRepository = itemRepository;
            this._complexRepository = complexRepository;
            this._companyRepository = companyRepository;
            this._schoolRepository = schoolRepository;
            this._teacherRepository = teacherRepository;
    

        }
        #region Custom Report

        #region Custom Report For Experment

        public ActionResult CustomReportForExperment()
        {
            try
            {
                if (Session["UserInfo"] != null)
                {
                    ViewBag.company = new SelectList(_companyRepository.GetAll(), "Id", "com_arName");
                    ViewBag.complex = new SelectList(_complexRepository.GetAll(), "Id", "comp_arName");
                    ViewBag.school = new SelectList(_schoolRepository.GetAll(), "Id", "sch_arName");
                    
                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception)
            {

                return RedirectToAction("CustomReport", "Report");
            }

        }

        #endregion
        [ScreenPermissionFilter(screenId = 70)]
        public ActionResult CustomReport()
        {
            try
            {
                if (Session["UserInfo"] != null)
                {
                    ViewBag.company = new SelectList(_companyRepository.GetAll(), "Id", "com_arName");
                    ViewBag.complex = new SelectList(_complexRepository.GetAll(), "Id", "comp_arName");
                    ViewBag.school = new SelectList(_schoolRepository.GetAll(), "Id", "sch_arName");
                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception)
            {

                return RedirectToAction("CustomReport", "Report");
            }

        }

        public ActionResult TransferCustody()
        {
            try
            {
                if (Session["UserInfo"] != null)
                {
                    ViewBag.company = new SelectList(_companyRepository.GetAll(), "Id", "com_arName");
                    ViewBag.complex = new SelectList(_complexRepository.GetAll(), "Id", "comp_arName");
                    ViewBag.school = new SelectList(_schoolRepository.GetAll(), "Id", "sch_arName");
                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception)
            {

                return RedirectToAction("TransferCustody", "Report");
            }

        }

        public ActionResult DownloadTransferCustody(int schoolId1, int schoolId2)
        {
            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];

                    var searchIds = _itemRepository.GetAll().Where(x => x.itm_type != Domain.Type.اثاث_مختبر && x.itm_year == year && x.itm_schId == schoolId2 && x.itm_isExisting == ExsistState.غير_موجود).Select(x => x.itm_code).ToList();
                    var items = _itemRepository.GetAllItemViewModel(p => p.itm_type != Domain.Type.اثاث_مختبر && p.itm_year == year && p.itm_schId == schoolId1 && p.itm_isExisting == ExsistState.موجود && p.itm_presentQty > 0 && searchIds.Contains(p.itm_code)).ToList();





                    List<ItemViewModel> model = new List<ItemViewModel>();

                    model = items.Select(o => new ItemViewModel
                    {
                        
                        itm_code = o.itm_code,
                        itm_arName = o.itm_arName,
                        itm_enName = o.itm_enName,
                        itm_desc = o.itm_desc,

                        itm_department = o.itm_department.ToString(),

                        itm_level = o.itm_level.ToString(),

                        itm_type = o.itm_type.ToString(),



                        itm_Unit = o.itm_Unit.unt_arName,

                        itm_sugQty = o.itm_sugQty ?? 0.0,

                        itm_presentQty = o.itm_presentQty ?? 0.0,

                        itm_isExisting = o.itm_isExisting.ToString(),
                        itm_availableMethod = o.itm_availableMethod.ToString(),

                        itm_chapter = o.itm_chapter ?? 0,

                        itm_term = o.itm_term.ToString(),

                        itm_School = o.itm_School.sch_arName,
                        itm_ValidState = o.itm_ValidState.ToString(),
                        itm_completionYear = o.itm_completionYear ?? 0,
                        itm_excessiveQty = o.itm_excessiveQty ?? 0,
                        itm_note = o.itm_note,
                        company = o.itm_School.sch_complex.comp_company.com_arName,
                        complex = o.itm_School.sch_complex.comp_arName
                    }).ToList();

                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "transfer.rpt"));
                    DataSet ds = new DataSet();
                    ds = new LogoDataSet.LogoDataSet();
                    var comlogo = _companyRepository.GetAll().FirstOrDefault().com_image;
                    var complogo = _complexRepository.GetAll().FirstOrDefault().comp_image;

                    ds.Tables[0].Rows.Add(comlogo, complogo);
                    rd.Database.Tables[0].SetDataSource(model);
                    rd.Database.Tables[1].SetDataSource(ds.Tables[0]);
                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "الاصناف التى يمكن نقلها.pdf");
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }

            }
            catch (Exception)
            {

                return RedirectToAction("TransferCustody", "Report");
            }

        }

        public ActionResult GetTransferCustody(int? schoolId1, int? schoolId2, int? page)
        {


            try
            {
                if (Session["ScoolId1"] == null || Session["ScoolId2"] == null)
                {
                    Session["ScoolId1"] = schoolId1;
                    Session["ScoolId2"] = schoolId2;
                }

                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    var searchIds = _itemRepository.MultiSearch(x => x.itm_type != Domain.Type.اثاث_مختبر && x.itm_year == year && x.itm_schId == schoolId2 && x.itm_isExisting == ExsistState.غير_موجود).Select(x => x.itm_code).ToList();
                    var ModelList = _itemRepository.GetAllItemWithUnit(p => p.itm_type != Domain.Type.اثاث_مختبر && p.itm_year == year && p.itm_schId == schoolId1 && p.itm_isExisting == ExsistState.موجود && p.itm_presentQty > 0 && searchIds.Contains(p.itm_code)).ToList();

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


                return RedirectToAction("TransferCustody", "Report");
            }
        }


        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult EditTransferCustody(string code, int presentQty, int transferQty)
        {
            try
            {
                if ((User)Session["UserInfo"] != null)
                {

                    if (Session["ScoolId1"] != null && Session["ScoolId2"] != null)
                    {
                        int schollid1 = Convert.ToInt32(Session["ScoolId1"]);
                        int schollid2 = Convert.ToInt32(Session["ScoolId2"]);
                        if (_schoolRepository.GetByNum(schollid1).sch_department !=_schoolRepository.GetByNum(schollid2).sch_department)
                        {
                            return Json("2", JsonRequestBehavior.AllowGet);
                        }
                        var item1 = _itemRepository.SingleSearch(x => x.itm_schId == schollid1 && x.itm_code == code);
                        if (item1 != null)
                        {
                            item1.itm_presentQty = presentQty;
                            if (presentQty == 0)
                            {
                                item1.itm_isExisting = ExsistState.غير_موجود;
                            }
                            _itemRepository.Update(item1);
                        }
                        var item2 = _itemRepository.SingleSearch(x => x.itm_schId == schollid2 && x.itm_code == code);
                        if (item2 != null)
                        {
                            item2.itm_presentQty = item2.itm_presentQty + transferQty;
                            item2.itm_isExisting = ExsistState.موجود;
                            _itemRepository.Update(item2);
                        }

                        return Json("1", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("0", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }


            }
            catch (Exception ex)
            {

                return Json("0", JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult DownloadTransferedItem(double qty, string itm_name)
        {
            try
            {
                int schollid1 = Convert.ToInt32(Session["ScoolId1"]);
                int schollid2 = Convert.ToInt32(Session["ScoolId2"]);
                string school1 = _schoolRepository.GetByNum(schollid1).sch_arName;
                string school2 = _schoolRepository.GetByNum(schollid2).sch_arName;

                List<ItemTransferViewModel> model = new List<ItemTransferViewModel>();
                ItemTransferViewModel Item = new ItemTransferViewModel();
                Item.school1 = school1;
                Item.school2 = school2;
                Item.itm_name = _itemRepository.SingleSearch(x => x.itm_schId == schollid1 && x.itm_code == itm_name).itm_arName;
                Item.qty = qty;
                model.Add(Item);
                ReportDocument rd = new ReportDocument();
                rd.Load(Path.Combine(Server.MapPath("~/Reports"), "transferedItem.rpt"));
                DataSet ds = new DataSet();
                ds = new LogoDataSet.LogoDataSet();
                var comlogo = _companyRepository.GetAll().FirstOrDefault().com_image;
                var complogo = _complexRepository.GetAll().FirstOrDefault().comp_image;

                ds.Tables[0].Rows.Add(comlogo, complogo);
                rd.Database.Tables[0].SetDataSource(model);
                rd.Database.Tables[1].SetDataSource(ds.Tables[0]);

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "نقل عهدة صنف :" + itm_name + ".pdf");

            }
            catch (Exception)
            {

                return RedirectToAction("TransferCustody", "Report");
            }

        }

        public JsonResult GetSchools(int complex_Id)

        {
            try
            {
                var schoolList = _schoolRepository.MultiSearch(x => x.sch_comp_id == complex_Id);
                return Json(schoolList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json(null);
            }


        }
        public JsonResult GetTeachers(int school_Id)

        {
            try
            {
                var teacherList = _teacherRepository.MultiSearch(x => x.tech_schId == school_Id);
                return Json(teacherList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json(null);
            }


        }
        public JsonResult GetSchoolsByDepartment(int complex_Id, int type_Id,int department_Id)

        {
            try
            {
                var schoolList = _schoolRepository.MultiSearch(x => x.sch_comp_id == complex_Id &&x.sch_type==(SchoolType)type_Id &&x.sch_department==(SchoolDepartment)department_Id);
                return Json(schoolList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json(null);
            }


        }
        public ActionResult DownloadCustomData(string sortOrder, string currentFilter, string searchString, string reportType, int companyId, int? complexId, int? schoolId, int? page)
        {
            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    var items = _itemRepository.GetAllItemViewModel(x => x.itm_year == year);
                    if (complexId == null )
                    {
                        items = _itemRepository.GetAllItemViewModel(x => x.itm_year == year);
                    }
                    else if (complexId != null && schoolId == null )

                    {
                        items = _itemRepository.GetAllItemViewModel(x => x.itm_year == year && x.itm_School.sch_comp_id == complexId);
                    }
                    else if (complexId != null && schoolId != null)
                    {
                        items = _itemRepository.GetAllItemViewModel(x => x.itm_year == year && x.itm_schId == schoolId);
                    }
                    else
                    {
                        int school_id = 0;
                        school_id = (int)Session["ScoolId"];
                        items = _itemRepository.GetAllItemViewModel(x => x.itm_year == year && x.itm_schId == school_id);
                    }
                    List<ItemViewModel> model = new List<ItemViewModel>();
                    if (reportType == "GetItemsCustom")
                    {
                        model = items.Where(x => x.itm_type != Domain.Type.اثاث_مختبر).Select(o => new ItemViewModel
                        {

                            itm_code = o.itm_code,
                            itm_arName = o.itm_arName,
                            itm_enName = o.itm_enName,
                            itm_desc = o.itm_desc,

                            itm_department = o.itm_department.ToString(),

                            itm_level = o.itm_level.ToString(),

                            itm_type = o.itm_type.ToString(),



                            itm_Unit = o.itm_Unit.unt_arName,

                            itm_sugQty = o.itm_sugQty ?? 0.0,

                            itm_presentQty = o.itm_presentQty ?? 0.0,

                            itm_isExisting = o.itm_isExisting.ToString(),
                            itm_availableMethod = o.itm_availableMethod.ToString(),

                            itm_chapter = o.itm_chapter ?? 0,

                            itm_term = o.itm_term.ToString(),

                            itm_School = o.itm_School.sch_arName,
                            itm_ValidState = o.itm_ValidState.ToString(),
                            itm_completionYear = o.itm_completionYear ?? 0,
                            itm_excessiveQty = o.itm_excessiveQty ?? 0,
                            itm_note = o.itm_note,
                            company = o.itm_School.sch_complex.comp_company.com_arName,
                            complex = o.itm_School.sch_complex.comp_arName
                        }).ToList();

                    }
                    else if (reportType == "GetCanAvalableItemsCustom")
                    {
                        model = items.Where(x => x.itm_availableMethod == AvailableMethod.المدرسة && x.itm_isExisting == ExsistState.غير_موجود && x.itm_type != Domain.Type.اثاث_مختبر).Select(o => new ItemViewModel
                        {

                            itm_code = o.itm_code,
                            itm_arName = o.itm_arName,
                            itm_enName = o.itm_enName,
                            itm_desc = o.itm_desc,

                            itm_department = o.itm_department.ToString(),

                            itm_level = o.itm_level.ToString(),

                            itm_type = o.itm_type.ToString(),



                            itm_Unit = o.itm_Unit.unt_arName,

                            itm_sugQty = o.itm_sugQty ?? 0.0,

                            itm_presentQty = o.itm_presentQty ?? 0.0,

                            itm_isExisting = o.itm_isExisting.ToString(),
                            itm_availableMethod = o.itm_availableMethod.ToString(),

                            itm_chapter = o.itm_chapter ?? 0,

                            itm_term = o.itm_term.ToString(),

                            itm_School = o.itm_School.sch_arName,
                            itm_ValidState = o.itm_ValidState.ToString(),
                            itm_completionYear = o.itm_completionYear ?? 0,
                            itm_excessiveQty = o.itm_excessiveQty ?? 0,
                            itm_note = o.itm_note,
                            company = o.itm_School.sch_complex.comp_company.com_arName,
                            complex = o.itm_School.sch_complex.comp_arName
                        }).ToList();

                    }
                    else if (reportType == "GetWantedItemsCustom")
                    {
                        model = items.Where(x => x.itm_availableMethod == AvailableMethod.المستودع && x.itm_isExisting == ExsistState.غير_موجود && x.itm_type != Domain.Type.اثاث_مختبر).Select(o => new ItemViewModel
                        {

                            itm_code = o.itm_code,
                            itm_arName = o.itm_arName,
                            itm_enName = o.itm_enName,
                            itm_desc = o.itm_desc,

                            itm_department = o.itm_department.ToString(),

                            itm_level = o.itm_level.ToString(),

                            itm_type = o.itm_type.ToString(),



                            itm_Unit = o.itm_Unit.unt_arName,

                            itm_sugQty = o.itm_sugQty ?? 0.0,

                            itm_presentQty = o.itm_presentQty ?? 0.0,

                            itm_isExisting = o.itm_isExisting.ToString(),
                            itm_availableMethod = o.itm_availableMethod.ToString(),

                            itm_chapter = o.itm_chapter ?? 0,

                            itm_term = o.itm_term.ToString(),

                            itm_School = o.itm_School.sch_arName,
                            itm_ValidState = o.itm_ValidState.ToString(),
                            itm_completionYear = o.itm_completionYear ?? 0,
                            itm_excessiveQty = o.itm_excessiveQty ?? 0,
                            itm_note = o.itm_note,
                            company = o.itm_School.sch_complex.comp_company.com_arName,
                            complex = o.itm_School.sch_complex.comp_arName
                        }).ToList();

                    }
                    else if (reportType == "GetOverItemsCustom")
                    {
                        model = items.Where(x => x.itm_over == Over.زائد && x.itm_type != Domain.Type.اثاث_مختبر).Select(o => new ItemViewModel
                        {

                            itm_code = o.itm_code,
                            itm_arName = o.itm_arName,
                            itm_enName = o.itm_enName,
                            itm_desc = o.itm_desc,

                            itm_department = o.itm_department.ToString(),

                            itm_level = o.itm_level.ToString(),

                            itm_type = o.itm_type.ToString(),



                            itm_Unit = o.itm_Unit.unt_arName,

                            itm_sugQty = o.itm_sugQty ?? 0.0,

                            itm_presentQty = o.itm_presentQty ?? 0.0,

                            itm_isExisting = o.itm_isExisting.ToString(),
                            itm_availableMethod = o.itm_availableMethod.ToString(),

                            itm_chapter = o.itm_chapter ?? 0,

                            itm_term = o.itm_term.ToString(),

                            itm_School = o.itm_School.sch_arName,
                            itm_ValidState = o.itm_ValidState.ToString(),
                            itm_completionYear = o.itm_completionYear ?? 0,
                            itm_excessiveQty = o.itm_excessiveQty ?? 0,
                            itm_note = o.itm_note,
                            company = o.itm_School.sch_complex.comp_company.com_arName,
                            complex = o.itm_School.sch_complex.comp_arName
                        }).ToList();

                    }
                    else if (reportType == "GetExcessiveQtyCustom")
                    {
                        model = items.Where(x => x.itm_isExisting == ExsistState.موجود && x.itm_type != Domain.Type.اثاث_مختبر).Select(o => new ItemViewModel
                        {

                            itm_code = o.itm_code,
                            itm_arName = o.itm_arName,
                            itm_enName = o.itm_enName,
                            itm_desc = o.itm_desc,

                            itm_department = o.itm_department.ToString(),

                            itm_level = o.itm_level.ToString(),

                            itm_type = o.itm_type.ToString(),



                            itm_Unit = o.itm_Unit.unt_arName,

                            itm_sugQty = o.itm_sugQty ?? 0.0,

                            itm_presentQty = o.itm_presentQty ?? 0.0,

                            itm_isExisting = o.itm_isExisting.ToString(),
                            itm_availableMethod = o.itm_availableMethod.ToString(),

                            itm_chapter = o.itm_chapter ?? 0,

                            itm_term = o.itm_term.ToString(),

                            itm_School = o.itm_School.sch_arName,
                            itm_ValidState = o.itm_ValidState.ToString(),
                            itm_completionYear = o.itm_completionYear ?? 0,
                            itm_excessiveQty = o.itm_excessiveQty ?? 0,
                            itm_note = o.itm_note,
                            company = o.itm_School.sch_complex.comp_company.com_arName,
                            complex = o.itm_School.sch_complex.comp_arName
                        }).ToList();

                    }
                    else if (reportType == "GetPresentItemsCustom")
                    {
                        model = items.Where(x => x.itm_isExisting == ExsistState.موجود && x.itm_type != Domain.Type.اثاث_مختبر && x.itm_excessiveQty > 0).Select(o => new ItemViewModel
                        {

                            itm_code = o.itm_code,
                            itm_arName = o.itm_arName,
                            itm_enName = o.itm_enName,
                            itm_desc = o.itm_desc,

                            itm_department = o.itm_department.ToString(),

                            itm_level = o.itm_level.ToString(),

                            itm_type = o.itm_type.ToString(),



                            itm_Unit = o.itm_Unit.unt_arName,

                            itm_sugQty = o.itm_sugQty ?? 0.0,

                            itm_presentQty = o.itm_presentQty ?? 0.0,

                            itm_isExisting = o.itm_isExisting.ToString(),
                            itm_availableMethod = o.itm_availableMethod.ToString(),

                            itm_chapter = o.itm_chapter ?? 0,

                            itm_term = o.itm_term.ToString(),

                            itm_School = o.itm_School.sch_arName,
                            itm_ValidState = o.itm_ValidState.ToString(),
                            itm_completionYear = o.itm_completionYear ?? 0,
                            itm_excessiveQty = o.itm_excessiveQty ?? 0,
                            itm_note = o.itm_note,
                            company = o.itm_School.sch_complex.comp_company.com_arName,
                            complex = o.itm_School.sch_complex.comp_arName
                        }).ToList();

                    }
                    else if (reportType == "GetUnAvalableItemsCustom")
                    {
                        model = items.Where(x => x.itm_isExisting == ExsistState.غير_موجود && x.itm_type != Domain.Type.اثاث_مختبر).Select(o => new ItemViewModel
                        {

                            itm_code = o.itm_code,
                            itm_arName = o.itm_arName,
                            itm_enName = o.itm_enName,
                            itm_desc = o.itm_desc,

                            itm_department = o.itm_department.ToString(),

                            itm_level = o.itm_level.ToString(),

                            itm_type = o.itm_type.ToString(),



                            itm_Unit = o.itm_Unit.unt_arName,

                            itm_sugQty = o.itm_sugQty ?? 0.0,

                            itm_presentQty = o.itm_presentQty ?? 0.0,

                            itm_isExisting = o.itm_isExisting.ToString(),
                            itm_availableMethod = o.itm_availableMethod.ToString(),

                            itm_chapter = o.itm_chapter ?? 0,

                            itm_term = o.itm_term.ToString(),

                            itm_School = o.itm_School.sch_arName,
                            itm_ValidState = o.itm_ValidState.ToString(),
                            itm_completionYear = o.itm_completionYear ?? 0,
                            itm_excessiveQty = o.itm_excessiveQty ?? 0,
                            itm_note = o.itm_note,
                            company = o.itm_School.sch_complex.comp_company.com_arName,
                            complex = o.itm_School.sch_complex.comp_arName
                        }).ToList();

                    }
                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "" + reportType + ".rpt"));
                    DataSet ds = new DataSet();
                    ds = new LogoDataSet.LogoDataSet();
                    var comlogo = _companyRepository.GetAll().FirstOrDefault().com_image;
                    var complogo = _complexRepository.GetAll().FirstOrDefault().comp_image;

                    ds.Tables[0].Rows.Add(comlogo, complogo);
                    rd.Database.Tables[0].SetDataSource(model);
                    rd.Database.Tables[1].SetDataSource(ds.Tables[0]);

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "الاصناف.pdf");
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }

            }
            catch (Exception)
            {

                return RedirectToAction("CustomReport", "Report");
            }

        }

        #endregion


        // GET: Report
        #region All Items
        [ScreenPermissionFilter(screenId = 63)]
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
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    var ModelList = _itemRepository.GetAllItemViewModel(x => x.itm_year == year && x.itm_type != Domain.Type.اثاث_مختبر && x.itm_schId == school_id);

                    //ViewBag.CurrentFilter, provides the view with the current filter string.
                    //he search string is changed when a value is entered in the text box and the submit button is pressed. In that case, the searchString parameter is not null.
                    if (searchString != null)
                    {
                        page = 1;
                    }
                    else
                    {
                        searchString = currentFilter;
                    }

                    ViewBag.CurrentFilter = searchString;



                    int schoolid = 0;
                    school_id = (int)Session["ScoolId"];
                    var model = from s in _itemRepository.GetAllItemViewModel(x => x.itm_year == year && x.itm_type != Domain.Type.اثاث_مختبر && x.itm_schId ==schoolid)
                                select s;
                    //Search and match data, if search string is not null or empty
                    if (!String.IsNullOrEmpty(searchString))
                    {
                        model = model.Where(s => s.itm_schId == school_id && (s.itm_arName.Contains(searchString)
                                               || s.itm_enName.Contains(searchString)
                                               || s.itm_desc.Contains(searchString)
                                               || s.itm_School.sch_arName.Contains(searchString)
                                               || s.itm_Unit.unt_arName.Contains(searchString)));
                    }
                    switch (sortOrder)
                    {
                        case "name_desc":
                            ModelList = model.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.Id);
                            break;

                        default:
                            ModelList = model.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.Id);
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

                return RedirectToAction("Index");
            }


        }


        public ActionResult DownloadAllItems()
        {

            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(x => x.itm_year == year && x.itm_type != Domain.Type.اثاث_مختبر && x.itm_schId ==school_id && x.itm_type != Domain.Type.اثاث_مختبر).Select(o => new ItemViewModel
                    {
                        company = o.itm_School.sch_complex.comp_company.com_arName,
                        itm_code = o.itm_code,
                        itm_arName = o.itm_arName,
                        itm_enName = o.itm_enName,
                        itm_desc = o.itm_desc,

                        itm_department = o.itm_department.ToString(),

                        itm_level = o.itm_level.ToString(),

                        itm_type = o.itm_type.ToString(),



                        itm_Unit = o.itm_Unit.unt_arName,

                        itm_sugQty = o.itm_sugQty ?? 0.0,

                        itm_presentQty = o.itm_presentQty ?? 0.0,

                        itm_isExisting = o.itm_isExisting.ToString(),
                        itm_availableMethod = o.itm_availableMethod.ToString(),

                        itm_chapter = o.itm_chapter ?? 0,

                        itm_term = o.itm_term.ToString(),

                        itm_School = o.itm_School.sch_arName,
                        itm_ValidState = o.itm_ValidState.ToString(),
                        itm_completionYear = o.itm_completionYear ?? 0,
                        itm_excessiveQty = o.itm_excessiveQty ?? 0,
                        itm_note = o.itm_note

                    }).ToList();

                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "GetItems.rpt"));
                    DataSet ds = new DataSet();
                    ds = new LogoDataSet.LogoDataSet();
                    var comlogo = _companyRepository.GetAll().FirstOrDefault().com_image;
                    var complogo = _complexRepository.GetAll().FirstOrDefault().comp_image;

                    ds.Tables[0].Rows.Add(comlogo, complogo);
                    rd.Database.Tables[0].SetDataSource(model);
                    rd.Database.Tables[1].SetDataSource(ds.Tables[0]);

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "جميع الاصناف.pdf");
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

        #endregion


        #region Primary DepartMent
        #region GetPrimaryItems
        [ScreenPermissionFilter(screenId = 63)]
        public ActionResult GetPrimaryItems(int? page)
        {
            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    var ModelList = _itemRepository.GetAllItemViewModel(x => x.itm_year == year && x.itm_type != Domain.Type.اثاث_مختبر && x.itm_schId == school_id && x.itm_department == 0);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.Id);
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
                return RedirectToAction("GetPrimaryItems");
            }



        }


        public ActionResult DownloadPrimaryItems()
        {

            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o => o.itm_year == year && o.itm_type != Domain.Type.اثاث_مختبر && o.itm_department == 0 && o.itm_schId == school_id).Select(o => new ItemViewModel
                    {
                        company = o.itm_School.sch_complex.comp_company.com_arName,
                        itm_code = o.itm_code,
                        itm_arName = o.itm_arName,
                        itm_enName = o.itm_enName,
                        itm_desc = o.itm_desc,

                        itm_department = o.itm_department.ToString(),

                        itm_level = o.itm_level.ToString(),

                        itm_type = o.itm_type.ToString(),



                        itm_Unit = o.itm_Unit.unt_arName,

                        itm_sugQty = o.itm_sugQty ?? 0.0,

                        itm_presentQty = o.itm_presentQty ?? 0.0,

                        itm_isExisting = o.itm_isExisting.ToString(),
                        itm_availableMethod = o.itm_availableMethod.ToString(),

                        itm_chapter = o.itm_chapter ?? 1,

                        itm_term = o.itm_term.ToString(),
                        itm_School = o.itm_School.sch_arName,
                        itm_ValidState = o.itm_ValidState.ToString(),
                        itm_completionYear = o.itm_completionYear ?? 0,
                        itm_excessiveQty = o.itm_excessiveQty ?? 0,
                        itm_note = o.itm_note,
                        sch_type = o.itm_School.sch_type.ToString()
                    }).ToList();

                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "GetItems.rpt"));
                    DataSet ds = new DataSet();
                    ds = new LogoDataSet.LogoDataSet();
                    var comlogo = _companyRepository.GetAll().FirstOrDefault().com_image;
                    var complogo = _complexRepository.GetAll().FirstOrDefault().comp_image;

                    ds.Tables[0].Rows.Add(comlogo, complogo);
                    rd.Database.Tables[0].SetDataSource(model);
                    rd.Database.Tables[1].SetDataSource(ds.Tables[0]);

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "اصناف المرحبة الابتدائية.pdf");
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception)
            {

                return RedirectToAction("GetPrimaryItems");
            }

        }
        #endregion

        #region GetPresentPrimaryItems
        [ScreenPermissionFilter(screenId = 64)]
        public ActionResult GetPresentPrimaryItems(int? page)
        {


            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    var ModelList = _itemRepository.GetAllItemViewModel(x => x.itm_year == year && x.itm_department == 0 && x.itm_schId == school_id && x.itm_type != Domain.Type.اثاث_مختبر && x.itm_isExisting == ExsistState.موجود);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.Id);
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

                return RedirectToAction("GetPresentPrimaryItems");
            }
        }


        public ActionResult DownloadPresentPrimaryItems()
        {


            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o => o.itm_year == year && o.itm_department == 0 && o.itm_schId == school_id && o.itm_type != Domain.Type.اثاث_مختبر && o.itm_isExisting == ExsistState.موجود).Select(o => new ItemViewModel
                    {
                        company = o.itm_School.sch_complex.comp_company.com_arName,
                        itm_code = o.itm_code,
                        itm_arName = o.itm_arName,
                        itm_enName = o.itm_enName,
                        itm_desc = o.itm_desc,

                        itm_department = o.itm_department.ToString(),

                        itm_level = o.itm_level.ToString(),

                        itm_type = o.itm_type.ToString(),



                        itm_Unit = o.itm_Unit.unt_arName,

                        itm_sugQty = o.itm_sugQty ?? 0.0,

                        itm_presentQty = o.itm_presentQty ?? 0.0,

                        itm_isExisting = o.itm_isExisting.ToString(),
                        itm_availableMethod = o.itm_availableMethod.ToString(),

                        itm_chapter = o.itm_chapter ?? 1,

                        itm_term = o.itm_term.ToString(),




                        itm_School = o.itm_School.sch_arName,
                        itm_ValidState = o.itm_ValidState.ToString(),
                        itm_completionYear = o.itm_completionYear ?? 0,
                        itm_excessiveQty = o.itm_excessiveQty ?? 0,
                        itm_note = o.itm_note,
                        sch_type = o.itm_School.sch_type.ToString()

                    }).ToList();

                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "GetPresentItems.rpt"));
                    DataSet ds = new DataSet();
                    ds = new LogoDataSet.LogoDataSet();
                    var comlogo = _companyRepository.GetAll().FirstOrDefault().com_image;
                    var complogo = _complexRepository.GetAll().FirstOrDefault().comp_image;

                    ds.Tables[0].Rows.Add(comlogo, complogo);
                    rd.Database.Tables[0].SetDataSource(model);
                    rd.Database.Tables[1].SetDataSource(ds.Tables[0]);

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "اصناف المرحبة الابتدائية.pdf");
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception)
            {

                return RedirectToAction("GetPresentPrimaryItems");
            }
        }
        #endregion

        #region GetUnValidItems
        [ScreenPermissionFilter(screenId = 64)]
        public ActionResult GetUnValidPrimaryItems(int? page)
        {


            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    var ModelList = _itemRepository.GetAllItemViewModel(x => x.itm_year == year && x.itm_department == 0 && x.itm_schId == school_id && x.itm_type != Domain.Type.اثاث_مختبر && x.itm_isExisting == ExsistState.موجود &&x.itm_ValidState==ValidState.غير_صالح);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.Id);
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

                return RedirectToAction("GetUnValidPrimaryItems");
            }
        }


        public ActionResult DownloadUnValidPrimaryItems()
        {


            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o => o.itm_year == year && o.itm_department == 0 && o.itm_schId == school_id && o.itm_type != Domain.Type.اثاث_مختبر && o.itm_isExisting == ExsistState.موجود && o.itm_ValidState == ValidState.غير_صالح).Select(o => new ItemViewModel
                    {
                        company = o.itm_School.sch_complex.comp_company.com_arName,
                        itm_code = o.itm_code,
                        itm_arName = o.itm_arName,
                        itm_enName = o.itm_enName,
                        itm_desc = o.itm_desc,

                        itm_department = o.itm_department.ToString(),

                        itm_level = o.itm_level.ToString(),

                        itm_type = o.itm_type.ToString(),



                        itm_Unit = o.itm_Unit.unt_arName,

                        itm_sugQty = o.itm_sugQty ?? 0.0,

                        itm_presentQty = o.itm_presentQty ?? 0.0,

                        itm_isExisting = o.itm_isExisting.ToString(),
                        itm_availableMethod = o.itm_availableMethod.ToString(),

                        itm_chapter = o.itm_chapter ?? 1,

                        itm_term = o.itm_term.ToString(),




                        itm_School = o.itm_School.sch_arName,
                        itm_ValidState = o.itm_ValidState.ToString(),
                        itm_completionYear = o.itm_completionYear ?? 0,
                        itm_excessiveQty = o.itm_excessiveQty ?? 0,
                        itm_note = o.itm_note,
                        sch_type = o.itm_School.sch_type.ToString()

                    }).ToList();

                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "GetPresentItems.rpt"));
                    DataSet ds = new DataSet();
                    ds = new LogoDataSet.LogoDataSet();
                    var comlogo = _companyRepository.GetAll().FirstOrDefault().com_image;
                    var complogo = _complexRepository.GetAll().FirstOrDefault().comp_image;

                    ds.Tables[0].Rows.Add(comlogo, complogo);
                    rd.Database.Tables[0].SetDataSource(model);
                    rd.Database.Tables[1].SetDataSource(ds.Tables[0]);

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "اصناف المرحبة الابتدائية.pdf");
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception)
            {

                return RedirectToAction("GetPresentPrimaryItems");
            }
        }
        #endregion

        #region GetUnAvalablePrimaryItems
        [ScreenPermissionFilter(screenId = 65)]
        public ActionResult GetUnAvalablePrimaryItems(int? page)
        {

            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];

                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    var ModelList = _itemRepository.GetAllItemViewModel(x => x.itm_year == year && x.itm_department == 0 && x.itm_schId == school_id && x.itm_type != Domain.Type.اثاث_مختبر && x.itm_isExisting == ExsistState.غير_موجود);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.Id);

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
                return RedirectToAction("GetUnAvalablePrimaryItems");
            }

        }


        public ActionResult DownloadUnAvalablePrimaryItems()
        {


            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o => o.itm_year == year && o.itm_department == 0 && o.itm_schId == school_id&& o.itm_type != Domain.Type.اثاث_مختبر && o.itm_isExisting == ExsistState.غير_موجود).Select(o => new ItemViewModel
                    {
                        company = o.itm_School.sch_complex.comp_company.com_arName,
                        itm_code = o.itm_code,
                        itm_arName = o.itm_arName,
                        itm_enName = o.itm_enName,
                        itm_desc = o.itm_desc,

                        itm_department = o.itm_department.ToString(),

                        itm_level = o.itm_level.ToString(),

                        itm_type = o.itm_type.ToString(),



                        itm_Unit = o.itm_Unit.unt_arName,

                        itm_sugQty = o.itm_sugQty ?? 0.0,

                        itm_presentQty = o.itm_presentQty ?? 0.0,

                        itm_isExisting = o.itm_isExisting.ToString(),
                        itm_availableMethod = o.itm_availableMethod.ToString(),

                        itm_chapter = o.itm_chapter ?? 1,

                        itm_term = o.itm_term.ToString(),
                        itm_School = o.itm_School.sch_arName,
                        itm_ValidState = o.itm_ValidState.ToString(),
                        itm_completionYear = o.itm_completionYear ?? 0,
                        itm_excessiveQty = o.itm_excessiveQty ?? 0,
                        itm_note = o.itm_note,
                        sch_type = o.itm_School.sch_type.ToString()

                    }).ToList();
                    var y = model.Count();
                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "GetUnAvalableItems.rpt"));
                    DataSet ds = new DataSet();
                    ds = new LogoDataSet.LogoDataSet();
                    var comlogo = _companyRepository.GetAll().FirstOrDefault().com_image;
                    var complogo = _complexRepository.GetAll().FirstOrDefault().comp_image;

                    ds.Tables[0].Rows.Add(comlogo, complogo);
                    rd.Database.Tables[0].SetDataSource(model);
                    rd.Database.Tables[1].SetDataSource(ds.Tables[0]);

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "اصناف المرحبة الابتدائية.pdf");
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception)
            {

                return RedirectToAction("GetUnAvalablePrimaryItems");
            }
        }
        #endregion

        #region Get Over Primary Items
        [ScreenPermissionFilter(screenId = 68)]
        public ActionResult GetOverPrimaryItems(int? page)
        {


            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    var ModelList = _itemRepository.GetAllItemViewModel(x => x.itm_year == year && x.itm_department == 0 && x.itm_schId == school_id && x.itm_type != Domain.Type.اثاث_مختبر && x.itm_over == Over.زائد);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.Id);
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

                return RedirectToAction("GetOverPrimaryItems");
            }
        }


        public ActionResult DownloadOverPrimaryItems()
        {

            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];

                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o => o.itm_year == year && o.itm_department == 0 && o.itm_schId ==school_id && o.itm_type != Domain.Type.اثاث_مختبر && o.itm_over == Over.زائد).Select(o => new ItemViewModel
                    {
                        company = o.itm_School.sch_complex.comp_company.com_arName,
                        itm_code = o.itm_code,
                        itm_arName = o.itm_arName,
                        itm_enName = o.itm_enName,
                        itm_desc = o.itm_desc,

                        itm_department = o.itm_department.ToString(),

                        itm_level = o.itm_level.ToString(),

                        itm_type = o.itm_type.ToString(),



                        itm_Unit = o.itm_Unit.unt_arName,

                        itm_sugQty = o.itm_sugQty ?? 0.0,

                        itm_presentQty = o.itm_presentQty ?? 0.0,

                        itm_isExisting = o.itm_isExisting.ToString(),
                        itm_availableMethod = o.itm_availableMethod.ToString(),

                        itm_chapter = o.itm_chapter ?? 1,

                        itm_term = o.itm_term.ToString(),
                        itm_School = o.itm_School.sch_arName,
                        itm_ValidState = o.itm_ValidState.ToString(),
                        itm_completionYear = o.itm_completionYear ?? 0,
                        itm_excessiveQty = o.itm_excessiveQty ?? 0,
                        itm_note = o.itm_note,
                        sch_type = o.itm_School.sch_type.ToString()

                    }).ToList();

                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "GetOverItems.rpt"));
                    DataSet ds = new DataSet();
                    ds = new LogoDataSet.LogoDataSet();
                    var comlogo = _companyRepository.GetAll().FirstOrDefault().com_image;
                    var complogo = _complexRepository.GetAll().FirstOrDefault().comp_image;

                    ds.Tables[0].Rows.Add(comlogo, complogo);
                    rd.Database.Tables[0].SetDataSource(model);
                    rd.Database.Tables[1].SetDataSource(ds.Tables[0]);

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "اصناف المرحبة الابتدائية.pdf");
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception)
            {

                return RedirectToAction("GetOverPrimaryItems");
            }
        }
        #endregion

        #region Get Wanted Primary Items
        [ScreenPermissionFilter(screenId = 66)]
        public ActionResult GetWantedPrimaryItems(int? page)
        {

            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    var ModelList = _itemRepository.GetAllItemViewModel(x => x.itm_year == year && x.itm_availableMethod == 0 && x.itm_department == 0 && x.itm_schId ==school_id&& x.itm_type != Domain.Type.اثاث_مختبر && x.itm_isExisting == ExsistState.غير_موجود);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.Id);
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

                return RedirectToAction("GetWantedPrimaryItems");
            }

        }


        public ActionResult DownloadWantedPrimaryItems()
        {


            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];

                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o => o.itm_year == year && o.itm_availableMethod == 0 && o.itm_department == 0 && o.itm_schId == school_id && o.itm_type != Domain.Type.اثاث_مختبر && o.itm_isExisting == ExsistState.غير_موجود).Select(o => new ItemViewModel
                    {
                        company = o.itm_School.sch_complex.comp_company.com_arName,
                        itm_code = o.itm_code,
                        itm_arName = o.itm_arName,
                        itm_enName = o.itm_enName,
                        itm_desc = o.itm_desc,

                        itm_department = o.itm_department.ToString(),

                        itm_level = o.itm_level.ToString(),

                        itm_type = o.itm_type.ToString(),



                        itm_Unit = o.itm_Unit.unt_arName,

                        itm_sugQty = o.itm_sugQty ?? 0.0,

                        itm_presentQty = o.itm_presentQty ?? 0.0,

                        itm_isExisting = o.itm_isExisting.ToString(),
                        itm_availableMethod = o.itm_availableMethod.ToString(),

                        itm_chapter = o.itm_chapter ?? 1,

                        itm_term = o.itm_term.ToString(),
                        itm_School = o.itm_School.sch_arName,
                        itm_ValidState = o.itm_ValidState.ToString(),
                        itm_completionYear = o.itm_completionYear ?? 0,
                        itm_excessiveQty = o.itm_excessiveQty ?? 0,
                        itm_note = o.itm_note,
                        sch_type = o.itm_School.sch_type.ToString()

                    }).ToList();

                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "GetWantedItems.rpt"));
                    DataSet ds = new DataSet();
                    ds = new LogoDataSet.LogoDataSet();
                    var comlogo = _companyRepository.GetAll().FirstOrDefault().com_image;
                    var complogo = _complexRepository.GetAll().FirstOrDefault().comp_image;

                    ds.Tables[0].Rows.Add(comlogo, complogo);
                    rd.Database.Tables[0].SetDataSource(model);
                    rd.Database.Tables[1].SetDataSource(ds.Tables[0]);

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "اصناف المرحبة الابتدائية.pdf");
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception)
            {

                return RedirectToAction("GetWantedPrimaryItems");
            }
        }
        #endregion


        #region Get Can Avalable Primary Items
        [ScreenPermissionFilter(screenId = 67)]
        public ActionResult GetCanAvalablePrimaryItems(int? page)
        {



            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    var ModelList = _itemRepository.GetAllItemViewModel(x => x.itm_year == year && x.itm_availableMethod == AvailableMethod.المدرسة && x.itm_department == 0 && x.itm_schId == school_id&& x.itm_type != Domain.Type.اثاث_مختبر && x.itm_isExisting == ExsistState.غير_موجود);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.Id);
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

                return RedirectToAction("GetCanAvalablePrimaryItems");
            }

        }


        public ActionResult DownloadCanAvalablePrimaryItems()
        {

            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];

                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o => o.itm_year == year && o.itm_availableMethod == AvailableMethod.المدرسة && o.itm_department == 0 && o.itm_schId == school_id && o.itm_type != Domain.Type.اثاث_مختبر && o.itm_isExisting == ExsistState.غير_موجود).Select(o => new ItemViewModel
                    {
                        company = o.itm_School.sch_complex.comp_company.com_arName,
                        itm_code = o.itm_code,
                        itm_arName = o.itm_arName,
                        itm_enName = o.itm_enName,
                        itm_desc = o.itm_desc,

                        itm_department = o.itm_department.ToString(),

                        itm_level = o.itm_level.ToString(),

                        itm_type = o.itm_type.ToString(),



                        itm_Unit = o.itm_Unit.unt_arName,

                        itm_sugQty = o.itm_sugQty ?? 0.0,

                        itm_presentQty = o.itm_presentQty ?? 0.0,

                        itm_isExisting = o.itm_isExisting.ToString(),
                        itm_availableMethod = o.itm_availableMethod.ToString(),

                        itm_chapter = o.itm_chapter ?? 1,

                        itm_term = o.itm_term.ToString(),
                        itm_School = o.itm_School.sch_arName,
                        itm_ValidState = o.itm_ValidState.ToString(),
                        itm_completionYear = o.itm_completionYear ?? 0,
                        itm_excessiveQty = o.itm_excessiveQty ?? 0,
                        itm_note = o.itm_note,
                        sch_type = o.itm_School.sch_type.ToString()

                    }).ToList();

                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "GetCanAvalableItems.rpt"));
                    DataSet ds = new DataSet();
                    ds = new LogoDataSet.LogoDataSet();
                    var comlogo = _companyRepository.GetAll().FirstOrDefault().com_image;
                    var complogo = _complexRepository.GetAll().FirstOrDefault().comp_image;

                    ds.Tables[0].Rows.Add(comlogo, complogo);
                    rd.Database.Tables[0].SetDataSource(model);
                    rd.Database.Tables[1].SetDataSource(ds.Tables[0]);

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "اصناف المرحبة الابتدائية.pdf");
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception)
            {

                return RedirectToAction("GetCanAvalablePrimaryItems");
            }
        }
        #endregion

        #region GetexcessiveQtyPrimaryItems
        [ScreenPermissionFilter(screenId = 69)]
        public ActionResult GetexcessiveQtyPrimaryItems(int? page)
        {


            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    var ModelList = _itemRepository.GetAllItemViewModel(x => x.itm_year == year && x.itm_department == 0 && x.itm_schId == school_id&& x.itm_type != Domain.Type.اثاث_مختبر && x.itm_excessiveQty > 0 && x.itm_isExisting == ExsistState.موجود);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.Id);
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

                return RedirectToAction("GetexcessiveQtyPrimaryItems");
            }
        }


        public ActionResult DownloadexcessiveQtyPrimaryItems()
        {


            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o => o.itm_year == year && o.itm_department == 0 && o.itm_schId == school_id && o.itm_type != Domain.Type.اثاث_مختبر && o.itm_excessiveQty > 0 && o.itm_isExisting == ExsistState.موجود).Select(o => new ItemViewModel
                    {
                        company = o.itm_School.sch_complex.comp_company.com_arName,
                        itm_code = o.itm_code,
                        itm_arName = o.itm_arName,
                        itm_enName = o.itm_enName,
                        itm_desc = o.itm_desc,

                        itm_department = o.itm_department.ToString(),

                        itm_level = o.itm_level.ToString(),

                        itm_type = o.itm_type.ToString(),



                        itm_Unit = o.itm_Unit.unt_arName,

                        itm_sugQty = o.itm_sugQty ?? 0.0,

                        itm_presentQty = o.itm_presentQty ?? 0.0,

                        itm_isExisting = o.itm_isExisting.ToString(),
                        itm_availableMethod = o.itm_availableMethod.ToString(),

                        itm_chapter = o.itm_chapter ?? 1,

                        itm_term = o.itm_term.ToString(),




                        itm_School = o.itm_School.sch_arName,
                        itm_ValidState = o.itm_ValidState.ToString(),
                        itm_completionYear = o.itm_completionYear ?? 0,
                        itm_excessiveQty = o.itm_excessiveQty ?? 0,
                        itm_note = o.itm_note,
                        sch_type = o.itm_School.sch_type.ToString()

                    }).ToList();

                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "GetExcessiveQty.rpt"));
                    DataSet ds = new DataSet();
                    ds = new LogoDataSet.LogoDataSet();
                    var comlogo = _companyRepository.GetAll().FirstOrDefault().com_image;
                    var complogo = _complexRepository.GetAll().FirstOrDefault().comp_image;

                    ds.Tables[0].Rows.Add(comlogo, complogo);
                    rd.Database.Tables[0].SetDataSource(model);
                    rd.Database.Tables[1].SetDataSource(ds.Tables[0]);

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "اصناف المرحبة الابتدائية.pdf");
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception)
            {

                return RedirectToAction("GetexcessiveQtyPrimaryItems");
            }
        }
        #endregion



        #endregion



        #region Secondry DepartMent
        #region GetSecondryItems
        [ScreenPermissionFilter(screenId = 63)]
        public ActionResult GetSecondryItems(int? page)
        {


            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    var ModelList = _itemRepository.GetAllItemViewModel(x => x.itm_year == year && x.itm_type != Domain.Type.اثاث_مختبر && x.itm_schId == school_id && x.itm_department == Department.متوسط);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.Id);
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

                return RedirectToAction("GetSecondryItems");
            }
        }


        public ActionResult DownloadSecondryItems()
        {

            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];

                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o => o.itm_year == year && o.itm_type != Domain.Type.اثاث_مختبر && o.itm_schId == school_id && o.itm_department == Department.متوسط).Select(o => new ItemViewModel
                    {
                        company = o.itm_School.sch_complex.comp_company.com_arName,
                        itm_code = o.itm_code,
                        itm_arName = o.itm_arName,
                        itm_enName = o.itm_enName,
                        itm_desc = o.itm_desc,

                        itm_department = o.itm_department.ToString(),

                        itm_level = o.itm_level.ToString(),

                        itm_type = o.itm_type.ToString(),



                        itm_Unit = o.itm_Unit.unt_arName,

                        itm_sugQty = o.itm_sugQty ?? 0.0,

                        itm_presentQty = o.itm_presentQty ?? 0.0,

                        itm_isExisting = o.itm_isExisting.ToString(),
                        itm_availableMethod = o.itm_availableMethod.ToString(),

                        itm_chapter = o.itm_chapter ?? 1,

                        itm_term = o.itm_term.ToString(),




                        itm_School = o.itm_School.sch_arName,

                    }).ToList();

                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "GetItems.rpt"));
                    DataSet ds = new DataSet();
                    ds = new LogoDataSet.LogoDataSet();
                    var comlogo = _companyRepository.GetAll().FirstOrDefault().com_image;
                    var complogo = _complexRepository.GetAll().FirstOrDefault().comp_image;

                    ds.Tables[0].Rows.Add(comlogo, complogo);
                    rd.Database.Tables[0].SetDataSource(model);
                    rd.Database.Tables[1].SetDataSource(ds.Tables[0]);

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "اصناف المرحبة المتوسطة.pdf");
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception)
            {

                return RedirectToAction("GetSecondryItems");
            }
        }
        #endregion

        #region GetPresentSecondryItems
        [ScreenPermissionFilter(screenId = 64)]
        public ActionResult GetPresentSecondryItems(int? page)
        {


            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    var ModelList = _itemRepository.GetAllItemViewModel(x => x.itm_year == year && x.itm_type != Domain.Type.اثاث_مختبر && x.itm_schId ==school_id && x.itm_department == Department.متوسط && x.itm_isExisting == ExsistState.موجود);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.Id);
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

                return RedirectToAction("GetPresentSecondryItems");
            }

        }


        public ActionResult DownloadPresentSecondryItems()
        {


            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o => o.itm_year == year && o.itm_type != Domain.Type.اثاث_مختبر && o.itm_schId == school_id && o.itm_department == Department.متوسط && o.itm_isExisting == ExsistState.موجود).Select(o => new ItemViewModel
                    {
                        company = o.itm_School.sch_complex.comp_company.com_arName,
                        itm_code = o.itm_code,
                        itm_arName = o.itm_arName,
                        itm_enName = o.itm_enName,
                        itm_desc = o.itm_desc,

                        itm_department = o.itm_department.ToString(),

                        itm_level = o.itm_level.ToString(),

                        itm_type = o.itm_type.ToString(),



                        itm_Unit = o.itm_Unit.unt_arName,

                        itm_sugQty = o.itm_sugQty ?? 0.0,

                        itm_presentQty = o.itm_presentQty ?? 0.0,

                        itm_isExisting = o.itm_isExisting.ToString(),
                        itm_availableMethod = o.itm_availableMethod.ToString(),

                        itm_chapter = o.itm_chapter ?? 1,

                        itm_term = o.itm_term.ToString(),
                        itm_School = o.itm_School.sch_arName,
                        itm_ValidState = o.itm_ValidState.ToString(),
                        itm_completionYear = o.itm_completionYear ?? 0,
                        itm_excessiveQty = o.itm_excessiveQty ?? 0,
                        itm_note = o.itm_note,
                        sch_type = o.itm_School.sch_type.ToString()

                    }).ToList();

                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "GetPresentItems.rpt"));
                    DataSet ds = new DataSet();
                    ds = new LogoDataSet.LogoDataSet();
                    var comlogo = _companyRepository.GetAll().FirstOrDefault().com_image;
                    var complogo = _complexRepository.GetAll().FirstOrDefault().comp_image;

                    ds.Tables[0].Rows.Add(comlogo, complogo);
                    rd.Database.Tables[0].SetDataSource(model);
                    rd.Database.Tables[1].SetDataSource(ds.Tables[0]);

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "اصناف المرحبة المتوسطة.pdf");
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception)
            {

                return RedirectToAction("GetPresentSecondryItems");
            }
        }
        #endregion

        #region GetValidSecondryItems
        [ScreenPermissionFilter(screenId = 64)]
        public ActionResult GetUnValidSecondryItems(int? page)
        {


            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    var ModelList = _itemRepository.GetAllItemViewModel(x => x.itm_year == year && x.itm_type != Domain.Type.اثاث_مختبر && x.itm_schId == school_id && x.itm_department == Department.متوسط && x.itm_isExisting == ExsistState.موجود &&x.itm_ValidState==ValidState.غير_صالح);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.Id);
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

                return RedirectToAction("GetUnValidSecondryItems");
            }

        }


        public ActionResult DownloadUnValidSecondryItems()
        {


            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o => o.itm_year == year && o.itm_type != Domain.Type.اثاث_مختبر && o.itm_schId == school_id && o.itm_department == Department.متوسط && o.itm_isExisting == ExsistState.موجود &&o.itm_ValidState==ValidState.غير_صالح).Select(o => new ItemViewModel
                    {
                        company = o.itm_School.sch_complex.comp_company.com_arName,
                        itm_code = o.itm_code,
                        itm_arName = o.itm_arName,
                        itm_enName = o.itm_enName,
                        itm_desc = o.itm_desc,

                        itm_department = o.itm_department.ToString(),

                        itm_level = o.itm_level.ToString(),

                        itm_type = o.itm_type.ToString(),



                        itm_Unit = o.itm_Unit.unt_arName,

                        itm_sugQty = o.itm_sugQty ?? 0.0,

                        itm_presentQty = o.itm_presentQty ?? 0.0,

                        itm_isExisting = o.itm_isExisting.ToString(),
                        itm_availableMethod = o.itm_availableMethod.ToString(),

                        itm_chapter = o.itm_chapter ?? 1,

                        itm_term = o.itm_term.ToString(),
                        itm_School = o.itm_School.sch_arName,
                        itm_ValidState = o.itm_ValidState.ToString(),
                        itm_completionYear = o.itm_completionYear ?? 0,
                        itm_excessiveQty = o.itm_excessiveQty ?? 0,
                        itm_note = o.itm_note,
                        sch_type = o.itm_School.sch_type.ToString()

                    }).ToList();

                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "GetUnValidItems.rpt"));
                    DataSet ds = new DataSet();
                    ds = new LogoDataSet.LogoDataSet();
                    var comlogo = _companyRepository.GetAll().FirstOrDefault().com_image;
                    var complogo = _complexRepository.GetAll().FirstOrDefault().comp_image;

                    ds.Tables[0].Rows.Add(comlogo, complogo);
                    rd.Database.Tables[0].SetDataSource(model);
                    rd.Database.Tables[1].SetDataSource(ds.Tables[0]);

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "اصناف المرحبة المتوسطة.pdf");
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception)
            {

                return RedirectToAction("GetUnValidSecondryItems");
            }
        }
        #endregion

        #region GetUnAvalableSecondryItems
        [ScreenPermissionFilter(screenId = 65)]
        public ActionResult GetUnAvalableSecondryItems(int? page)
        {


            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    var ModelList = _itemRepository.GetAllItemViewModel(x => x.itm_year == year && x.itm_type != Domain.Type.اثاث_مختبر && x.itm_schId == school_id && x.itm_department == Department.متوسط && x.itm_isExisting == ExsistState.غير_موجود);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.Id);
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

                return RedirectToAction("GetUnAvalableSecondryItems");
            }
        }


        public ActionResult DownloadUnAvalableSecondryItems()
        {

            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o => o.itm_year == year && o.itm_type != Domain.Type.اثاث_مختبر && o.itm_schId ==school_id && o.itm_department == Department.متوسط && o.itm_isExisting == ExsistState.غير_موجود).Select(o => new ItemViewModel
                    {
                        company = o.itm_School.sch_complex.comp_company.com_arName,
                        itm_code = o.itm_code,
                        itm_arName = o.itm_arName,
                        itm_enName = o.itm_enName,
                        itm_desc = o.itm_desc,

                        itm_department = o.itm_department.ToString(),

                        itm_level = o.itm_level.ToString(),

                        itm_type = o.itm_type.ToString(),



                        itm_Unit = o.itm_Unit.unt_arName,

                        itm_sugQty = o.itm_sugQty ?? 0.0,

                        itm_presentQty = o.itm_presentQty ?? 0.0,

                        itm_isExisting = o.itm_isExisting.ToString(),
                        itm_availableMethod = o.itm_availableMethod.ToString(),

                        itm_chapter = o.itm_chapter ?? 1,

                        itm_term = o.itm_term.ToString(),
                        itm_School = o.itm_School.sch_arName,
                        itm_ValidState = o.itm_ValidState.ToString(),
                        itm_completionYear = o.itm_completionYear ?? 0,
                        itm_excessiveQty = o.itm_excessiveQty ?? 0,
                        itm_note = o.itm_note,
                        sch_type = o.itm_School.sch_type.ToString()

                    }).ToList();

                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "GetUnAvalableItems.rpt"));
                    DataSet ds = new DataSet();
                    ds = new LogoDataSet.LogoDataSet();
                    var comlogo = _companyRepository.GetAll().FirstOrDefault().com_image;
                    var complogo = _complexRepository.GetAll().FirstOrDefault().comp_image;

                    ds.Tables[0].Rows.Add(comlogo, complogo);
                    rd.Database.Tables[0].SetDataSource(model);
                    rd.Database.Tables[1].SetDataSource(ds.Tables[0]);

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "اصناف المرحبة المتوسطة.pdf");
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception)
            {

                return RedirectToAction("GetUnAvalableSecondryItems");
            }

        }
        #endregion

        #region Get Over  SecondryItems
        [ScreenPermissionFilter(screenId = 68)]
        public ActionResult GetOverSecondryItems(int? page)
        {

            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    var ModelList = _itemRepository.GetAllItemViewModel(x => x.itm_year == year && x.itm_type != Domain.Type.اثاث_مختبر && x.itm_schId ==school_id && x.itm_department == Department.متوسط && x.itm_over == Over.زائد);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.Id);
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

                return RedirectToAction("GetOverSecondryItems");
            }

        }


        public ActionResult DownloadOverSecondryItems()
        {


            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int schoolId = 0;
                    schoolId = (int)Session["ScoolId"];
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o => o.itm_year == year && o.itm_type != Domain.Type.اثاث_مختبر && o.itm_schId == schoolId && o.itm_department == Department.متوسط && o.itm_over == Over.زائد).Select(o => new ItemViewModel
                    {
                        company = o.itm_School.sch_complex.comp_company.com_arName,
                        itm_code = o.itm_code,
                        itm_arName = o.itm_arName,
                        itm_enName = o.itm_enName,
                        itm_desc = o.itm_desc,

                        itm_department = o.itm_department.ToString(),

                        itm_level = o.itm_level.ToString(),

                        itm_type = o.itm_type.ToString(),



                        itm_Unit = o.itm_Unit.unt_arName,

                        itm_sugQty = o.itm_sugQty ?? 0.0,

                        itm_presentQty = o.itm_presentQty ?? 0.0,

                        itm_isExisting = o.itm_isExisting.ToString(),
                        itm_availableMethod = o.itm_availableMethod.ToString(),

                        itm_chapter = o.itm_chapter ?? 1,

                        itm_term = o.itm_term.ToString(),
                        itm_School = o.itm_School.sch_arName,
                        itm_ValidState = o.itm_ValidState.ToString(),
                        itm_completionYear = o.itm_completionYear ?? 0,
                        itm_excessiveQty = o.itm_excessiveQty ?? 0,
                        itm_note = o.itm_note,
                        sch_type = o.itm_School.sch_type.ToString()

                    }).ToList();

                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "GetOverItems.rpt"));
                    DataSet ds = new DataSet();
                    ds = new LogoDataSet.LogoDataSet();
                    var comlogo = _companyRepository.GetAll().FirstOrDefault().com_image;
                    var complogo = _complexRepository.GetAll().FirstOrDefault().comp_image;

                    ds.Tables[0].Rows.Add(comlogo, complogo);
                    rd.Database.Tables[0].SetDataSource(model);
                    rd.Database.Tables[1].SetDataSource(ds.Tables[0]);

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "اصناف المرحبة المتوسطة.pdf");
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception)
            {

                return RedirectToAction("GetOverSecondryItems");
            }
        }
        #endregion

        #region Get Wanted  SecondryItems
        [ScreenPermissionFilter(screenId = 66)]
        public ActionResult GetWantedSecondryItems(int? page)
        {


            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int schoolId = 0;
                    schoolId = (int)Session["ScoolId"];
                    var ModelList = _itemRepository.GetAllItemViewModel(x => x.itm_year == year && x.itm_availableMethod == AvailableMethod.المستودع && x.itm_schId == schoolId && x.itm_type != Domain.Type.اثاث_مختبر && x.itm_department == Department.متوسط && x.itm_isExisting == ExsistState.غير_موجود);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.Id);
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

                return RedirectToAction("GetWantedSecondryItems");
            }
        }


        public ActionResult DownloadWantedSecondryItems()
        {


            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int schoolId = 0;
                    schoolId = (int)Session["ScoolId"];
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o => o.itm_year == year && o.itm_availableMethod == AvailableMethod.المستودع && o.itm_schId == schoolId && o.itm_type != Domain.Type.اثاث_مختبر && o.itm_department == Department.متوسط && o.itm_isExisting == ExsistState.غير_موجود).Select(o => new ItemViewModel
                    {
                        company = o.itm_School.sch_complex.comp_company.com_arName,
                        itm_code = o.itm_code,
                        itm_arName = o.itm_arName,
                        itm_enName = o.itm_enName,
                        itm_desc = o.itm_desc,

                        itm_department = o.itm_department.ToString(),

                        itm_level = o.itm_level.ToString(),

                        itm_type = o.itm_type.ToString(),



                        itm_Unit = o.itm_Unit.unt_arName,

                        itm_sugQty = o.itm_sugQty ?? 0.0,

                        itm_presentQty = o.itm_presentQty ?? 0.0,

                        itm_isExisting = o.itm_isExisting.ToString(),
                        itm_availableMethod = o.itm_availableMethod.ToString(),

                        itm_chapter = o.itm_chapter ?? 1,

                        itm_term = o.itm_term.ToString(),
                        itm_School = o.itm_School.sch_arName,
                        itm_ValidState = o.itm_ValidState.ToString(),
                        itm_completionYear = o.itm_completionYear ?? 0,
                        itm_excessiveQty = o.itm_excessiveQty ?? 0,
                        itm_note = o.itm_note,
                        sch_type = o.itm_School.sch_type.ToString()

                    }).ToList();

                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "GetWantedItems.rpt"));
                    DataSet ds = new DataSet();
                    ds = new LogoDataSet.LogoDataSet();
                    var comlogo = _companyRepository.GetAll().FirstOrDefault().com_image;
                    var complogo = _complexRepository.GetAll().FirstOrDefault().comp_image;

                    ds.Tables[0].Rows.Add(comlogo, complogo);
                    rd.Database.Tables[0].SetDataSource(model);
                    rd.Database.Tables[1].SetDataSource(ds.Tables[0]);

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "اصناف المرحبة المتوسطة.pdf");
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception)
            {

                return RedirectToAction("GetWantedSecondryItems");
            }
        }
        #endregion
        #region Get CanAvalable  SecondryItems
        [ScreenPermissionFilter(screenId = 67)]
        public ActionResult GetCanAvalableSecondryItems(int? page)
        {

            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int schoolId = 0;
                    schoolId = (int)Session["ScoolId"];
                    var ModelList = _itemRepository.GetAllItemViewModel(x => x.itm_year == year && x.itm_availableMethod == AvailableMethod.المدرسة && x.itm_schId == schoolId && x.itm_type != Domain.Type.اثاث_مختبر && x.itm_department == Department.متوسط && x.itm_isExisting == ExsistState.غير_موجود);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.Id);
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

                return RedirectToAction("GetCanAvalableSecondryItems");
            }

        }


        public ActionResult DownloadCanAvalableSecondryItems()
        {

            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int schoolId = 0;
                    schoolId = (int)Session["ScoolId"];
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o => o.itm_year == year && o.itm_availableMethod == AvailableMethod.المدرسة && o.itm_schId == schoolId && o.itm_type != Domain.Type.اثاث_مختبر && o.itm_department == Department.متوسط && o.itm_isExisting == ExsistState.غير_موجود).Select(o => new ItemViewModel
                    {
                        company = o.itm_School.sch_complex.comp_company.com_arName,
                        itm_code = o.itm_code,
                        itm_arName = o.itm_arName,
                        itm_enName = o.itm_enName,
                        itm_desc = o.itm_desc,

                        itm_department = o.itm_department.ToString(),

                        itm_level = o.itm_level.ToString(),

                        itm_type = o.itm_type.ToString(),



                        itm_Unit = o.itm_Unit.unt_arName,

                        itm_sugQty = o.itm_sugQty ?? 0.0,

                        itm_presentQty = o.itm_presentQty ?? 0.0,

                        itm_isExisting = o.itm_isExisting.ToString(),
                        itm_availableMethod = o.itm_availableMethod.ToString(),

                        itm_chapter = o.itm_chapter ?? 1,

                        itm_term = o.itm_term.ToString(),
                        itm_School = o.itm_School.sch_arName,
                        itm_ValidState = o.itm_ValidState.ToString(),
                        itm_completionYear = o.itm_completionYear ?? 0,
                        itm_excessiveQty = o.itm_excessiveQty ?? 0,
                        itm_note = o.itm_note,
                        sch_type = o.itm_School.sch_type.ToString()

                    }).ToList();

                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "GetCanAvalableItems.rpt"));
                    DataSet ds = new DataSet();
                    ds = new LogoDataSet.LogoDataSet();
                    var comlogo = _companyRepository.GetAll().FirstOrDefault().com_image;
                    var complogo = _complexRepository.GetAll().FirstOrDefault().comp_image;

                    ds.Tables[0].Rows.Add(comlogo, complogo);
                    rd.Database.Tables[0].SetDataSource(model);
                    rd.Database.Tables[1].SetDataSource(ds.Tables[0]);

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "اصناف المرحبة المتوسطة.pdf");
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception)
            {

                return RedirectToAction("GetCanAvalableSecondryItems");
            }

        }
        #endregion

        #region GetexcessiveQtySecondryItems
        [ScreenPermissionFilter(screenId = 68)]
        public ActionResult GetexcessiveQtySecondryItems(int? page)
        {


            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int schoolId = 0;
                    schoolId = (int)Session["ScoolId"];
                    var ModelList = _itemRepository.GetAllItemViewModel(x => x.itm_year == year && x.itm_department == Department.متوسط && x.itm_schId == schoolId && x.itm_type != Domain.Type.اثاث_مختبر && x.itm_excessiveQty > 0 && x.itm_isExisting == ExsistState.موجود);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.Id);
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

                return RedirectToAction("GetexcessiveQtySecondryItems");
            }
        }


        public ActionResult DownloadexcessiveQtySecondryItems()
        {


            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int schoolId = 0;
                    schoolId = (int)Session["ScoolId"];
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o => o.itm_year == year && o.itm_department == Department.متوسط && o.itm_schId == schoolId && o.itm_type != Domain.Type.اثاث_مختبر && o.itm_excessiveQty > 0 && o.itm_isExisting == ExsistState.موجود).Select(o => new ItemViewModel
                    {
                        company = o.itm_School.sch_complex.comp_company.com_arName,
                        itm_code = o.itm_code,
                        itm_arName = o.itm_arName,
                        itm_enName = o.itm_enName,
                        itm_desc = o.itm_desc,

                        itm_department = o.itm_department.ToString(),

                        itm_level = o.itm_level.ToString(),

                        itm_type = o.itm_type.ToString(),



                        itm_Unit = o.itm_Unit.unt_arName,

                        itm_sugQty = o.itm_sugQty ?? 0.0,

                        itm_presentQty = o.itm_presentQty ?? 0.0,

                        itm_isExisting = o.itm_isExisting.ToString(),
                        itm_availableMethod = o.itm_availableMethod.ToString(),

                        itm_chapter = o.itm_chapter ?? 1,

                        itm_term = o.itm_term.ToString(),




                        itm_School = o.itm_School.sch_arName,
                        itm_ValidState = o.itm_ValidState.ToString(),
                        itm_completionYear = o.itm_completionYear ?? 0,
                        itm_excessiveQty = o.itm_excessiveQty ?? 0,
                        itm_note = o.itm_note,
                        sch_type = o.itm_School.sch_type.ToString()

                    }).ToList();

                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "GetExcessiveQty.rpt"));
                    DataSet ds = new DataSet();
                    ds = new LogoDataSet.LogoDataSet();
                    var comlogo = _companyRepository.GetAll().FirstOrDefault().com_image;
                    var complogo = _complexRepository.GetAll().FirstOrDefault().comp_image;

                    ds.Tables[0].Rows.Add(comlogo, complogo);
                    rd.Database.Tables[0].SetDataSource(model);
                    rd.Database.Tables[1].SetDataSource(ds.Tables[0]);

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "اصناف المرحبة الابتدائية.pdf");
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception)
            {

                return RedirectToAction("GetexcessiveQtySecondryItems");
            }
        }
        #endregion
        #endregion


        #region High School DepartMent

        #region Get High Scool Items 
        [ScreenPermissionFilter(screenId = 63)]
        public ActionResult GetHighScoolItemsPhysics(int? page)
        {

            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int schoolId = 0;
                    schoolId = (int)Session["ScoolId"];
                    var ModelList = _itemRepository.GetAllItemViewModel(x => x.itm_year == year && x.itm_schId == schoolId && x.itm_department == Department.ثانوى && x.itm_type == Domain.Type.فيزياء);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.Id);
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

                return RedirectToAction("GetHighScoolItemsPhysics");
            }

        }


        public ActionResult DownloadHighScoolItemsPhysics()
        {

            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int schoolId = 0;
                    schoolId = (int)Session["ScoolId"];
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o => o.itm_year == year && o.itm_schId == schoolId && o.itm_department == Department.ثانوى && o.itm_type == Domain.Type.فيزياء).Select(o => new ItemViewModel
                    {
                        company = o.itm_School.sch_complex.comp_company.com_arName,
                        itm_code = o.itm_code,
                        itm_arName = o.itm_arName,
                        itm_enName = o.itm_enName,
                        itm_desc = o.itm_desc,

                        itm_department = o.itm_department.ToString(),

                        itm_level = o.itm_level.ToString(),

                        itm_type = o.itm_type.ToString(),



                        itm_Unit = o.itm_Unit.unt_arName,

                        itm_sugQty = o.itm_sugQty ?? 0.0,

                        itm_presentQty = o.itm_presentQty ?? 0.0,

                        itm_isExisting = o.itm_isExisting.ToString(),
                        itm_availableMethod = o.itm_availableMethod.ToString(),

                        itm_chapter = o.itm_chapter ?? 1,

                        itm_term = o.itm_term.ToString(),




                        itm_School = o.itm_School.sch_arName,

                    }).ToList();

                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "GetItems.rpt"));
                    DataSet ds = new DataSet();
                    ds = new LogoDataSet.LogoDataSet();
                    var comlogo = _companyRepository.GetAll().FirstOrDefault().com_image;
                    var complogo = _complexRepository.GetAll().FirstOrDefault().comp_image;

                    ds.Tables[0].Rows.Add(comlogo, complogo);
                    rd.Database.Tables[0].SetDataSource(model);
                    rd.Database.Tables[1].SetDataSource(ds.Tables[0]);

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "اصناف الفيزياء المرحبةالثانوية.pdf");
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception)
            {

                return RedirectToAction("GetHighScoolItemsPhysics");
            }
        }
        #endregion
        #region Physics
        #region GetPresentHighScoolItemsPhysics
        [ScreenPermissionFilter(screenId = 64)]
        public ActionResult GetPresentHighScoolItemsPhysics(int? page)
        {


            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int schoolId = 0;
                    schoolId = (int)Session["ScoolId"];
                    var ModelList = _itemRepository.GetAllItemViewModel(x => x.itm_year == year && x.itm_schId == schoolId && x.itm_department == Department.ثانوى && x.itm_isExisting == ExsistState.موجود && x.itm_type == Domain.Type.فيزياء);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.Id);
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

                return RedirectToAction("GetPresentHighScoolItemsPhysics");
            }
        }


        public ActionResult DownloadPresentHighScoolItemsPhysics()
        {

            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int schoolId = 0;
                    schoolId = (int)Session["ScoolId"];
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o => o.itm_year == year && o.itm_schId ==schoolId && o.itm_department == Department.ثانوى && o.itm_isExisting == ExsistState.موجود && o.itm_type == Domain.Type.فيزياء).Select(o => new ItemViewModel
                    {
                        company = o.itm_School.sch_complex.comp_company.com_arName,
                        itm_code = o.itm_code,
                        itm_arName = o.itm_arName,
                        itm_enName = o.itm_enName,
                        itm_desc = o.itm_desc,

                        itm_department = o.itm_department.ToString(),

                        itm_level = o.itm_level.ToString(),

                        itm_type = o.itm_type.ToString(),



                        itm_Unit = o.itm_Unit.unt_arName,

                        itm_sugQty = o.itm_sugQty ?? 0.0,

                        itm_presentQty = o.itm_presentQty ?? 0.0,

                        itm_isExisting = o.itm_isExisting.ToString(),
                        itm_availableMethod = o.itm_availableMethod.ToString(),

                        itm_chapter = o.itm_chapter ?? 1,

                        itm_term = o.itm_term.ToString(),
                        itm_School = o.itm_School.sch_arName,
                        itm_ValidState = o.itm_ValidState.ToString(),
                        itm_completionYear = o.itm_completionYear ?? 0,
                        itm_excessiveQty = o.itm_excessiveQty ?? 0,
                        itm_note = o.itm_note,
                        sch_type = o.itm_School.sch_type.ToString()

                    }).ToList();

                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "GetPresentItems.rpt"));
                    DataSet ds = new DataSet();
                    ds = new LogoDataSet.LogoDataSet();
                    var comlogo = _companyRepository.GetAll().FirstOrDefault().com_image;
                    var complogo = _complexRepository.GetAll().FirstOrDefault().comp_image;

                    ds.Tables[0].Rows.Add(comlogo, complogo);
                    rd.Database.Tables[0].SetDataSource(model);
                    rd.Database.Tables[1].SetDataSource(ds.Tables[0]);

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "اصناف المرحبةالثانوية.pdf");
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception)
            {

                return RedirectToAction("GetPresentHighScoolItemsPhysics");
            }
        }
        #endregion

        #region GetPresentHighScoolItemsPhysics
        [ScreenPermissionFilter(screenId = 64)]
        public ActionResult GetUnValidHighScoolItemsPhysics(int? page)
        {


            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int schoolId = 0;
                    schoolId = (int)Session["ScoolId"];
                    var ModelList = _itemRepository.GetAllItemViewModel(x => x.itm_year == year && x.itm_schId == schoolId && x.itm_department == Department.ثانوى && x.itm_isExisting == ExsistState.موجود && x.itm_type == Domain.Type.فيزياء && x.itm_ValidState==ValidState.غير_صالح);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.Id);
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

                return RedirectToAction("GetUnValidHighScoolItemsPhysics");
            }
        }


        public ActionResult DownloadUnValidHighScoolItemsPhysics()
        {

            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int schoolId = 0;
                    schoolId = (int)Session["ScoolId"];
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o => o.itm_year == year && o.itm_schId == schoolId && o.itm_department == Department.ثانوى && o.itm_isExisting == ExsistState.موجود && o.itm_type == Domain.Type.فيزياء && o.itm_ValidState==ValidState.غير_صالح).Select(o => new ItemViewModel
                    {
                        company = o.itm_School.sch_complex.comp_company.com_arName,
                        itm_code = o.itm_code,
                        itm_arName = o.itm_arName,
                        itm_enName = o.itm_enName,
                        itm_desc = o.itm_desc,

                        itm_department = o.itm_department.ToString(),

                        itm_level = o.itm_level.ToString(),

                        itm_type = o.itm_type.ToString(),



                        itm_Unit = o.itm_Unit.unt_arName,

                        itm_sugQty = o.itm_sugQty ?? 0.0,

                        itm_presentQty = o.itm_presentQty ?? 0.0,

                        itm_isExisting = o.itm_isExisting.ToString(),
                        itm_availableMethod = o.itm_availableMethod.ToString(),

                        itm_chapter = o.itm_chapter ?? 1,

                        itm_term = o.itm_term.ToString(),
                        itm_School = o.itm_School.sch_arName,
                        itm_ValidState = o.itm_ValidState.ToString(),
                        itm_completionYear = o.itm_completionYear ?? 0,
                        itm_excessiveQty = o.itm_excessiveQty ?? 0,
                        itm_note = o.itm_note,
                        sch_type = o.itm_School.sch_type.ToString()

                    }).ToList();

                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "GetUnValidItems.rpt"));
                    DataSet ds = new DataSet();
                    ds = new LogoDataSet.LogoDataSet();
                    var comlogo = _companyRepository.GetAll().FirstOrDefault().com_image;
                    var complogo = _complexRepository.GetAll().FirstOrDefault().comp_image;

                    ds.Tables[0].Rows.Add(comlogo, complogo);
                    rd.Database.Tables[0].SetDataSource(model);
                    rd.Database.Tables[1].SetDataSource(ds.Tables[0]);

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "اصناف المرحبةالثانوية.pdf");
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception)
            {

                return RedirectToAction("GetUnValidHighScoolItemsPhysics");
            }
        }
        #endregion

        #region GetUnAvalableHighScoolItemsPhysics
        [ScreenPermissionFilter(screenId = 65)]
        public ActionResult GetUnAvalableHighScoolItemsPhysics(int? page)
        {


            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int schoolId = 0;
                    schoolId = (int)Session["ScoolId"];
                    var ModelList = _itemRepository.GetAllItemViewModel(x => x.itm_year == year && x.itm_schId ==schoolId && x.itm_department == Department.ثانوى && x.itm_isExisting == ExsistState.غير_موجود && x.itm_type == Domain.Type.فيزياء);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.Id);
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

                return RedirectToAction("GetUnAvalableHighScoolItemsPhysics");
            }
        }


        public ActionResult DownloadUnAvalableHighScoolItemsPhysics()
        {


            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int schoolId = 0;
                    schoolId = (int)Session["ScoolId"];
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o => o.itm_year == year && o.itm_schId == schoolId && o.itm_department == Department.ثانوى && o.itm_isExisting == ExsistState.غير_موجود && o.itm_type == Domain.Type.فيزياء).Select(o => new ItemViewModel
                    {
                        company = o.itm_School.sch_complex.comp_company.com_arName,
                        itm_code = o.itm_code,
                        itm_arName = o.itm_arName,
                        itm_enName = o.itm_enName,
                        itm_desc = o.itm_desc,

                        itm_department = o.itm_department.ToString(),

                        itm_level = o.itm_level.ToString(),

                        itm_type = o.itm_type.ToString(),



                        itm_Unit = o.itm_Unit.unt_arName,

                        itm_sugQty = o.itm_sugQty ?? 0.0,

                        itm_presentQty = o.itm_presentQty ?? 0.0,

                        itm_isExisting = o.itm_isExisting.ToString(),
                        itm_availableMethod = o.itm_availableMethod.ToString(),

                        itm_chapter = o.itm_chapter ?? 1,

                        itm_term = o.itm_term.ToString(),
                        itm_School = o.itm_School.sch_arName,
                        itm_ValidState = o.itm_ValidState.ToString(),
                        itm_completionYear = o.itm_completionYear ?? 0,
                        itm_excessiveQty = o.itm_excessiveQty ?? 0,
                        itm_note = o.itm_note,
                        sch_type = o.itm_School.sch_type.ToString()

                    }).ToList();

                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "GetUnAvalableItems.rpt"));
                    DataSet ds = new DataSet();
                    ds = new LogoDataSet.LogoDataSet();
                    var comlogo = _companyRepository.GetAll().FirstOrDefault().com_image;
                    var complogo = _complexRepository.GetAll().FirstOrDefault().comp_image;

                    ds.Tables[0].Rows.Add(comlogo, complogo);
                    rd.Database.Tables[0].SetDataSource(model);
                    rd.Database.Tables[1].SetDataSource(ds.Tables[0]);

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "اصناف المرحبةالثانوية.pdf");
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception)
            {

                return RedirectToAction("GetUnAvalableHighScoolItemsPhysics");
            }
        }
        #endregion

        #region GetOverHighScoolItemsPhysics 
        [ScreenPermissionFilter(screenId = 68)]
        public ActionResult GetOverHighScoolItemsPhysics(int? page)
        {


            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int schoolId = 0;
                    schoolId = (int)Session["ScoolId"];
                    var ModelList = _itemRepository.GetAllItemViewModel(x => x.itm_year == year && x.itm_schId == schoolId && x.itm_department == Department.ثانوى && x.itm_over == Over.زائد && x.itm_type == Domain.Type.فيزياء);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.Id);
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

                return RedirectToAction("GetOverHighScoolItemsPhysics");
            }
        }


        public ActionResult DownloadOverHighScoolItemsPhysics()
        {

            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int schoolId = 0;
                    schoolId = (int)Session["ScoolId"];
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o => o.itm_year == year && o.itm_schId ==schoolId&& o.itm_department == Department.ثانوى && o.itm_over == Over.زائد && o.itm_type == Domain.Type.فيزياء).Select(o => new ItemViewModel
                    {
                        company = o.itm_School.sch_complex.comp_company.com_arName,
                        itm_code = o.itm_code,
                        itm_arName = o.itm_arName,
                        itm_enName = o.itm_enName,
                        itm_desc = o.itm_desc,

                        itm_department = o.itm_department.ToString(),

                        itm_level = o.itm_level.ToString(),

                        itm_type = o.itm_type.ToString(),



                        itm_Unit = o.itm_Unit.unt_arName,

                        itm_sugQty = o.itm_sugQty ?? 0.0,

                        itm_presentQty = o.itm_presentQty ?? 0.0,

                        itm_isExisting = o.itm_isExisting.ToString(),
                        itm_availableMethod = o.itm_availableMethod.ToString(),

                        itm_chapter = o.itm_chapter ?? 1,

                        itm_term = o.itm_term.ToString(),
                        itm_School = o.itm_School.sch_arName,
                        itm_ValidState = o.itm_ValidState.ToString(),
                        itm_completionYear = o.itm_completionYear ?? 0,
                        itm_excessiveQty = o.itm_excessiveQty ?? 0,
                        itm_note = o.itm_note,
                        sch_type = o.itm_School.sch_type.ToString()

                    }).ToList();

                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "GetOverItems.rpt"));
                    DataSet ds = new DataSet();
                    ds = new LogoDataSet.LogoDataSet();
                    var comlogo = _companyRepository.GetAll().FirstOrDefault().com_image;
                    var complogo = _complexRepository.GetAll().FirstOrDefault().comp_image;

                    ds.Tables[0].Rows.Add(comlogo, complogo);
                    rd.Database.Tables[0].SetDataSource(model);
                    rd.Database.Tables[1].SetDataSource(ds.Tables[0]);

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "اصناف المرحبةالثانوية.pdf");
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception)
            {

                return RedirectToAction("GetOverHighScoolItemsPhysics");
            }
        }
        #endregion

        #region GetWantedHighScoolItemsPhysics
        [ScreenPermissionFilter(screenId = 66)]
        public ActionResult GetWantedHighScoolItemsPhysics(int? page)
        {


            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int schoolId = 0;
                    schoolId = (int)Session["ScoolId"];
                    var ModelList = _itemRepository.GetAllItemViewModel(x => x.itm_year == year && x.itm_availableMethod == AvailableMethod.المستودع && x.itm_schId == schoolId && x.itm_type != Domain.Type.اثاث_مختبر && x.itm_department == Department.ثانوى && x.itm_type == Domain.Type.فيزياء && x.itm_isExisting == ExsistState.غير_موجود);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.Id);
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

                return RedirectToAction("GetWantedHighScoolItemsPhysics");
            }
        }


        public ActionResult DownloadWantedHighScoolItemsPhysics()
        {

            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int schoolId = 0;
                    schoolId = (int)Session["ScoolId"];
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o => o.itm_year == year && o.itm_availableMethod == AvailableMethod.المستودع && o.itm_schId == schoolId && o.itm_type != Domain.Type.اثاث_مختبر && o.itm_department == Department.ثانوى && o.itm_type == Domain.Type.فيزياء && o.itm_isExisting == ExsistState.غير_موجود).Select(o => new ItemViewModel
                    {
                        company=o.itm_School.sch_complex.comp_company.com_arName,
                        itm_code = o.itm_code,
                        itm_arName = o.itm_arName,
                        itm_enName = o.itm_enName,
                        itm_desc = o.itm_desc,

                        itm_department = o.itm_department.ToString(),

                        itm_level = o.itm_level.ToString(),

                        itm_type = o.itm_type.ToString(),



                        itm_Unit = o.itm_Unit.unt_arName,

                        itm_sugQty = o.itm_sugQty ?? 0.0,

                        itm_presentQty = o.itm_presentQty ?? 0.0,

                        itm_isExisting = o.itm_isExisting.ToString(),
                        itm_availableMethod = o.itm_availableMethod.ToString(),

                        itm_chapter = o.itm_chapter ?? 1,

                        itm_term = o.itm_term.ToString(),
                        itm_School = o.itm_School.sch_arName,
                        itm_ValidState = o.itm_ValidState.ToString(),
                        itm_completionYear = o.itm_completionYear ?? 0,
                        itm_excessiveQty = o.itm_excessiveQty ?? 0,
                        itm_note = o.itm_note,
                        sch_type = o.itm_School.sch_type.ToString()
                      
                    }).ToList();
                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "GetWantedItems.rpt"));
                    DataSet ds = new DataSet();
                    ds = new LogoDataSet.LogoDataSet();
                    var comlogo = _companyRepository.GetAll().FirstOrDefault().com_image;
                    var complogo = _complexRepository.GetAll().FirstOrDefault().comp_image;

                    ds.Tables[0].Rows.Add(comlogo, complogo);
                    rd.Database.Tables[0].SetDataSource(model);
                    rd.Database.Tables[1].SetDataSource(ds.Tables[0]);
                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "اصناف المرحبةالثانوية.pdf");
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception ex)
            {

                return RedirectToAction("GetWantedHighScoolItemsPhysics");
            }
        }
        #endregion
        #region Get CanAvalable HighScoolItemsPhysics
        [ScreenPermissionFilter(screenId = 67)]
        public ActionResult GetCanAvalableHighScoolItemsPhysics(int? page)
        {


            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int schoolId = 0;
                    schoolId = (int)Session["ScoolId"];
                    var ModelList = _itemRepository.GetAllItemViewModel(x => x.itm_year == year && x.itm_availableMethod == AvailableMethod.المدرسة && x.itm_schId == schoolId && x.itm_type != Domain.Type.اثاث_مختبر && x.itm_department == Department.ثانوى && x.itm_type == Domain.Type.فيزياء && x.itm_isExisting == ExsistState.غير_موجود);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.Id);
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

                return RedirectToAction("GetCanAvalableHighScoolItemsPhysics");
            }
        }


        public ActionResult DownloadCanAvalableHighScoolItemsPhysics()
        {

            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int schoolId = 0;
                    schoolId = (int)Session["ScoolId"];
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o => o.itm_year == year && o.itm_availableMethod == AvailableMethod.المدرسة && o.itm_schId ==schoolId&& o.itm_type != Domain.Type.اثاث_مختبر && o.itm_department == Department.ثانوى && o.itm_type == Domain.Type.فيزياء && o.itm_isExisting == ExsistState.غير_موجود).Select(o => new ItemViewModel
                    {
                        company = o.itm_School.sch_complex.comp_company.com_arName,
                        itm_code = o.itm_code,
                        itm_arName = o.itm_arName,
                        itm_enName = o.itm_enName,
                        itm_desc = o.itm_desc,

                        itm_department = o.itm_department.ToString(),

                        itm_level = o.itm_level.ToString(),

                        itm_type = o.itm_type.ToString(),



                        itm_Unit = o.itm_Unit.unt_arName,

                        itm_sugQty = o.itm_sugQty ?? 0.0,

                        itm_presentQty = o.itm_presentQty ?? 0.0,

                        itm_isExisting = o.itm_isExisting.ToString(),
                        itm_availableMethod = o.itm_availableMethod.ToString(),

                        itm_chapter = o.itm_chapter ?? 1,

                        itm_term = o.itm_term.ToString(),
                        itm_School = o.itm_School.sch_arName,
                        itm_ValidState = o.itm_ValidState.ToString(),
                        itm_completionYear = o.itm_completionYear ?? 0,
                        itm_excessiveQty = o.itm_excessiveQty ?? 0,
                        itm_note = o.itm_note,
                        sch_type = o.itm_School.sch_type.ToString()
                  
                    }).ToList();

                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "GetCanAvalableItems.rpt"));
                    DataSet ds = new DataSet();
                    ds = new LogoDataSet.LogoDataSet();
                    var comlogo = _companyRepository.GetAll().FirstOrDefault().com_image;
                    var complogo = _complexRepository.GetAll().FirstOrDefault().comp_image;

                    ds.Tables[0].Rows.Add(comlogo, complogo);
                    rd.Database.Tables[0].SetDataSource(model);
                    rd.Database.Tables[1].SetDataSource(ds.Tables[0]);

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "اصناف المرحبةالثانوية.pdf");
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception ex)
            {

                return RedirectToAction("GetCanAvalableHighScoolItemsPhysics");
            }
        }
        #endregion

        #region GetexcessiveQtyHighScoolItemsPhysics
        [ScreenPermissionFilter(screenId = 69)]
        public ActionResult GetexcessiveQtyHighScoolItemsPhysics(int? page)
        {


            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int schoolId = 0;
                    schoolId = (int)Session["ScoolId"];
                    var ModelList = _itemRepository.GetAllItemViewModel(x => x.itm_year == year && x.itm_schId == schoolId && x.itm_department == Department.ثانوى && x.itm_isExisting == ExsistState.موجود && x.itm_type == Domain.Type.فيزياء && x.itm_excessiveQty > 0);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.Id);
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

                return RedirectToAction("GetexcessiveQtyHighScoolItemsPhysics");
            }
        }


        public ActionResult DownloadexcessiveQtyHighScoolItemsPhysics()
        {

            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int schoolId = 0;
                    schoolId = (int)Session["ScoolId"];
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o => o.itm_year == year && o.itm_schId == schoolId && o.itm_department == Department.ثانوى && o.itm_isExisting == ExsistState.موجود && o.itm_type == Domain.Type.فيزياء && o.itm_excessiveQty > 0).Select(o => new ItemViewModel
                    {
                        company = o.itm_School.sch_complex.comp_company.com_arName,
                        itm_code = o.itm_code,
                        itm_arName = o.itm_arName,
                        itm_enName = o.itm_enName,
                        itm_desc = o.itm_desc,

                        itm_department = o.itm_department.ToString(),

                        itm_level = o.itm_level.ToString(),

                        itm_type = o.itm_type.ToString(),



                        itm_Unit = o.itm_Unit.unt_arName,

                        itm_sugQty = o.itm_sugQty ?? 0.0,

                        itm_presentQty = o.itm_presentQty ?? 0.0,

                        itm_isExisting = o.itm_isExisting.ToString(),
                        itm_availableMethod = o.itm_availableMethod.ToString(),

                        itm_chapter = o.itm_chapter ?? 1,

                        itm_term = o.itm_term.ToString(),
                        itm_School = o.itm_School.sch_arName,
                        itm_ValidState = o.itm_ValidState.ToString(),
                        itm_completionYear = o.itm_completionYear ?? 0,
                        itm_excessiveQty = o.itm_excessiveQty ?? 0,
                        itm_note = o.itm_note,
                        sch_type = o.itm_School.sch_type.ToString()

                    }).ToList();

                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "GetExcessiveQty.rpt"));
                    DataSet ds = new DataSet();
                    ds = new LogoDataSet.LogoDataSet();
                    var comlogo = _companyRepository.GetAll().FirstOrDefault().com_image;
                    var complogo = _complexRepository.GetAll().FirstOrDefault().comp_image;

                    ds.Tables[0].Rows.Add(comlogo, complogo);
                    rd.Database.Tables[0].SetDataSource(model);
                    rd.Database.Tables[1].SetDataSource(ds.Tables[0]);

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "اصناف المرحبةالثانوية.pdf");
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("GetexcessiveQtyHighScoolItemsPhysics");
            }
        }
        #endregion
        #endregion

        #region Chemistry
        #region Get High Scool Items 
        [ScreenPermissionFilter(screenId = 63)]
        public ActionResult GetHighScoolItemsChemistry(int? page)
        {

            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int schoolId = 0;
                    schoolId = (int)Session["ScoolId"];
                    var ModelList = _itemRepository.GetAllItemViewModel(x => x.itm_year == year && x.itm_schId == schoolId && x.itm_department == Department.ثانوى && x.itm_type == Domain.Type.كيمياء);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.Id);
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

                return RedirectToAction("GetHighScoolItemsChemistry");
            }

        }


        public ActionResult DownloadHighScoolItemsChemistry()
        {

            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int schoolId = 0;
                    schoolId = (int)Session["ScoolId"];
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o => o.itm_year == year && o.itm_schId == schoolId && o.itm_department == Department.ثانوى && o.itm_type == Domain.Type.كيمياء).Select(o => new ItemViewModel
                    {
                        company = o.itm_School.sch_complex.comp_company.com_arName,
                        itm_code = o.itm_code,
                        itm_arName = o.itm_arName,
                        itm_enName = o.itm_enName,
                        itm_desc = o.itm_desc,

                        itm_department = o.itm_department.ToString(),

                        itm_level = o.itm_level.ToString(),

                        itm_type = o.itm_type.ToString(),



                        itm_Unit = o.itm_Unit.unt_arName,

                        itm_sugQty = o.itm_sugQty ?? 0.0,

                        itm_presentQty = o.itm_presentQty ?? 0.0,

                        itm_isExisting = o.itm_isExisting.ToString(),
                        itm_availableMethod = o.itm_availableMethod.ToString(),

                        itm_chapter = o.itm_chapter ?? 1,

                        itm_term = o.itm_term.ToString(),




                        itm_School = o.itm_School.sch_arName,

                    }).ToList();

                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "GetItems.rpt"));
                    DataSet ds = new DataSet();
                    ds = new LogoDataSet.LogoDataSet();
                    var comlogo = _companyRepository.GetAll().FirstOrDefault().com_image;
                    var complogo = _complexRepository.GetAll().FirstOrDefault().comp_image;

                    ds.Tables[0].Rows.Add(comlogo, complogo);
                    rd.Database.Tables[0].SetDataSource(model);
                    rd.Database.Tables[1].SetDataSource(ds.Tables[0]);

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "اصناف  الكيمياء المرحبةالثانوية.pdf");
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception)
            {

                return RedirectToAction("GetHighScoolItemsChemistry");
            }
        }
        #endregion
        #region GetPresentHighScoolItemsChemistry
        [ScreenPermissionFilter(screenId = 64)]
        public ActionResult GetPresentHighScoolItemsChemistry(int? page)
        {
            try
            {

                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int schoolId = 0;
                    schoolId = (int)Session["ScoolId"];
                    var ModelList = _itemRepository.GetAllItemViewModel(x => x.itm_year == year && x.itm_schId == schoolId && x.itm_department == Department.ثانوى && x.itm_isExisting == ExsistState.موجود && x.itm_type == Domain.Type.كيمياء);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.Id);
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

                return RedirectToAction("Index");
            }

        }


        public ActionResult DownloadPresentHighScoolItemsChemistry()
        {

            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int schoolId = 0;
                    schoolId = (int)Session["ScoolId"];
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o => o.itm_year == year && o.itm_schId ==schoolId&& o.itm_department == Department.ثانوى && o.itm_isExisting == ExsistState.موجود && o.itm_type == Domain.Type.كيمياء).Select(o => new ItemViewModel
                    {
                        company = o.itm_School.sch_complex.comp_company.com_arName,
                        itm_code = o.itm_code,
                        itm_arName = o.itm_arName,
                        itm_enName = o.itm_enName,
                        itm_desc = o.itm_desc,

                        itm_department = o.itm_department.ToString(),

                        itm_level = o.itm_level.ToString(),

                        itm_type = o.itm_type.ToString(),



                        itm_Unit = o.itm_Unit.unt_arName,

                        itm_sugQty = o.itm_sugQty ?? 0.0,

                        itm_presentQty = o.itm_presentQty ?? 0.0,

                        itm_isExisting = o.itm_isExisting.ToString(),
                        itm_availableMethod = o.itm_availableMethod.ToString(),

                        itm_chapter = o.itm_chapter ?? 1,

                        itm_term = o.itm_term.ToString(),
                        itm_School = o.itm_School.sch_arName,
                        itm_ValidState = o.itm_ValidState.ToString(),
                        itm_completionYear = o.itm_completionYear ?? 0,
                        itm_excessiveQty = o.itm_excessiveQty ?? 0,
                        itm_note = o.itm_note,
                        sch_type = o.itm_School.sch_type.ToString()

                    }).ToList();

                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "GetPresentItems.rpt"));
                    DataSet ds = new DataSet();
                    ds = new LogoDataSet.LogoDataSet();
                    var comlogo = _companyRepository.GetAll().FirstOrDefault().com_image;
                    var complogo = _complexRepository.GetAll().FirstOrDefault().comp_image;

                    ds.Tables[0].Rows.Add(comlogo, complogo);
                    rd.Database.Tables[0].SetDataSource(model);
                    rd.Database.Tables[1].SetDataSource(ds.Tables[0]);

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "اصناف المرحبةالثانوية.pdf");
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
        #endregion
        #region GetUnValidHighScoolItemsChemistry
        [ScreenPermissionFilter(screenId = 64)]
        public ActionResult GetUnValidHighScoolItemsChemistry(int? page)
        {
            try
            {

                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int schoolId = 0;
                    schoolId = (int)Session["ScoolId"];
                    var ModelList = _itemRepository.GetAllItemViewModel(x => x.itm_year == year && x.itm_schId == schoolId && x.itm_department == Department.ثانوى && x.itm_isExisting == ExsistState.موجود && x.itm_type == Domain.Type.كيمياء  && x.itm_ValidState==ValidState.غير_صالح);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.Id);
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

                return RedirectToAction("Index");
            }

        }


        public ActionResult DownloadUnValidHighScoolItemsChemistry()
        {

            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int schoolId = 0;
                    schoolId = (int)Session["ScoolId"];
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o => o.itm_year == year && o.itm_schId == schoolId && o.itm_department == Department.ثانوى && o.itm_isExisting == ExsistState.موجود && o.itm_type == Domain.Type.كيمياء &&o.itm_ValidState==ValidState.غير_صالح).Select(o => new ItemViewModel
                    {
                        company = o.itm_School.sch_complex.comp_company.com_arName,
                        itm_code = o.itm_code,
                        itm_arName = o.itm_arName,
                        itm_enName = o.itm_enName,
                        itm_desc = o.itm_desc,

                        itm_department = o.itm_department.ToString(),

                        itm_level = o.itm_level.ToString(),

                        itm_type = o.itm_type.ToString(),



                        itm_Unit = o.itm_Unit.unt_arName,

                        itm_sugQty = o.itm_sugQty ?? 0.0,

                        itm_presentQty = o.itm_presentQty ?? 0.0,

                        itm_isExisting = o.itm_isExisting.ToString(),
                        itm_availableMethod = o.itm_availableMethod.ToString(),

                        itm_chapter = o.itm_chapter ?? 1,

                        itm_term = o.itm_term.ToString(),
                        itm_School = o.itm_School.sch_arName,
                        itm_ValidState = o.itm_ValidState.ToString(),
                        itm_completionYear = o.itm_completionYear ?? 0,
                        itm_excessiveQty = o.itm_excessiveQty ?? 0,
                        itm_note = o.itm_note,
                        sch_type = o.itm_School.sch_type.ToString()

                    }).ToList();

                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "GetUnValidItems.rpt"));
                    DataSet ds = new DataSet();
                    ds = new LogoDataSet.LogoDataSet();
                    var comlogo = _companyRepository.GetAll().FirstOrDefault().com_image;
                    var complogo = _complexRepository.GetAll().FirstOrDefault().comp_image;

                    ds.Tables[0].Rows.Add(comlogo, complogo);
                    rd.Database.Tables[0].SetDataSource(model);
                    rd.Database.Tables[1].SetDataSource(ds.Tables[0]);

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "اصناف المرحبةالثانوية.pdf");
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
        #endregion

        #region GetUnAvalableHighScoolItemsChemistry
        [ScreenPermissionFilter(screenId = 65)]
        public ActionResult GetUnAvalableHighScoolItemsChemistry(int? page)
        {


            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int schoolId = 0;
                    schoolId = (int)Session["ScoolId"];
                    var ModelList = _itemRepository.GetAllItemViewModel(x => x.itm_year == year && x.itm_schId == schoolId && x.itm_department == Department.ثانوى && x.itm_isExisting == ExsistState.غير_موجود && x.itm_type == Domain.Type.كيمياء);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.Id);
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

                return RedirectToAction("GetUnAvalableHighScoolItemsChemistry");
            }
        }


        public ActionResult DownloadUnAvalableHighScoolItemsChemistry()
        {


            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int schoolId = 0;
                    schoolId = (int)Session["ScoolId"];
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o => o.itm_year == year && o.itm_schId == schoolId && o.itm_department == Department.ثانوى && o.itm_isExisting == ExsistState.غير_موجود && o.itm_type == Domain.Type.كيمياء).Select(o => new ItemViewModel
                    {
                        company = o.itm_School.sch_complex.comp_company.com_arName,
                        itm_code = o.itm_code,
                        itm_arName = o.itm_arName,
                        itm_enName = o.itm_enName,
                        itm_desc = o.itm_desc,

                        itm_department = o.itm_department.ToString(),

                        itm_level = o.itm_level.ToString(),

                        itm_type = o.itm_type.ToString(),



                        itm_Unit = o.itm_Unit.unt_arName,

                        itm_sugQty = o.itm_sugQty ?? 0.0,

                        itm_presentQty = o.itm_presentQty ?? 0.0,

                        itm_isExisting = o.itm_isExisting.ToString(),
                        itm_availableMethod = o.itm_availableMethod.ToString(),

                        itm_chapter = o.itm_chapter ?? 1,

                        itm_term = o.itm_term.ToString(),
                        itm_School = o.itm_School.sch_arName,
                        itm_ValidState = o.itm_ValidState.ToString(),
                        itm_completionYear = o.itm_completionYear ?? 0,
                        itm_excessiveQty = o.itm_excessiveQty ?? 0,
                        itm_note = o.itm_note,
                        sch_type = o.itm_School.sch_type.ToString()

                    }).ToList();

                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "GetUnAvalableItems.rpt"));
                    DataSet ds = new DataSet();
                    ds = new LogoDataSet.LogoDataSet();
                    var comlogo = _companyRepository.GetAll().FirstOrDefault().com_image;
                    var complogo = _complexRepository.GetAll().FirstOrDefault().comp_image;

                    ds.Tables[0].Rows.Add(comlogo, complogo);
                    rd.Database.Tables[0].SetDataSource(model);
                    rd.Database.Tables[1].SetDataSource(ds.Tables[0]);

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "اصناف المرحبةالثانوية.pdf");
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception)
            {

                return RedirectToAction("GetUnAvalableHighScoolItemsChemistry");
            }
        }
        #endregion

        #region GetOverHighScoolItemsChemistry
        [ScreenPermissionFilter(screenId = 68)]
        public ActionResult GetOverHighScoolItemsChemistry(int? page)
        {


            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int schoolId = 0;
                    schoolId = (int)Session["ScoolId"];
                    var ModelList = _itemRepository.GetAllItemViewModel(x => x.itm_year == year && x.itm_schId == schoolId && x.itm_department == Department.ثانوى && x.itm_over == Over.زائد && x.itm_type == Domain.Type.كيمياء);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.Id);
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

                return RedirectToAction("GetOverHighScoolItemsChemistry");
            }
        }


        public ActionResult DownloadOverHighScoolItemsChemistry()
        {

            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int schoolId = 0;
                    schoolId = (int)Session["ScoolId"];
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o => o.itm_year == year && o.itm_schId ==schoolId && o.itm_department == Department.ثانوى && o.itm_over == Over.زائد && o.itm_type == Domain.Type.كيمياء).Select(o => new ItemViewModel
                    {
                        company = o.itm_School.sch_complex.comp_company.com_arName,
                        itm_code = o.itm_code,
                        itm_arName = o.itm_arName,
                        itm_enName = o.itm_enName,
                        itm_desc = o.itm_desc,

                        itm_department = o.itm_department.ToString(),

                        itm_level = o.itm_level.ToString(),

                        itm_type = o.itm_type.ToString(),



                        itm_Unit = o.itm_Unit.unt_arName,

                        itm_sugQty = o.itm_sugQty ?? 0.0,

                        itm_presentQty = o.itm_presentQty ?? 0.0,

                        itm_isExisting = o.itm_isExisting.ToString(),
                        itm_availableMethod = o.itm_availableMethod.ToString(),

                        itm_chapter = o.itm_chapter ?? 1,

                        itm_term = o.itm_term.ToString(),
                        itm_School = o.itm_School.sch_arName,
                        itm_ValidState = o.itm_ValidState.ToString(),
                        itm_completionYear = o.itm_completionYear ?? 0,
                        itm_excessiveQty = o.itm_excessiveQty ?? 0,
                        itm_note = o.itm_note,
                        sch_type = o.itm_School.sch_type.ToString()

                    }).ToList();

                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "GetOverItems.rpt"));
                    DataSet ds = new DataSet();
                    ds = new LogoDataSet.LogoDataSet();
                    var comlogo = _companyRepository.GetAll().FirstOrDefault().com_image;
                    var complogo = _complexRepository.GetAll().FirstOrDefault().comp_image;

                    ds.Tables[0].Rows.Add(comlogo, complogo);
                    rd.Database.Tables[0].SetDataSource(model);
                    rd.Database.Tables[1].SetDataSource(ds.Tables[0]);

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "اصناف المرحبةالثانوية.pdf");
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception)
            {

                return RedirectToAction("GetOverHighScoolItemsChemistry");
            }
        }
        #endregion

        #region GetWantedHighScoolItemsChemistry
        [ScreenPermissionFilter(screenId = 66)]
        public ActionResult GetWantedHighScoolItemsChemistry(int? page)
        {


            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int schoolId = 0;
                    schoolId = (int)Session["ScoolId"];
                    var ModelList = _itemRepository.GetAllItemViewModel(x => x.itm_year == year && x.itm_availableMethod == AvailableMethod.المستودع && x.itm_schId == schoolId && x.itm_type != Domain.Type.اثاث_مختبر && x.itm_department == Department.ثانوى && x.itm_type == Domain.Type.كيمياء && x.itm_isExisting == ExsistState.غير_موجود);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.Id);
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

                return RedirectToAction("GetWantedHighScoolItemsChemistry");
            }
        }


        public ActionResult DownloadWantedHighScoolItemsChemistry()
        {

            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o => o.itm_year == year && o.itm_availableMethod == AvailableMethod.المستودع && o.itm_schId == school_id && o.itm_type != Domain.Type.اثاث_مختبر && o.itm_department == Department.ثانوى && o.itm_type == Domain.Type.كيمياء && o.itm_isExisting == ExsistState.غير_موجود).Select(o => new ItemViewModel
                    {
                        company = o.itm_School.sch_complex.comp_company.com_arName,
                        itm_code = o.itm_code,
                        itm_arName = o.itm_arName,
                        itm_enName = o.itm_enName,
                        itm_desc = o.itm_desc,

                        itm_department = o.itm_department.ToString(),

                        itm_level = o.itm_level.ToString(),

                        itm_type = o.itm_type.ToString(),



                        itm_Unit = o.itm_Unit.unt_arName,

                        itm_sugQty = o.itm_sugQty ?? 0.0,

                        itm_presentQty = o.itm_presentQty ?? 0.0,

                        itm_isExisting = o.itm_isExisting.ToString(),
                        itm_availableMethod = o.itm_availableMethod.ToString(),

                        itm_chapter = o.itm_chapter ?? 1,

                        itm_term = o.itm_term.ToString(),
                        itm_School = o.itm_School.sch_arName,
                        itm_ValidState = o.itm_ValidState.ToString(),
                        itm_completionYear = o.itm_completionYear ?? 0,
                        itm_excessiveQty = o.itm_excessiveQty ?? 0,
                        itm_note = o.itm_note,
                        sch_type = o.itm_School.sch_type.ToString()

                    }).ToList();

                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "GetWantedItems.rpt"));
                    DataSet ds = new DataSet();
                    ds = new LogoDataSet.LogoDataSet();
                    var comlogo = _companyRepository.GetAll().FirstOrDefault().com_image;
                    var complogo = _complexRepository.GetAll().FirstOrDefault().comp_image;

                    ds.Tables[0].Rows.Add(comlogo, complogo);
                    rd.Database.Tables[0].SetDataSource(model);
                    rd.Database.Tables[1].SetDataSource(ds.Tables[0]);

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "اصناف المرحبةالثانوية.pdf");
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception)
            {

                return RedirectToAction("GetWantedHighScoolItemsChemistry");
            }
        }
        #endregion
        #region Get CanAvalable HighScoolItemsChemistry
        [ScreenPermissionFilter(screenId = 67)]
        public ActionResult GetCanAvalableHighScoolItemsChemistry(int? page)
        {

            try
            {

                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    var ModelList = _itemRepository.GetAllItemViewModel(x => x.itm_year == year && x.itm_availableMethod == AvailableMethod.المدرسة && x.itm_schId == school_id && x.itm_type != Domain.Type.اثاث_مختبر && x.itm_department == Department.ثانوى && x.itm_type == Domain.Type.كيمياء);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.Id);
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

                return RedirectToAction("GetCanAvalableHighScoolItemsChemistry");
            }
        }


        public ActionResult DownloadCanAvalableHighScoolItemsChemistry()
        {

            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o => o.itm_year == year && o.itm_availableMethod == AvailableMethod.المدرسة && o.itm_schId == school_id && o.itm_type != Domain.Type.اثاث_مختبر && o.itm_department == Department.ثانوى && o.itm_type == Domain.Type.كيمياء).Select(o => new ItemViewModel
                    {
                        company = o.itm_School.sch_complex.comp_company.com_arName,
                        itm_code = o.itm_code,
                        itm_arName = o.itm_arName,
                        itm_enName = o.itm_enName,
                        itm_desc = o.itm_desc,

                        itm_department = o.itm_department.ToString(),

                        itm_level = o.itm_level.ToString(),

                        itm_type = o.itm_type.ToString(),



                        itm_Unit = o.itm_Unit.unt_arName,

                        itm_sugQty = o.itm_sugQty ?? 0.0,

                        itm_presentQty = o.itm_presentQty ?? 0.0,

                        itm_isExisting = o.itm_isExisting.ToString(),
                        itm_availableMethod = o.itm_availableMethod.ToString(),

                        itm_chapter = o.itm_chapter ?? 1,

                        itm_term = o.itm_term.ToString(),
                        itm_School = o.itm_School.sch_arName,
                        itm_ValidState = o.itm_ValidState.ToString(),
                        itm_completionYear = o.itm_completionYear ?? 0,
                        itm_excessiveQty = o.itm_excessiveQty ?? 0,
                        itm_note = o.itm_note,
                        sch_type = o.itm_School.sch_type.ToString()

                    }).ToList();

                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "GetCanAvalableItems.rpt"));
                    DataSet ds = new DataSet();
                    ds = new LogoDataSet.LogoDataSet();
                    var comlogo = _companyRepository.GetAll().FirstOrDefault().com_image;
                    var complogo = _complexRepository.GetAll().FirstOrDefault().comp_image;

                    ds.Tables[0].Rows.Add(comlogo, complogo);
                    rd.Database.Tables[0].SetDataSource(model);
                    rd.Database.Tables[1].SetDataSource(ds.Tables[0]);

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "اصناف المرحبةالثانوية.pdf");
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception)
            {

                return RedirectToAction("GetCanAvalableHighScoolItemsChemistry");
            }
        }
        #endregion

        #region GetexcessiveQtyHighScoolItemsChemistry
        [ScreenPermissionFilter(screenId = 69)]
        public ActionResult GetexcessiveQtyHighScoolItemsChemistry(int? page)
        {
            try
            {

                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    var ModelList = _itemRepository.GetAllItemViewModel(x => x.itm_year == year && x.itm_schId == school_id && x.itm_department == Department.ثانوى && x.itm_isExisting == ExsistState.موجود && x.itm_type == Domain.Type.كيمياء && x.itm_excessiveQty > 0);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.Id);
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

                return RedirectToAction("GetexcessiveQtyHighScoolItemsChemistry");
            }

        }


        public ActionResult DownloadexcessiveQtyHighScoolItemsChemistry()
        {

            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o => o.itm_year == year && o.itm_schId == school_id&& o.itm_department == Department.ثانوى && o.itm_isExisting == ExsistState.موجود && o.itm_type == Domain.Type.كيمياء && o.itm_excessiveQty > 0).Select(o => new ItemViewModel
                    {
                        company = o.itm_School.sch_complex.comp_company.com_arName,
                        itm_code = o.itm_code,
                        itm_arName = o.itm_arName,
                        itm_enName = o.itm_enName,
                        itm_desc = o.itm_desc,

                        itm_department = o.itm_department.ToString(),

                        itm_level = o.itm_level.ToString(),

                        itm_type = o.itm_type.ToString(),



                        itm_Unit = o.itm_Unit.unt_arName,

                        itm_sugQty = o.itm_sugQty ?? 0.0,

                        itm_presentQty = o.itm_presentQty ?? 0.0,

                        itm_isExisting = o.itm_isExisting.ToString(),
                        itm_availableMethod = o.itm_availableMethod.ToString(),

                        itm_chapter = o.itm_chapter ?? 1,

                        itm_term = o.itm_term.ToString(),
                        itm_School = o.itm_School.sch_arName,
                        itm_ValidState = o.itm_ValidState.ToString(),
                        itm_completionYear = o.itm_completionYear ?? 0,
                        itm_excessiveQty = o.itm_excessiveQty ?? 0,
                        itm_note = o.itm_note,
                        sch_type = o.itm_School.sch_type.ToString()

                    }).ToList();

                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "GetExcessiveQty.rpt"));
                    DataSet ds = new DataSet();
                    ds = new LogoDataSet.LogoDataSet();
                    var comlogo = _companyRepository.GetAll().FirstOrDefault().com_image;
                    var complogo = _complexRepository.GetAll().FirstOrDefault().comp_image;

                    ds.Tables[0].Rows.Add(comlogo, complogo);
                    rd.Database.Tables[0].SetDataSource(model);
                    rd.Database.Tables[1].SetDataSource(ds.Tables[0]);

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "اصناف المرحبةالثانوية.pdf");
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception)
            {

                return RedirectToAction("GetexcessiveQtyHighScoolItemsChemistry");
            }
        }
        #endregion
        #endregion

        #region Biology
        #region Get High Scool Items 
        [ScreenPermissionFilter(screenId = 63)]
        public ActionResult GetHighScoolItemsBiology(int? page)
        {

            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    var ModelList = _itemRepository.GetAllItemViewModel(x => x.itm_year == year && x.itm_schId ==school_id && x.itm_department == Department.ثانوى && x.itm_type == Domain.Type.احياء);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.Id);
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

                return RedirectToAction("GetHighScoolItemsBiology");
            }

        }


        public ActionResult DownloadHighScoolItemsBiology()
        {

            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o => o.itm_year == year && o.itm_schId == school_id && o.itm_department == Department.ثانوى && o.itm_type == Domain.Type.احياء).Select(o => new ItemViewModel
                    {
                        company = o.itm_School.sch_complex.comp_company.com_arName,
                        itm_code = o.itm_code,
                        itm_arName = o.itm_arName,
                        itm_enName = o.itm_enName,
                        itm_desc = o.itm_desc,

                        itm_department = o.itm_department.ToString(),

                        itm_level = o.itm_level.ToString(),

                        itm_type = o.itm_type.ToString(),



                        itm_Unit = o.itm_Unit.unt_arName,

                        itm_sugQty = o.itm_sugQty ?? 0.0,

                        itm_presentQty = o.itm_presentQty ?? 0.0,

                        itm_isExisting = o.itm_isExisting.ToString(),
                        itm_availableMethod = o.itm_availableMethod.ToString(),

                        itm_chapter = o.itm_chapter ?? 1,

                        itm_term = o.itm_term.ToString(),




                        itm_School = o.itm_School.sch_arName,

                    }).ToList();

                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "GetItems.rpt"));
                    DataSet ds = new DataSet();
                    ds = new LogoDataSet.LogoDataSet();
                    var comlogo = _companyRepository.GetAll().FirstOrDefault().com_image;
                    var complogo = _complexRepository.GetAll().FirstOrDefault().comp_image;

                    ds.Tables[0].Rows.Add(comlogo, complogo);
                    rd.Database.Tables[0].SetDataSource(model);
                    rd.Database.Tables[1].SetDataSource(ds.Tables[0]);

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "اصناف  الاحياء المرحبةالثانوية.pdf");
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception)
            {

                return RedirectToAction("GetHighScoolItemsBiology");
            }
        }
        #endregion
        #region GetPresentHighScoolItemsBiology
        [ScreenPermissionFilter(screenId = 64)]
        public ActionResult GetPresentHighScoolItemsBiology(int? page)
        {

            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"]; int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    var ModelList = _itemRepository.GetAllItemViewModel(x => x.itm_year == year && x.itm_schId == school_id && x.itm_department == Department.ثانوى && x.itm_isExisting == ExsistState.موجود && x.itm_type == Domain.Type.احياء);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.Id);
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

                return RedirectToAction("GetPresentHighScoolItemsBiology");
            }

        }


        public ActionResult DownloadPresentHighScoolItemsBiology()
        {
            try
            {

                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o => o.itm_year == year && o.itm_schId == school_id && o.itm_department == Department.ثانوى && o.itm_isExisting == ExsistState.موجود && o.itm_type == Domain.Type.احياء).Select(o => new ItemViewModel
                    {
                        company = o.itm_School.sch_complex.comp_company.com_arName,
                        itm_code = o.itm_code,
                        itm_arName = o.itm_arName,
                        itm_enName = o.itm_enName,
                        itm_desc = o.itm_desc,

                        itm_department = o.itm_department.ToString(),

                        itm_level = o.itm_level.ToString(),

                        itm_type = o.itm_type.ToString(),



                        itm_Unit = o.itm_Unit.unt_arName,

                        itm_sugQty = o.itm_sugQty ?? 0.0,

                        itm_presentQty = o.itm_presentQty ?? 0.0,

                        itm_isExisting = o.itm_isExisting.ToString(),
                        itm_availableMethod = o.itm_availableMethod.ToString(),

                        itm_chapter = o.itm_chapter ?? 1,

                        itm_term = o.itm_term.ToString(),
                        itm_School = o.itm_School.sch_arName,
                        itm_ValidState = o.itm_ValidState.ToString(),
                        itm_completionYear = o.itm_completionYear ?? 0,
                        itm_excessiveQty = o.itm_excessiveQty ?? 0,
                        itm_note = o.itm_note,
                        sch_type = o.itm_School.sch_type.ToString()

                    }).ToList();

                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "GetPresentItems.rpt"));
                    DataSet ds = new DataSet();
                    ds = new LogoDataSet.LogoDataSet();
                    var comlogo = _companyRepository.GetAll().FirstOrDefault().com_image;
                    var complogo = _complexRepository.GetAll().FirstOrDefault().comp_image;

                    ds.Tables[0].Rows.Add(comlogo, complogo);
                    rd.Database.Tables[0].SetDataSource(model);
                    rd.Database.Tables[1].SetDataSource(ds.Tables[0]);

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "اصناف المرحبةالثانوية.pdf");
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception)
            {

                return RedirectToAction("GetPresentHighScoolItemsBiology");
            }
        }
        #endregion

        #region GetPresentHighScoolItemsBiology
        [ScreenPermissionFilter(screenId = 64)]
        public ActionResult GetUnValidHighScoolItemsBiology(int? page)
        {

            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"]; int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    var ModelList = _itemRepository.GetAllItemViewModel(x => x.itm_year == year && x.itm_schId == school_id && x.itm_department == Department.ثانوى && x.itm_isExisting == ExsistState.موجود && x.itm_type == Domain.Type.احياء &&x.itm_ValidState==ValidState.غير_صالح);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.Id);
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

                return RedirectToAction("GetUnValidHighScoolItemsBiology");
            }

        }


        public ActionResult DownloadUnValidHighScoolItemsBiology()
        {
            try
            {

                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o => o.itm_year == year && o.itm_schId == school_id && o.itm_department == Department.ثانوى && o.itm_isExisting == ExsistState.موجود && o.itm_type == Domain.Type.احياء && o.itm_ValidState==ValidState.غير_صالح).Select(o => new ItemViewModel
                    {
                        company = o.itm_School.sch_complex.comp_company.com_arName,
                        itm_code = o.itm_code,
                        itm_arName = o.itm_arName,
                        itm_enName = o.itm_enName,
                        itm_desc = o.itm_desc,

                        itm_department = o.itm_department.ToString(),

                        itm_level = o.itm_level.ToString(),

                        itm_type = o.itm_type.ToString(),



                        itm_Unit = o.itm_Unit.unt_arName,

                        itm_sugQty = o.itm_sugQty ?? 0.0,

                        itm_presentQty = o.itm_presentQty ?? 0.0,

                        itm_isExisting = o.itm_isExisting.ToString(),
                        itm_availableMethod = o.itm_availableMethod.ToString(),

                        itm_chapter = o.itm_chapter ?? 1,

                        itm_term = o.itm_term.ToString(),
                        itm_School = o.itm_School.sch_arName,
                        itm_ValidState = o.itm_ValidState.ToString(),
                        itm_completionYear = o.itm_completionYear ?? 0,
                        itm_excessiveQty = o.itm_excessiveQty ?? 0,
                        itm_note = o.itm_note,
                        sch_type = o.itm_School.sch_type.ToString()

                    }).ToList();

                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "GetUnValidItems.rpt"));
                    DataSet ds = new DataSet();
                    ds = new LogoDataSet.LogoDataSet();
                    var comlogo = _companyRepository.GetAll().FirstOrDefault().com_image;
                    var complogo = _complexRepository.GetAll().FirstOrDefault().comp_image;

                    ds.Tables[0].Rows.Add(comlogo, complogo);
                    rd.Database.Tables[0].SetDataSource(model);
                    rd.Database.Tables[1].SetDataSource(ds.Tables[0]);

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "اصناف المرحبةالثانوية.pdf");
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception)
            {

                return RedirectToAction("GetUnValidHighScoolItemsBiology");
            }
        }
        #endregion

        #region GetUnAvalableHighScoolItemsBiology
        [ScreenPermissionFilter(screenId = 65)]
        public ActionResult GetUnAvalableHighScoolItemsBiology(int? page)
        {

            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    var ModelList = _itemRepository.GetAllItemViewModel(x => x.itm_year == year && x.itm_schId == school_id && x.itm_department == Department.ثانوى && x.itm_isExisting == ExsistState.غير_موجود && x.itm_type == Domain.Type.احياء);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.Id);
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

                return RedirectToAction("GetUnAvalableHighScoolItemsBiology");
            }
        }


        public ActionResult DownloadUnAvalableHighScoolItemsBiology()
        {


            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o => o.itm_year == year && o.itm_schId == school_id && o.itm_department == Department.ثانوى && o.itm_isExisting == ExsistState.غير_موجود && o.itm_type == Domain.Type.احياء).Select(o => new ItemViewModel
                    {
                        company = o.itm_School.sch_complex.comp_company.com_arName,
                        itm_code = o.itm_code,
                        itm_arName = o.itm_arName,
                        itm_enName = o.itm_enName,
                        itm_desc = o.itm_desc,

                        itm_department = o.itm_department.ToString(),

                        itm_level = o.itm_level.ToString(),

                        itm_type = o.itm_type.ToString(),



                        itm_Unit = o.itm_Unit.unt_arName,

                        itm_sugQty = o.itm_sugQty ?? 0.0,

                        itm_presentQty = o.itm_presentQty ?? 0.0,

                        itm_isExisting = o.itm_isExisting.ToString(),
                        itm_availableMethod = o.itm_availableMethod.ToString(),

                        itm_chapter = o.itm_chapter ?? 1,

                        itm_term = o.itm_term.ToString(),
                        itm_School = o.itm_School.sch_arName,
                        itm_ValidState = o.itm_ValidState.ToString(),
                        itm_completionYear = o.itm_completionYear ?? 0,
                        itm_excessiveQty = o.itm_excessiveQty ?? 0,
                        itm_note = o.itm_note,
                        sch_type = o.itm_School.sch_type.ToString()

                    }).ToList();

                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "GetUnAvalableItems.rpt"));
                    DataSet ds = new DataSet();
                    ds = new LogoDataSet.LogoDataSet();
                    var comlogo = _companyRepository.GetAll().FirstOrDefault().com_image;
                    var complogo = _complexRepository.GetAll().FirstOrDefault().comp_image;

                    ds.Tables[0].Rows.Add(comlogo, complogo);
                    rd.Database.Tables[0].SetDataSource(model);
                    rd.Database.Tables[1].SetDataSource(ds.Tables[0]);

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "اصناف المرحبةالثانوية.pdf");
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception)
            {

                return RedirectToAction("GetUnAvalableHighScoolItemsBiology");
            }
        }
        #endregion

        #region GetOverHighScoolItemsBiology
        [ScreenPermissionFilter(screenId = 68)]
        public ActionResult GetOverHighScoolItemsBiology(int? page)
        {

            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    var ModelList = _itemRepository.GetAllItemViewModel(x => x.itm_year == year && x.itm_schId ==school_id && x.itm_department == Department.ثانوى && x.itm_over == Over.زائد && x.itm_type == Domain.Type.احياء);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.Id);
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

                return RedirectToAction("GetOverHighScoolItemsBiology");
            }

        }


        public ActionResult DownloadOverHighScoolItemsBiology()
        {
            try
            {

                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o => o.itm_year == year && o.itm_schId == school_id && o.itm_department == Department.ثانوى && o.itm_over == Over.زائد && o.itm_type == Domain.Type.احياء).Select(o => new ItemViewModel
                    {
                        company = o.itm_School.sch_complex.comp_company.com_arName,
                        itm_code = o.itm_code,
                        itm_arName = o.itm_arName,
                        itm_enName = o.itm_enName,
                        itm_desc = o.itm_desc,

                        itm_department = o.itm_department.ToString(),

                        itm_level = o.itm_level.ToString(),

                        itm_type = o.itm_type.ToString(),



                        itm_Unit = o.itm_Unit.unt_arName,

                        itm_sugQty = o.itm_sugQty ?? 0.0,

                        itm_presentQty = o.itm_presentQty ?? 0.0,

                        itm_isExisting = o.itm_isExisting.ToString(),
                        itm_availableMethod = o.itm_availableMethod.ToString(),

                        itm_chapter = o.itm_chapter ?? 1,

                        itm_term = o.itm_term.ToString(),
                        itm_School = o.itm_School.sch_arName,
                        itm_ValidState = o.itm_ValidState.ToString(),
                        itm_completionYear = o.itm_completionYear ?? 0,
                        itm_excessiveQty = o.itm_excessiveQty ?? 0,
                        itm_note = o.itm_note,
                        sch_type = o.itm_School.sch_type.ToString()

                    }).ToList();

                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "GetOverItems.rpt"));
                    DataSet ds = new DataSet();
                    ds = new LogoDataSet.LogoDataSet();
                    var comlogo = _companyRepository.GetAll().FirstOrDefault().com_image;
                    var complogo = _complexRepository.GetAll().FirstOrDefault().comp_image;

                    ds.Tables[0].Rows.Add(comlogo, complogo);
                    rd.Database.Tables[0].SetDataSource(model);
                    rd.Database.Tables[1].SetDataSource(ds.Tables[0]);

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "اصناف المرحبةالثانوية.pdf");
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception)
            {

                return RedirectToAction("GetOverHighScoolItemsBiology");
            }
        }
        #endregion

        #region GetWantedHighScoolItemsBiology
        [ScreenPermissionFilter(screenId = 66)]
        public ActionResult GetWantedHighScoolItemsBiology(int? page)
        {


            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    var ModelList = _itemRepository.GetAllItemViewModel(x => x.itm_year == year && x.itm_availableMethod == AvailableMethod.المستودع && x.itm_schId == school_id && x.itm_type != Domain.Type.اثاث_مختبر && x.itm_department == Department.ثانوى && x.itm_type == Domain.Type.احياء && x.itm_isExisting == ExsistState.غير_موجود);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.Id);
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

                return RedirectToAction("GetWantedHighScoolItemsBiology");
            }
        }


        public ActionResult DownloadWantedHighScoolItemsBiology()
        {

            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o => o.itm_year == year && o.itm_availableMethod == AvailableMethod.المستودع && o.itm_schId == school_id && o.itm_type != Domain.Type.اثاث_مختبر && o.itm_department == Department.ثانوى && o.itm_type == Domain.Type.احياء && o.itm_isExisting == ExsistState.غير_موجود).Select(o => new ItemViewModel
                    {
                        company = o.itm_School.sch_complex.comp_company.com_arName,
                        itm_code = o.itm_code,
                        itm_arName = o.itm_arName,
                        itm_enName = o.itm_enName,
                        itm_desc = o.itm_desc,

                        itm_department = o.itm_department.ToString(),

                        itm_level = o.itm_level.ToString(),

                        itm_type = o.itm_type.ToString(),



                        itm_Unit = o.itm_Unit.unt_arName,

                        itm_sugQty = o.itm_sugQty ?? 0.0,

                        itm_presentQty = o.itm_presentQty ?? 0.0,

                        itm_isExisting = o.itm_isExisting.ToString(),
                        itm_availableMethod = o.itm_availableMethod.ToString(),

                        itm_chapter = o.itm_chapter ?? 1,

                        itm_term = o.itm_term.ToString(),
                        itm_School = o.itm_School.sch_arName,
                        itm_ValidState = o.itm_ValidState.ToString(),
                        itm_completionYear = o.itm_completionYear ?? 0,
                        itm_excessiveQty = o.itm_excessiveQty ?? 0,
                        itm_note = o.itm_note,
                        sch_type = o.itm_School.sch_type.ToString()

                    }).ToList();

                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "GetWantedItems.rpt"));
                    DataSet ds = new DataSet();
                    ds = new LogoDataSet.LogoDataSet();
                    var comlogo = _companyRepository.GetAll().FirstOrDefault().com_image;
                    var complogo = _complexRepository.GetAll().FirstOrDefault().comp_image;

                    ds.Tables[0].Rows.Add(comlogo, complogo);
                    rd.Database.Tables[0].SetDataSource(model);
                    rd.Database.Tables[1].SetDataSource(ds.Tables[0]);

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "اصناف المرحبةالثانوية.pdf");
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception)
            {

                return RedirectToAction("GetWantedHighScoolItemsBiology");
            }
        }
        #endregion
        #region Get CanAvalable HighScoolItemsBiology
        [ScreenPermissionFilter(screenId = 67)]
        public ActionResult GetCanAvalableHighScoolItemsBiology(int? page)
        {


            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    var ModelList = _itemRepository.GetAllItemViewModel(x => x.itm_year == year && x.itm_availableMethod == AvailableMethod.المدرسة && x.itm_schId == school_id && x.itm_type != Domain.Type.اثاث_مختبر && x.itm_department == Department.ثانوى && x.itm_type == Domain.Type.احياء && x.itm_isExisting == ExsistState.غير_موجود);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.Id);
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

                return RedirectToAction("GetCanAvalableHighScoolItemsBiology");
            }
        }


        public ActionResult DownloadCanAvalableHighScoolItemsBiology()
        {

            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o => o.itm_year == year && o.itm_availableMethod == AvailableMethod.المدرسة && o.itm_schId == school_id && o.itm_type != Domain.Type.اثاث_مختبر && o.itm_department == Department.ثانوى && o.itm_type == Domain.Type.احياء && o.itm_isExisting == ExsistState.غير_موجود).Select(o => new ItemViewModel
                    {
                        company = o.itm_School.sch_complex.comp_company.com_arName,
                        itm_code = o.itm_code,
                        itm_arName = o.itm_arName,
                        itm_enName = o.itm_enName,
                        itm_desc = o.itm_desc,

                        itm_department = o.itm_department.ToString(),

                        itm_level = o.itm_level.ToString(),

                        itm_type = o.itm_type.ToString(),



                        itm_Unit = o.itm_Unit.unt_arName,

                        itm_sugQty = o.itm_sugQty ?? 0.0,

                        itm_presentQty = o.itm_presentQty ?? 0.0,

                        itm_isExisting = o.itm_isExisting.ToString(),
                        itm_availableMethod = o.itm_availableMethod.ToString(),

                        itm_chapter = o.itm_chapter ?? 1,

                        itm_term = o.itm_term.ToString(),
                        itm_School = o.itm_School.sch_arName,
                        itm_ValidState = o.itm_ValidState.ToString(),
                        itm_completionYear = o.itm_completionYear ?? 0,
                        itm_excessiveQty = o.itm_excessiveQty ?? 0,
                        itm_note = o.itm_note,
                        sch_type = o.itm_School.sch_type.ToString()

                    }).ToList();

                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "GetCanAvalableItems.rpt"));
                    DataSet ds = new DataSet();
                    ds = new LogoDataSet.LogoDataSet();
                    var comlogo = _companyRepository.GetAll().FirstOrDefault().com_image;
                    var complogo = _complexRepository.GetAll().FirstOrDefault().comp_image;

                    ds.Tables[0].Rows.Add(comlogo, complogo);
                    rd.Database.Tables[0].SetDataSource(model);
                    rd.Database.Tables[1].SetDataSource(ds.Tables[0]);

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "اصناف المرحبةالثانوية.pdf");
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception)
            {

                return RedirectToAction("GetCanAvalableHighScoolItemsBiology");
            }
        }
        #endregion
        #region GetexcessiveQtyHighScoolItemsBiology
        [ScreenPermissionFilter(screenId = 69)]
        public ActionResult GetexcessiveQtyHighScoolItemsBiology(int? page)
        {

            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    var ModelList = _itemRepository.GetAllItemViewModel(x => x.itm_year == year && x.itm_schId == school_id && x.itm_department == Department.ثانوى && x.itm_isExisting == ExsistState.موجود && x.itm_type == Domain.Type.احياء && x.itm_excessiveQty > 0);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.Id);
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

                return RedirectToAction("GetexcessiveQtyHighScoolItemsBiology");
            }

        }


        public ActionResult DownloadexcessiveQtyHighScoolItemsBiology()
        {
            try
            {

                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o => o.itm_year == year && o.itm_schId == school_id && o.itm_department == Department.ثانوى && o.itm_isExisting == ExsistState.موجود && o.itm_type == Domain.Type.احياء && o.itm_excessiveQty > 0).Select(o => new ItemViewModel
                    {
                        company = o.itm_School.sch_complex.comp_company.com_arName,
                        itm_code = o.itm_code,
                        itm_arName = o.itm_arName,
                        itm_enName = o.itm_enName,
                        itm_desc = o.itm_desc,

                        itm_department = o.itm_department.ToString(),

                        itm_level = o.itm_level.ToString(),

                        itm_type = o.itm_type.ToString(),



                        itm_Unit = o.itm_Unit.unt_arName,

                        itm_sugQty = o.itm_sugQty ?? 0.0,

                        itm_presentQty = o.itm_presentQty ?? 0.0,

                        itm_isExisting = o.itm_isExisting.ToString(),
                        itm_availableMethod = o.itm_availableMethod.ToString(),

                        itm_chapter = o.itm_chapter ?? 1,

                        itm_term = o.itm_term.ToString(),
                        itm_School = o.itm_School.sch_arName,
                        itm_ValidState = o.itm_ValidState.ToString(),
                        itm_completionYear = o.itm_completionYear ?? 0,
                        itm_excessiveQty = o.itm_excessiveQty ?? 0,
                        itm_note = o.itm_note,
                        sch_type = o.itm_School.sch_type.ToString()

                    }).ToList();

                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "GetExcessiveQty.rpt"));
                    DataSet ds = new DataSet();
                    ds = new LogoDataSet.LogoDataSet();
                    var comlogo = _companyRepository.GetAll().FirstOrDefault().com_image;
                    var complogo = _complexRepository.GetAll().FirstOrDefault().comp_image;

                    ds.Tables[0].Rows.Add(comlogo, complogo);
                    rd.Database.Tables[0].SetDataSource(model);
                    rd.Database.Tables[1].SetDataSource(ds.Tables[0]);

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "اصناف المرحبةالثانوية.pdf");
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception)
            {

                return RedirectToAction("GetexcessiveQtyHighScoolItemsBiology");
            }
        }
        #endregion
        #endregion
        #endregion


    }
}