using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
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
    public class LaboratoryBasisReportsController : Controller
    {
        private IItemRepository _itemRepository;
        private IComplexRepository _complexRepository;
        private IRepository<Company> _companyRepository;
        private ISchoolRepository _schoolRepository;
        public LaboratoryBasisReportsController(IItemRepository itemRepository, IComplexRepository complexRepository, IRepository<Company> companyRepository, ISchoolRepository schoolRepository)
        {

            this._itemRepository = itemRepository;
            this._complexRepository = complexRepository;
            this._companyRepository = companyRepository;
            this._schoolRepository = schoolRepository;


        }

        #region Custom Report
        [ScreenPermissionFilter(screenId = 75)]
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

                return RedirectToAction("CustomReport");
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
                    var items = _itemRepository.GetAllItemViewModel(x=>x.itm_year==year);
                    if (complexId == null)
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
                        schoolId = (int)Session["ScoolId"];
                        items = _itemRepository.GetAllItemViewModel(x => x.itm_year == year && x.itm_schId == school_id);
                    }
                    List<ItemViewModel> model = new List<ItemViewModel>();
                    if (reportType == "GetItemsCustom")
                    {
                        model = items.Where(x => x.itm_type == Domain.Type.اثاث_مختبر).Select(o => new ItemViewModel
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
                        model = items.Where(x => x.itm_availableMethod == AvailableMethod.المدرسة && x.itm_isExisting == ExsistState.غير_موجود && x.itm_type == Domain.Type.اثاث_مختبر).Select(o => new ItemViewModel
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
                        model = items.Where(x => x.itm_availableMethod == AvailableMethod.المستودع && x.itm_isExisting == ExsistState.غير_موجود && x.itm_type == Domain.Type.اثاث_مختبر).Select(o => new ItemViewModel
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
                        model = items.Where(x => x.itm_over == Over.زائد && x.itm_type == Domain.Type.اثاث_مختبر).Select(o => new ItemViewModel
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
                        model = items.Where(x => x.itm_isExisting == ExsistState.موجود && x.itm_type == Domain.Type.اثاث_مختبر).Select(o => new ItemViewModel
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
                        model = items.Where(x => x.itm_isExisting == ExsistState.موجود && x.itm_type == Domain.Type.اثاث_مختبر && x.itm_excessiveQty>0).Select(o => new ItemViewModel
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
                        model = items.Where(x => x.itm_isExisting == ExsistState.غير_موجود && x.itm_type == Domain.Type.اثاث_مختبر).Select(o => new ItemViewModel
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

                return RedirectToAction("CustomReport");
            }




        }

        #endregion

        // GET: Report
        #region All Items
        [ScreenPermissionFilter(screenId = 71)]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    ViewBag.CurrentSort = sortOrder;
                    ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                    var ModelList = _itemRepository.GetAllItemWithUnit(x =>x.itm_year==year&& x.itm_type == Domain.Type.اثاث_مختبر && x.itm_schId == school_id);

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



                    var model = from s in _itemRepository.GetAllItemWithUnit(x =>x.itm_year==year&& x.itm_type == Domain.Type.اثاث_مختبر && x.itm_schId == school_id)
                                select s;
                    //Search and match data, if search string is not null or empty
                    if (!String.IsNullOrEmpty(searchString))
                    {
                        model = model.Where(s => s.itm_schId == school_id && (s.itm_arName.Contains(searchString)
                                               || s.itm_enName.Contains(searchString)
                                               || s.itm_desc.Contains(searchString)
                                               || s.itm_Unit.unt_arName.Contains(searchString)));
                    }
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
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(x =>x.itm_year==year&& x.itm_type == Domain.Type.اثاث_مختبر && x.itm_schId == school_id && x.itm_type == Domain.Type.اثاث_مختبر).Select(o => new ItemViewModel
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
        [ScreenPermissionFilter(screenId = 71)]
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
                    var ModelList = _itemRepository.GetAllItemWithUnit(x =>x.itm_year==year&& x.itm_type == Domain.Type.اثاث_مختبر && x.itm_schId == school_id&& x.itm_department == 0);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.itm_arName);
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
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o =>o.itm_year==year&& o.itm_type == Domain.Type.اثاث_مختبر && o.itm_department == 0 && o.itm_schId == school_id).Select(o => new ItemViewModel
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
                return RedirectToAction("Index");
            }



        }
        #endregion

        #region GetPresentPrimaryItems
        [ScreenPermissionFilter(screenId = 72)]
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
                    var ModelList = _itemRepository.GetAllItemWithUnit(x =>x.itm_year==year&& x.itm_department == 0 && x.itm_schId == school_id && x.itm_type == Domain.Type.اثاث_مختبر && x.itm_isExisting == ExsistState.موجود);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.itm_arName);
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
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o =>o.itm_year==year&& o.itm_department == 0 && o.itm_schId ==school_id && o.itm_type == Domain.Type.اثاث_مختبر && o.itm_isExisting == ExsistState.موجود).Select(o => new ItemViewModel
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
                return RedirectToAction("Index");
            }

        }
        #endregion
        #region GetUnAvalablePrimaryItems
        [ScreenPermissionFilter(screenId = 73)]
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
                    var ModelList = _itemRepository.GetAllItemWithUnit(x =>x.itm_year==year&& x.itm_department == 0 && x.itm_schId == school_id && x.itm_isExisting == ExsistState.غير_موجود && x.itm_type == Domain.Type.اثاث_مختبر);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.itm_arName);
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
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o =>o.itm_year==year&& o.itm_department == 0 && o.itm_schId ==school_id && o.itm_type == Domain.Type.اثاث_مختبر && o.itm_isExisting == ExsistState.غير_موجود).Select(o => new ItemViewModel
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
                    return File(stream, "application/pdf", "اصناف المرحبة الابتدائية.pdf");
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

        #region Get Over Primary Items
        [ScreenPermissionFilter(screenId = 74)]
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
                    var ModelList = _itemRepository.GetAllItemWithUnit(x =>x.itm_year==year&& x.itm_department == 0 && x.itm_schId == school_id && x.itm_type == Domain.Type.اثاث_مختبر && x.itm_over == Over.زائد);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.itm_arName);
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


        public ActionResult DownloadOverPrimaryItems()
        {
            if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
            {
                User oUser = (User)Session["UserInfo"];
                int year = (int)Session["CurrentYear"];

                int school_id = 0;
                school_id = (int)Session["ScoolId"];
                List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o =>o.itm_year==year&& o.itm_department == 0 && o.itm_schId == school_id&& o.itm_type == Domain.Type.اثاث_مختبر && o.itm_over == Over.زائد).Select(o => new ItemViewModel
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
        #endregion

        #region Get Wanted Primary Items
        [ScreenPermissionFilter(screenId = 72)]
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
                    var ModelList = _itemRepository.GetAllItemWithUnit(x =>x.itm_year==year&& x.itm_availableMethod == 0 && x.itm_department == 0 && x.itm_schId == school_id&& x.itm_type == Domain.Type.اثاث_مختبر && x.itm_isExisting == ExsistState.غير_موجود);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.itm_arName);
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
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o =>o.itm_year==year&& o.itm_availableMethod == 0 && o.itm_department == 0 && o.itm_schId == school_id && o.itm_type == Domain.Type.اثاث_مختبر && o.itm_isExisting == ExsistState.غير_موجود).Select(o => new ItemViewModel
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

                return RedirectToAction("Index");
            }
        }
        #endregion


        #region Get Can Avalable Primary Items
        [ScreenPermissionFilter(screenId = 74)]
        public ActionResult GetCanAvalablePrimaryItems(int? page)
        {

            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"]; int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    var ModelList = _itemRepository.GetAllItemWithUnit(x =>x.itm_year==year  &&  x.itm_availableMethod == AvailableMethod.المدرسة && x.itm_department == 0 && x.itm_schId == school_id && x.itm_type == Domain.Type.اثاث_مختبر && x.itm_isExisting == ExsistState.غير_موجود);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.itm_arName);
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
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o =>o.itm_year==year&& o.itm_availableMethod == AvailableMethod.المدرسة && o.itm_department == 0 && o.itm_schId == school_id && o.itm_type == Domain.Type.اثاث_مختبر && o.itm_isExisting == ExsistState.غير_موجود).Select(o => new ItemViewModel
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
                return RedirectToAction("Index");
            }
        }
        #endregion
        #region GetexcessiveQtyPrimaryItems
        [ScreenPermissionFilter(screenId = 72)]
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
                    var ModelList = _itemRepository.GetAllItemWithUnit(x =>x.itm_year==year&& x.itm_department == 0 && x.itm_schId == school_id && x.itm_type == Domain.Type.اثاث_مختبر && x.itm_isExisting == ExsistState.موجود &&x.itm_excessiveQty>0);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.itm_arName);
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
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o =>o.itm_year==year&& o.itm_department == 0 && o.itm_schId == school_id && o.itm_type == Domain.Type.اثاث_مختبر && o.itm_isExisting == ExsistState.موجود && o.itm_excessiveQty>0).Select(o => new ItemViewModel
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

                return RedirectToAction("Index");
            }

        }
        #endregion
        #endregion

        #region Secondry DepartMent
        #region GetSecondryItems
        [ScreenPermissionFilter(screenId = 71)]
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
                    var ModelList = _itemRepository.GetAllItemWithUnit(x =>x.itm_year==year&& x.itm_type == Domain.Type.اثاث_مختبر && x.itm_schId ==school_id && x.itm_department == Department.متوسط);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.itm_arName);
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
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o =>o.itm_year==year&& o.itm_type == Domain.Type.اثاث_مختبر && o.itm_schId == school_id && o.itm_department == Department.متوسط).Select(o => new ItemViewModel
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
                    return File(stream, "application/pdf", "اصناف المرحبة المتوسطة.pdf");
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

        #region GetPresentSecondryItems
        [ScreenPermissionFilter(screenId = 72)]
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
                    var ModelList = _itemRepository.GetAllItemWithUnit(x =>x.itm_year==year&& x.itm_type == Domain.Type.اثاث_مختبر && x.itm_schId == school_id && x.itm_department == Department.متوسط && x.itm_isExisting == ExsistState.موجود);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.itm_arName);
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
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o =>o.itm_year==year&& o.itm_type == Domain.Type.اثاث_مختبر && o.itm_schId == school_id && o.itm_department == Department.متوسط && o.itm_isExisting == ExsistState.موجود).Select(o => new ItemViewModel
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
                return RedirectToAction("Index");
            }
        }
        #endregion

        #region GetUnAvalableSecondryItems
        [ScreenPermissionFilter(screenId = 73)]
        public ActionResult GetUnAvalableSecondryItems(int? page)
        {


            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"]; int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    var ModelList = _itemRepository.GetAllItemWithUnit(x =>x.itm_year==year&& x.itm_type == Domain.Type.اثاث_مختبر && x.itm_schId == school_id && x.itm_department == Department.متوسط && x.itm_isExisting == ExsistState.غير_موجود);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.itm_arName);
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
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o =>o.itm_year==year&& o.itm_type == Domain.Type.اثاث_مختبر && o.itm_schId ==school_id && o.itm_department == Department.متوسط && o.itm_isExisting == ExsistState.غير_موجود).Select(o => new ItemViewModel
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

                return RedirectToAction("Index");
            }
        }
        #endregion

        #region Get Over  SecondryItems
        [ScreenPermissionFilter(screenId = 74)]
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
                    var ModelList = _itemRepository.GetAllItemWithUnit(x =>x.itm_year==year&& x.itm_type == Domain.Type.اثاث_مختبر && x.itm_schId == school_id&& x.itm_department == Department.متوسط && x.itm_over == Over.زائد);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.itm_arName);
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


        public ActionResult DownloadOverSecondryItems()
        {


            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o =>o.itm_year==year&& o.itm_type == Domain.Type.اثاث_مختبر && o.itm_schId == school_id && o.itm_department == Department.متوسط && o.itm_over == Over.زائد).Select(o => new ItemViewModel
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
                return RedirectToAction("Index");
            }
        }
        #endregion

        #region Get Wanted  SecondryItems
        [ScreenPermissionFilter(screenId = 72)]
        public ActionResult GetWantedSecondryItems(int? page)
        {

            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    var ModelList = _itemRepository.GetAllItemWithUnit(x =>x.itm_year==year&& x.itm_availableMethod == AvailableMethod.المستودع && x.itm_schId == school_id&& x.itm_type == Domain.Type.اثاث_مختبر && x.itm_department == Department.متوسط && x.itm_isExisting == ExsistState.غير_موجود);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.itm_arName);
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


        public ActionResult DownloadWantedSecondryItems()
        {


            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o =>o.itm_year==year&& o.itm_availableMethod == AvailableMethod.المستودع && o.itm_schId == school_id&& o.itm_type == Domain.Type.اثاث_مختبر && o.itm_department == Department.متوسط && o.itm_isExisting == ExsistState.غير_موجود).Select(o => new ItemViewModel
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

                return RedirectToAction("Index");
            }
        }
        #endregion
        #region Get CanAvalable  SecondryItems
        [ScreenPermissionFilter(screenId = 72)]
        public ActionResult GetCanAvalableSecondryItems(int? page)
        {

            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    var ModelList = _itemRepository.GetAllItemWithUnit(x =>x.itm_year==year&& x.itm_availableMethod == AvailableMethod.المدرسة && x.itm_schId == school_id && x.itm_type == Domain.Type.اثاث_مختبر && x.itm_department == Department.متوسط && x.itm_isExisting == ExsistState.غير_موجود);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.itm_arName);
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


        public ActionResult DownloadCanAvalableSecondryItems()
        {


            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o =>o.itm_year==year&& o.itm_availableMethod == AvailableMethod.المدرسة && o.itm_schId == school_id && o.itm_type == Domain.Type.اثاث_مختبر && o.itm_department == Department.متوسط && o.itm_isExisting == ExsistState.غير_موجود).Select(o => new ItemViewModel
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
                return RedirectToAction("Index");
            }
        }
        #endregion
        #region GetexcessiveQtySecondryItems
        [ScreenPermissionFilter(screenId = 74)]
        public ActionResult GetexcessiveQtySecondryItems(int? page)
        {

            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    var ModelList = _itemRepository.GetAllItemWithUnit(x =>x.itm_year==year&& x.itm_type == Domain.Type.اثاث_مختبر && x.itm_schId ==school_id && x.itm_department == Department.متوسط && x.itm_isExisting == ExsistState.موجود && x.itm_excessiveQty>0);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.itm_arName);
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


        public ActionResult DownloadexcessiveQtySecondryItems()
        {
            try
            {

                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o =>o.itm_year==year&& o.itm_type == Domain.Type.اثاث_مختبر && o.itm_schId ==school_id && o.itm_department == Department.متوسط && o.itm_isExisting == ExsistState.موجود && o.itm_excessiveQty>0).Select(o => new ItemViewModel
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
                    return File(stream, "application/pdf", "اصناف المرحبة المتوسطة.pdf");
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
        #endregion


        #region High School DepartMent

        #region Get High Scool Items 
        [ScreenPermissionFilter(screenId = 71)]
        public ActionResult GetHighScoolItems(int? page)
        {
            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    var ModelList = _itemRepository.GetAllItemWithUnit(x =>x.itm_year==year&& x.itm_schId == school_id && x.itm_department == Department.ثانوى && x.itm_type == Domain.Type.اثاث_مختبر);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.itm_arName);
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


        public ActionResult DownloadHighScoolItems()
        {

            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
            
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o =>o.itm_year==year&& o.itm_schId == school_id&& o.itm_department == Department.ثانوى && o.itm_type == Domain.Type.اثاث_مختبر).Select(o => new ItemViewModel
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

        #region GetPresentHighScoolItems
        [ScreenPermissionFilter(screenId = 72)]
        public ActionResult GetPresentHighScoolItems(int? page)
        {


            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    var ModelList = _itemRepository.GetAllItemWithUnit(x =>x.itm_year==year&& x.itm_schId == school_id && x.itm_department == Department.ثانوى && x.itm_isExisting == ExsistState.موجود && x.itm_type == Domain.Type.اثاث_مختبر);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.itm_arName);
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


        public ActionResult DownloadPresentHighScoolItems()
        {

            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o =>o.itm_year==year&& o.itm_schId == school_id && o.itm_department == Department.ثانوى && o.itm_isExisting == ExsistState.موجود && o.itm_type == Domain.Type.اثاث_مختبر).Select(o => new ItemViewModel
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
        #region GetUnAvalableHighScoolItems
        [ScreenPermissionFilter(screenId = 73)]
        public ActionResult GetUnAvalableHighScoolItems(int? page)
        {

            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    var ModelList = _itemRepository.GetAllItemWithUnit(x =>x.itm_year==year&& x.itm_schId == school_id && x.itm_department == Department.ثانوى && x.itm_isExisting == ExsistState.غير_موجود && x.itm_type == Domain.Type.اثاث_مختبر);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.itm_arName);
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


        public ActionResult DownloadUnAvalableHighScoolItems()
        {


            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o =>o.itm_year==year&& o.itm_schId == school_id && o.itm_department == Department.ثانوى && o.itm_isExisting == ExsistState.غير_موجود && o.itm_type == Domain.Type.اثاث_مختبر).Select(o => new ItemViewModel
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

                return RedirectToAction("Index");
            }
        }
        #endregion

        #region GetOverHighScoolItems
        [ScreenPermissionFilter(screenId = 74)]
        public ActionResult GetOverHighScoolItems(int? page)
        {

            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    var ModelList = _itemRepository.GetAllItemWithUnit(x =>x.itm_year==year&& x.itm_schId == school_id && x.itm_department == Department.ثانوى && x.itm_over == Over.زائد && x.itm_type == Domain.Type.اثاث_مختبر);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.itm_arName);
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


        public ActionResult DownloadOverHighScoolItems()
        {


            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o =>o.itm_year==year&& o.itm_schId == school_id && o.itm_department == Department.ثانوى && o.itm_over == Over.زائد && o.itm_type == Domain.Type.اثاث_مختبر).Select(o => new ItemViewModel
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

                return RedirectToAction("Index");
            }
        }
        #endregion

        #region GetWantedHighScoolItems
        [ScreenPermissionFilter(screenId = 72)]
        public ActionResult GetWantedHighScoolItems(int? page)
        {

            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    var ModelList = _itemRepository.GetAllItemWithUnit(x =>x.itm_year==year&& x.itm_availableMethod == AvailableMethod.المستودع && x.itm_schId == school_id && x.itm_type == Domain.Type.اثاث_مختبر && x.itm_department == Department.ثانوى);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.itm_arName);
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


        public ActionResult DownloadWantedHighScoolItems()
        {

            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o =>o.itm_year==year&& o.itm_availableMethod == AvailableMethod.المستودع && o.itm_schId == school_id && o.itm_type == Domain.Type.اثاث_مختبر && o.itm_department == Department.ثانوى).Select(o => new ItemViewModel
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

                return RedirectToAction("Index");
            }
        }
        #endregion
        #region Get CanAvalable HighScoolItems
        [ScreenPermissionFilter(screenId = 72)]
        public ActionResult GetCanAvalableHighScoolItems(int? page)
        {


            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    var ModelList = _itemRepository.GetAllItemWithUnit(x =>x.itm_year==year&& x.itm_availableMethod == AvailableMethod.المدرسة && x.itm_schId == school_id&& x.itm_type == Domain.Type.اثاث_مختبر && x.itm_department == Department.ثانوى);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.itm_arName);
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


        public ActionResult DownloadCanAvalableHighScoolItems()
        {

            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o =>o.itm_year==year&& o.itm_availableMethod == AvailableMethod.المدرسة && o.itm_schId ==school_id && o.itm_type == Domain.Type.اثاث_مختبر && o.itm_department == Department.ثانوى).Select(o => new ItemViewModel
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
                return RedirectToAction("Index");
            }
        }
        #endregion

        #region GetexcessiveQtyeHighScoolItems
        [ScreenPermissionFilter(screenId = 74)]
        public ActionResult GetexcessiveQtyHighScoolItems(int? page)
        {


            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    var ModelList = _itemRepository.GetAllItemWithUnit(x =>x.itm_year==year&& x.itm_schId == school_id && x.itm_department == Department.ثانوى && x.itm_isExisting == ExsistState.موجود && x.itm_type == Domain.Type.اثاث_مختبر && x.itm_excessiveQty>0);
                    ModelList = ModelList.OrderBy(x => x.itm_availableMethod).ThenBy(s => s.itm_arName);
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


        public ActionResult DownloadexcessiveQtyHighScoolItems()
        {

            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    int school_id = 0;
                    school_id = (int)Session["ScoolId"];
                    List<ItemViewModel> model = _itemRepository.GetAllItemViewModel(o =>o.itm_year==year&& o.itm_schId ==school_id && o.itm_department == Department.ثانوى && o.itm_isExisting == ExsistState.موجود && o.itm_type == Domain.Type.اثاث_مختبر && o.itm_excessiveQty>0).Select(o => new ItemViewModel
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

                return RedirectToAction("Index");
            }
        }
        #endregion
        #endregion


        #endregion


    }
}