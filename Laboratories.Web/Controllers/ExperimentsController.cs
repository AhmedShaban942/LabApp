using CrystalDecisions.CrystalReports.Engine;
using Laboratories.Application.Contracts;
using Laboratories.Domain;
using Laboratories.Web.Models;
using Laboratories.Web.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Laboratories.Web.Controllers
{
    [PermClass]
    public class ExperimentsController : Controller
    {
        
        private IExperimentRepository _experimentsRepository;
        private IRepository<Course> _courseRepository;
        private IRepository<Teacher> _teacherRepository;
        private IComplexRepository _complexRepository;
        private IRepository<Company> _companyRepository;
        private ISchoolRepository _schoolRepository;
        public ExperimentsController(IExperimentRepository experimentsRepository, IComplexRepository complexRepository, IRepository<Company> companyRepository, IRepository<Course> courseRepository, IRepository<Teacher> teacherRepository, ISchoolRepository schoolRepository)
        {
            this._experimentsRepository = experimentsRepository;
            this._courseRepository = courseRepository;
            this._teacherRepository = teacherRepository;
            this._complexRepository = complexRepository;
            this._companyRepository = companyRepository;
            this._schoolRepository = schoolRepository;
        }
        // GET: Experiments
        [ScreenPermissionFilter(screenId = 47)]
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



                    var model = from s in _experimentsRepository.GetAllExperimentsWithCourses().Where(x => x.expr_year == year && x.expr_teacher.tech_School.Id==(int)Session["ScoolId"])
                                select s;

                    //Search and match data, if search string is not null or empty
                    if (!String.IsNullOrEmpty(searchString))
                    {
                        model = model.Where(s => (s.expr_arName.Contains(searchString)));
                    }
                    var ModelList = model;
                    switch (sortOrder)
                    {
                        case "name_desc":
                            ModelList = model.OrderBy(x => x.expr_corsId).ThenBy(s => s.expr_arName);
                            break;

                        default:
                            ModelList = model.OrderBy(x => x.expr_corsId).ThenBy(s => s.expr_arName);
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
            catch (Exception ex)
            {


                return RedirectToAction("Index", "Home");
            }



        }

        // GET: Experiments/Details/5
        [ScreenPermissionFilter(screenId = 48)]
        public ActionResult Details(int id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Experiments experiment = _experimentsRepository.GetExperimentsWithCourseById(id);
                if (experiment == null)
                {
                    return HttpNotFound();
                }
                return View(experiment);
            }
            catch (Exception)
            {

                return RedirectToAction("Index");
            }

        }

        // GET: Experiments/Create
        [ScreenPermissionFilter(screenId = 49)]
        public ActionResult Create()
        {
            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    ViewBag.expr_corsId = new SelectList(_courseRepository.GetAll().Where(x => x.cors_department ==(Department)Session["Department"]), "Id", "cors_arName");
                ViewBag.expr_teachId = new SelectList(_teacherRepository.GetAll().Where(x=>x.tech_schId==(int)Session["ScoolId"]), "Id", "tech_arName");
                return View();
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

        // POST: Experiments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,expr_arName,expr_enName,expr_tpye,expr_page,expr_chapter,expr_corsId,expr_tools,expr_state,expr_teachId,expr_teacherSignature,AddedDate,ModifiedDate,AddedBy,ModifiedBy,IsAvtive,expr_ExecutionDate")] Experiments experiments)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                    {
                        User oUser = (User)Session["UserInfo"];
                        int year = (int)Session["CurrentYear"];
                        experiments.expr_year = year;
                        if (experiments.expr_state == State.لم_ينفذ)
                        {
                            experiments.expr_ExecutionDate = "";
                        }
                        _experimentsRepository.Insert(experiments);

                        return RedirectToAction("Index");
                    }
                    else
                        return RedirectToAction("Login", "Account");

                }
                else
                {
                   
                    return View(experiments);
                }
            }
            catch (Exception)
            {

                return RedirectToAction("Index");
            }



        }

        // GET: Experiments/Edit/5
        [ScreenPermissionFilter(screenId =50)]
        public ActionResult Edit(int? id)
        {
            try
            {
                if (Session["UserInfo"] != null)
                {
                    User oUser = (User)Session["UserInfo"];
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    Experiments experiments = _experimentsRepository.GetByNum(id);
                    if (experiments == null)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.expr_corsId = new SelectList(_courseRepository.GetAll().Where(x => x.cors_department == (Department)Session["Department"]), "Id", "cors_arName", experiments.expr_corsId);
                    ViewBag.expr_teachId = new SelectList(_teacherRepository.GetAll().Where(x => x.tech_schId == (int)Session["ScoolId"]), "Id", "tech_arName");

                    return View(experiments);
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

        // POST: Experiments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit([Bind(Include = "Id,expr_arName,expr_enName,expr_tpye,expr_page,expr_chapter,expr_corsId,expr_tools,expr_state,expr_teachId,expr_teacherSignature,AddedDate,ModifiedDate,AddedBy,ModifiedBy,IsAvtive,expr_year,expr_ExecutionDate")] Experiments experiments)
        {
            try
            {
                if (Session["UserInfo"] != null)
                {
                    User oUser = (User)Session["UserInfo"];
                    if (ModelState.IsValid)
                    {
                        if (experiments.expr_state == State.لم_ينفذ)
                        {
                            experiments.expr_ExecutionDate = "";
                        }
                        _experimentsRepository.Update(experiments);
                        return RedirectToAction("Index");
                    }
                    ViewBag.expr_corsId = new SelectList(_courseRepository.GetAll(), "Id", "cors_arName", experiments.expr_corsId);
                    ViewBag.expr_teachId = new SelectList(_teacherRepository.GetAll().Where(x => x.tech_schId == (int)Session["ScoolId"]), "Id", "tech_arName");

                    return View(experiments);
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

        // GET: Experiments/Delete/5
        [ScreenPermissionFilter(screenId = 51)]
        public ActionResult Delete(int? id)
        {
            try
            {
                if (Session["UserInfo"] != null)
                {
                    User oUser = (User)Session["UserInfo"];
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    Experiments experiments = _experimentsRepository.GetByNum(id);
                    if (experiments == null)
                    {
                        return HttpNotFound();
                    }
                    return View(experiments);
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

        // POST: Experiments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                if (Session["UserInfo"] != null)
                {
                    _experimentsRepository.Delete(id);

                    return RedirectToAction("Index");
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

        #region Report
        #region GetDone
        public ActionResult GetDone(int? page)
        {


            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    var ModelList = _experimentsRepository.GetAllExperimentsWithCourses().Where(x => x.expr_year == year && x.expr_teacher.tech_School.Id == (int)Session["ScoolId"] && x.expr_state == State.نفذ);
                    ModelList = ModelList.OrderBy(x => x.expr_corsId).ThenBy(s => s.expr_arName).ToList();
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
            catch (Exception ex)
            {

                return RedirectToAction("Index", "Home");
            }
        }


        public ActionResult DownloadDone()
        {


            try
            {

                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];

                    List<ExperimentsViewModel> ModelList = _experimentsRepository.GetAllExperimentsWithCourses().Where(x => x.expr_year == year && x.expr_teacher.tech_School.Id == (int)Session["ScoolId"] && x.expr_state == State.نفذ).OrderBy(x => x.expr_corsId).ThenBy(s => s.expr_arName).Select(o => new ExperimentsViewModel
                    {
                        expr_arName = o.expr_arName,
                        expr_enName = o.expr_enName,

                        expr_tpye = o.expr_tpye,
                        expr_page = o.expr_page,
                        expr_chapter = o.expr_chapter,

                        expr_course = _courseRepository.GetByNum(o.expr_corsId).cors_arName,

                        expr_tools = o.expr_tools,

                        expr_state = o.expr_state.ToString(),

                        expr_teacherName = o.expr_teacher.tech_arName,

                        expr_teacherSignature = o.expr_teacherSignature,
                        expr_year = o.expr_year,
                        expr_ExecutionDate = o.expr_ExecutionDate

                    }).ToList();
                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "GetDoneExperiments.rpt"));
                    DataSet ds = new DataSet();
                    ds = new LogoDataSet.LogoDataSet();
                    var comlogo = _companyRepository.GetAll().FirstOrDefault().com_image;
                    var complogo = _complexRepository.GetAll().FirstOrDefault().comp_image;

                    ds.Tables[0].Rows.Add(comlogo, complogo);
                    rd.Database.Tables[0].SetDataSource(ModelList);
                    rd.Database.Tables[1].SetDataSource(ds.Tables[0]);

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "  اسماء التجارب  المنفذة.pdf");
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
        #region GetUnAvalable
        public ActionResult GetNotDone(int? page)
        {

            try
            {
                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    var ModelList = _experimentsRepository.GetAllExperimentsWithCourses().Where(x => x.expr_year == year && x.expr_teacher.tech_School.Id == (int)Session["ScoolId"] && x.expr_state == State.لم_ينفذ);
                    ModelList = ModelList.OrderBy(x => x.expr_corsId).ThenBy(s => s.expr_arName).ToList();
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


        public ActionResult DownloadNotDone()
        {
            try
            {

                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];
                    List<ExperimentsViewModel> ModelList = _experimentsRepository.GetAllExperimentsWithCourses().Where(x => x.expr_year == year && x.expr_teacher.tech_School.Id == (int)Session["ScoolId"] && x.expr_state == State.لم_ينفذ).OrderBy(x => x.expr_corsId).ThenBy(s => s.expr_arName).Select(o => new ExperimentsViewModel
                    {
                        expr_arName = o.expr_arName,
                        expr_enName = o.expr_enName,

                        expr_tpye = o.expr_tpye,
                        expr_page = o.expr_page,
                        expr_chapter = o.expr_chapter,

                        expr_course = _courseRepository.GetByNum(o.expr_corsId).cors_arName,

                        expr_tools = o.expr_tools,

                        expr_state = o.expr_state.ToString(),

                        expr_teacherName = o.expr_teacher.tech_arName,

                        expr_teacherSignature = o.expr_teacherSignature,
                        expr_year = o.expr_year,
                        expr_ExecutionDate = o.expr_ExecutionDate

                    }).ToList();

                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "GetNotDoneExperiments.rpt"));
                    DataSet ds = new DataSet();
                    ds = new LogoDataSet.LogoDataSet();
                    var comlogo = _companyRepository.GetAll().FirstOrDefault().com_image;
                    var complogo = _complexRepository.GetAll().FirstOrDefault().comp_image;

                    ds.Tables[0].Rows.Add(comlogo, complogo);
                    rd.Database.Tables[0].SetDataSource(ModelList);
                    rd.Database.Tables[1].SetDataSource(ds.Tables[0]);
                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "  اسماء التجارب الغير منفذة.pdf");
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

        #region Done

        #region GetDoneByTeacher
        public ActionResult GetDoneByTeacher()
        {
            ViewBag.company = new SelectList(_companyRepository.GetAll(), "Id", "com_arName");
            ViewBag.complex = new SelectList(_complexRepository.GetAll(), "Id", "comp_arName");
            ViewBag.school = new SelectList(_schoolRepository.GetAll(), "Id", "sch_arName");


            return View();
        }


        public ActionResult DownloadDoneByTeacher(int teacher)
        {


            try
            {

                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];

                    List<ExperimentsViewModel> ModelList = _experimentsRepository.GetAllExperimentsWithCourses().Where(x => x.expr_year == year && x.expr_teachId==teacher && x.expr_state == State.نفذ).OrderBy(x => x.expr_corsId).ThenBy(s => s.expr_arName).Select(o => new ExperimentsViewModel
                    {
                        expr_arName = o.expr_arName,
                        expr_enName = o.expr_enName,

                        expr_tpye = o.expr_tpye,
                        expr_page = o.expr_page,
                        expr_chapter = o.expr_chapter,

                        expr_course = _courseRepository.GetByNum(o.expr_corsId).cors_arName,

                        expr_tools = o.expr_tools,

                        expr_state = o.expr_state.ToString(),

                        expr_teacherName = o.expr_teacher.tech_arName,

                        expr_teacherSignature = o.expr_teacherSignature,
                        expr_year = o.expr_year,
                        expr_ExecutionDate = o.expr_ExecutionDate

                    }).ToList();
                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "GetDoneExperiments.rpt"));
                    DataSet ds = new DataSet();
                    ds = new LogoDataSet.LogoDataSet();
                    var comlogo = _companyRepository.GetAll().FirstOrDefault().com_image;
                    var complogo = _complexRepository.GetAll().FirstOrDefault().comp_image;

                    ds.Tables[0].Rows.Add(comlogo, complogo);
                    rd.Database.Tables[0].SetDataSource(ModelList);
                    rd.Database.Tables[1].SetDataSource(ds.Tables[0]);

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "  اسماء التجارب  المنفذة.pdf");
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

        #region GetDoneByDepartment
        public ActionResult GetDoneByDepartment()
        {
            List<Department> departments = new List<Department>();
            ViewBag.company = new SelectList(_companyRepository.GetAll(), "Id", "com_arName");
            ViewBag.complex = new SelectList(_complexRepository.GetAll(), "Id", "comp_arName");
            ViewBag.department = new SelectList(departments);


            return View();
        }


        public ActionResult DownloadDoneByDepartment(int company, int complex, int department)
        {


            try
            {

                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];

                    List<ExperimentsViewModel> ModelList = _experimentsRepository.GetAllExperimentsWithCourses().Where(x => x.expr_year == year && x.expr_teacher.tech_School.sch_department ==(SchoolDepartment) department && x.expr_teacher.tech_School.sch_complex.comp_com_id == company && x.expr_teacher.tech_School.sch_comp_id == complex && x.expr_state == State.نفذ).OrderBy(x => x.expr_corsId).ThenBy(s => s.expr_arName).Select(o => new ExperimentsViewModel
                    {
                        expr_arName = o.expr_arName,
                        expr_enName = o.expr_enName,

                        expr_tpye = o.expr_tpye,
                        expr_page = o.expr_page,
                        expr_chapter = o.expr_chapter,

                        expr_course = _courseRepository.GetByNum(o.expr_corsId).cors_arName,

                        expr_tools = o.expr_tools,

                        expr_state = o.expr_state.ToString(),

                        expr_teacherName = o.expr_teacher.tech_arName,

                        expr_teacherSignature = o.expr_teacherSignature,
                        expr_year = o.expr_year,
                        expr_ExecutionDate = o.expr_ExecutionDate

                    }).ToList();
                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "GetDoneExperiments.rpt"));
                    DataSet ds = new DataSet();
                    ds = new LogoDataSet.LogoDataSet();
                    var comlogo = _companyRepository.GetAll().FirstOrDefault().com_image;
                    var complogo = _complexRepository.GetAll().FirstOrDefault().comp_image;

                    ds.Tables[0].Rows.Add(comlogo, complogo);
                    rd.Database.Tables[0].SetDataSource(ModelList);
                    rd.Database.Tables[1].SetDataSource(ds.Tables[0]);

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "  اسماء التجارب  المنفذة.pdf");
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

        #region NotDone
        #region GetDoneByTeacher
        public ActionResult GetNotDoneByTeacher()
        {
            ViewBag.company = new SelectList(_companyRepository.GetAll(), "Id", "com_arName");
            ViewBag.complex = new SelectList(_complexRepository.GetAll(), "Id", "comp_arName");
            ViewBag.school = new SelectList(_schoolRepository.GetAll(), "Id", "sch_arName");


            return View();
        }


        public ActionResult DownloadNotDoneByTeacher(int teacher)
        {


            try
            {

                if ((User)Session["UserInfo"] != null && (int)Session["CurrentYear"] != 0)
                {
                    User oUser = (User)Session["UserInfo"];
                    int year = (int)Session["CurrentYear"];

                    List<ExperimentsViewModel> ModelList = _experimentsRepository.GetAllExperimentsWithCourses().Where(x => x.expr_year == year && x.expr_teachId == teacher && x.expr_state == State.نفذ).OrderBy(x => x.expr_corsId).ThenBy(s => s.expr_arName).Select(o => new ExperimentsViewModel
                    {
                        expr_arName = o.expr_arName,
                        expr_enName = o.expr_enName,

                        expr_tpye = o.expr_tpye,
                        expr_page = o.expr_page,
                        expr_chapter = o.expr_chapter,

                        expr_course = _courseRepository.GetByNum(o.expr_corsId).cors_arName,

                        expr_tools = o.expr_tools,

                        expr_state = o.expr_state.ToString(),

                        expr_teacherName = o.expr_teacher.tech_arName,

                        expr_teacherSignature = o.expr_teacherSignature,
                        expr_year = o.expr_year,
                        expr_ExecutionDate = o.expr_ExecutionDate

                    }).ToList();
                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "GetNotDoneExperiments.rpt"));
                    DataSet ds = new DataSet();
                    ds = new LogoDataSet.LogoDataSet();
                    var comlogo = _companyRepository.GetAll().FirstOrDefault().com_image;
                    var complogo = _complexRepository.GetAll().FirstOrDefault().comp_image;

                    ds.Tables[0].Rows.Add(comlogo, complogo);
                    rd.Database.Tables[0].SetDataSource(ModelList);
                    rd.Database.Tables[1].SetDataSource(ds.Tables[0]);

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "  اسماء التجارب  المنفذة.pdf");
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
