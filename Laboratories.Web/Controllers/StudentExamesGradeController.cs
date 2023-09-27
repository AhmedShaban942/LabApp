using CrystalDecisions.CrystalReports.Engine;
using Laboratories.Application.Contracts;
using Laboratories.Domain;
using Laboratories.Persistence;
using Laboratories.Web.Models;
using Laboratories.Web.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Mvc;

namespace Laboratories.Web.Controllers
{

    [PermClass]
    public class StudentExamesGradeController : Controller
    {
        private IComplexRepository _complexRepository;
        private IRepository<Company> _companyRepository;
        private IRepository<Teacher> _teacherRepository;
        private IRepository<StudyCourse> _studyCourseRepository;
        private IRepository<Student> _studentRepository;
        private IRepository<Exam> _examRepository;
        private IStudentGradesHedRepository _studentGradesHedRepository;
        private IStudentGradesDetRepository _studentGradesDetRepository;
        public StudentExamesGradeController(IStudentGradesHedRepository studentGradesHedRepository, IStudentGradesDetRepository studentGradesDetRepository, IRepository<Teacher> teacherRepository, IItemRepository itemRepository, IComplexRepository complexRepository, IRepository<Company> companyRepository,  IRepository<StudyCourse> studyCourseRepository, IRepository<Student> studentRepository,IRepository<Exam> examRepository)
        {
            this._teacherRepository = teacherRepository;
            this._complexRepository = complexRepository;
            this._companyRepository = companyRepository;
            this._studyCourseRepository = studyCourseRepository;
            this._studentRepository = studentRepository;
            this._examRepository = examRepository;
            this._studentGradesHedRepository = studentGradesHedRepository;
            this._studentGradesDetRepository = studentGradesDetRepository;
        }
        private LaboratoryDbContext db = new LaboratoryDbContext();

        public JsonResult GetStudyCourse(int department_Id,int level_Id,int term_Id)

        {
            try
            {
                var studyCourseList = _studyCourseRepository.MultiSearch(x => x.cors_department == (Department)department_Id && x.cors_level == (Level)level_Id && x.cors_term == (Term)term_Id);
                return Json(studyCourseList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json(null);
            }


        }

        public JsonResult GetStudyCourseRate(int StudyCourse)
        {
            try
            {
                var studyCourse = _studyCourseRepository.GetByNum(StudyCourse);
                return Json(studyCourse, JsonRequestBehavior.AllowGet);
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
                var schoolList = _teacherRepository.MultiSearch(x => x.tech_schId ==school_Id);
                return Json(schoolList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json(null);
            }


        }


        public JsonResult GetStudent(int school_Id,int department_Id, int level_Id, int record_Id)

        {
            try
            {
                var studentList = _studentRepository.MultiSearch(x => x.std_department == (Department)department_Id && x.std_level == (Level)level_Id && x.std_levelRecord == (record)record_Id);
                return Json(studentList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json(null);
            }


        }


        // GET: StudentExamesGrade
        [ScreenPermissionFilter(screenId = 95)]
        public ActionResult Index(int? page)
        {
            try
            {
                if (Session["UserInfo"] != null)
                {
                    ViewBag.company = new SelectList(_companyRepository.GetAll(), "Id", "com_arName");
                    ViewBag.complex = new SelectList(_complexRepository.GetAll(), "Id", "comp_arName");

                    var ModelList = _studentGradesHedRepository.GetAllStudentGradesHedWithStudent().OrderBy(x => x.stdGH_Id);
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

                                        
        public JsonResult SaveExamGrade(int schoolid, int teacherid, int studyCourseid,string date,int term,int level,int stdRecord,int examid, [FromBody] List<StudentGradesDet> StudentGradesDet)
        {
            try
            {
                var courseRate = 0.0;
                var courseStudy  = _studyCourseRepository.GetByNum(studyCourseid);
                courseRate = (courseStudy.cors_rate )/ (courseStudy.cors_degree);
                StudentGradesHed studentGradesHed = new StudentGradesHed();
                List<StudentGradesDet> _studentGradesDet = new List<StudentGradesDet>();
                studentGradesHed.stdGH_schId = schoolid;
                studentGradesHed.stdGH_teacherId = teacherid;
                studentGradesHed.stdGH_studyCourseId = studyCourseid;
                studentGradesHed.stdGH_Date = date;
                studentGradesHed.stdGH_term =(Term) term;
                studentGradesHed.stdGH_level = (Level)level;
                studentGradesHed.stdGH_stdRecord = (record)stdRecord;
                studentGradesHed.stdGH_examId = examid;
                var retHead =_studentGradesHedRepository.Insert(studentGradesHed);
                if (retHead != null)
                {
                    foreach (var item in StudentGradesDet)
                    {
                        item.stdGD_studentGradesHedId = studentGradesHed.stdGH_Id;
                        item.stdGD_rate = item.stdGD_degree * courseRate;
                        _studentGradesDet.Add(item);
                    }
                    var retDet = _studentGradesDetRepository.AddRange(_studentGradesDet);
                    if (retDet == true)
                    {
                        // return RedirectToAction("Index");
                        return Json("تم اضافة   درجات الطلاب", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        _studentGradesHedRepository.Delete(studentGradesHed.stdGH_Id);
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

        [ScreenPermissionFilter(screenId = 96)]
        public ActionResult Details(long id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ICollection<StudentGradesDet> studentGradesDet = _studentGradesDetRepository.GetAllStudentGradesDetWithStudent(id).ToList();
                if (studentGradesDet == null)
                {
                    return HttpNotFound();
                }
                return View(studentGradesDet);
            }
            catch (Exception)
            {


                return RedirectToAction("Index", "StudentExamesGrade");
            }

        }

        // GET: StudentExamesGrade/Create
        [System.Web.Http.HttpGet]
        [ScreenPermissionFilter(screenId = 97)]
        public ActionResult Create()
        {
            ViewBag.company = new SelectList(_companyRepository.GetAll(), "Id", "com_arName");
            ViewBag.complex = new SelectList(_complexRepository.GetAll(), "Id", "comp_arName");
            ViewBag.stdGH_teacherId = new SelectList(_teacherRepository.GetAll(), "Id", "tech_arName");

            ViewBag.stdGH_examId = new SelectList(_examRepository.GetAll(), "Id", "exm_arName");
            ViewBag.stdGH_schId = new SelectList(db.Schools, "Id", "sch_arName");
            ViewBag.stdGH_studyCourseId = new SelectList(db.StudyCourses, "Id", "cors_arName");
           return View();
        }
        [ScreenPermissionFilter(screenId = 98)]
        public ActionResult Edit(long studentGradesHedId, int studentId)
        {
            try
            {
                if (studentGradesHedId == null || studentId == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                StudentGradesDet studentGradesDet = _studentGradesDetRepository.GetStudentGradesDetWithStudentById(studentGradesHedId ,studentId);
                if (studentGradesDet == null)
                {
                    return HttpNotFound();
                }
                ViewBag.schd_scheduleHedId = new SelectList(_studentGradesHedRepository.GetAll(), "stdGH_Id", "stdGH_Id", studentGradesDet.stdGD_studentGradesHedId);
                return View(studentGradesDet);
            }
            catch (Exception)
            {


                return RedirectToAction("Index", "StudentExamesGrade");
            }

        }

        [System.Web.Http.HttpPost]
        public ActionResult Update([Bind(Include = "stdGD_rate,stdGD_degree,stdGD_studentId,stdGD_studentGradesHedId")] StudentGradesDet studentGradesDet)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var courseRate = 0.0;
                    var hedCourse = _studyCourseRepository.GetByNum(_studentGradesHedRepository.GetByNum(studentGradesDet.stdGD_studentGradesHedId).stdGH_studyCourseId);
                    courseRate = (hedCourse.cors_rate) / (hedCourse.cors_degree);
                    if (studentGradesDet.stdGD_degree < 0 || studentGradesDet.stdGD_degree>hedCourse.cors_degree)
                    {
                        ViewBag.schd_scheduleHedId = new SelectList(_studentGradesHedRepository.GetAll(), "stdGH_Id", "stdGH_Id", studentGradesDet.stdGD_studentGradesHedId);
                        return RedirectToAction("Index");
                    }
                    studentGradesDet.stdGD_rate = studentGradesDet.stdGD_degree * courseRate;
                    _studentGradesDetRepository.Update(studentGradesDet);
              
                    return RedirectToAction("Index");
                }
                ViewBag.schd_scheduleHedId = new SelectList(_studentGradesHedRepository.GetAll(), "stdGH_Id", "stdGH_Id", studentGradesDet.stdGD_studentGradesHedId);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {


                return RedirectToAction("Index", "Schedule");
            }


        }
        [ScreenPermissionFilter(screenId = 99)]
        public ActionResult Delete(long id)
        {
            try
            {
                var _studentGradesHed = _studentGradesHedRepository.GetAll().Where(h => h.stdGH_Id == id).FirstOrDefault();
                var retHead = _studentGradesHedRepository.Delete(_studentGradesHed.stdGH_Id);
           
                    return RedirectToAction("Index");
               
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
        }
        [ScreenPermissionFilter(screenId = 98)]
        public ActionResult Print(int id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var scheduleHed = _studentGradesHedRepository.GetStudentGradesHedWithStudentById(id);

                List<StudentExamesGradeViewModel> model = _studentGradesDetRepository.GetAllStudentGradesDetWithStudent(id).Select(o => new StudentExamesGradeViewModel
                {

                    stdGH_Date = scheduleHed.stdGH_Date,
                    ExamId = scheduleHed.stdGH_examId,
                    ExamName = scheduleHed.stdGH_Exams.exm_arName,
                    StudentId = o.stdGD_studentId,
                    StudentName = o.stdGD_Student.std_arName,
                    StudyCourseId =scheduleHed.stdGH_studyCourseId,
                    StudyCourseName = scheduleHed.stdGH_StudyCourse.cors_arName,
                    Degree = o.stdGD_degree,
                    Rate = o.stdGD_rate,
                    SchoolName=scheduleHed.stdGH_School.sch_arName,
                    TeacherName=scheduleHed.stdGH_Teacher.tech_arName,
                    StdRecord=scheduleHed.stdGH_stdRecord.ToString()
                }).ToList();

                ReportDocument rd = new ReportDocument();
                rd.Load(Path.Combine(Server.MapPath("~/Reports"), "StudentExamGrad.rpt"));
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
                return File(stream, "application/pdf", " جدول المدرس.pdf");
            }
            catch (Exception ex)
            {
                var xxx = ex.InnerException;
                return RedirectToAction("Index");
            }

        }
        [ScreenPermissionFilter(screenId = 98)]
        public ActionResult PrintByExamId(int department,int type,int term,int exam)
        {
            try
            {
        
                List<StudentExamesGradeViewModel> model = new List<StudentExamesGradeViewModel>();
                List<StudentExamesGradeViewModel> modelList = new List<StudentExamesGradeViewModel>();
                var scheduleHedList = _studentGradesHedRepository.GetAllStudentGradesHedWithStudent().Where(x=>x.stdGH_examId==exam && x.stdGH_term==(Term)term  && x.stdGH_School.sch_department==(SchoolDepartment)department && x.stdGH_School.sch_type==(SchoolType)type).ToList();
                if (scheduleHedList.Count>0)
                {
                    foreach (var scheduleHed in scheduleHedList)
                    {
                       model = _studentGradesDetRepository.GetAllStudentGradesDetWithStudent(scheduleHed.stdGH_Id).Select(o => new StudentExamesGradeViewModel
                        {

                            stdGH_Date = scheduleHed.stdGH_Date,
                            ExamId = scheduleHed.stdGH_examId,
                           ExamName = scheduleHed.stdGH_Exams.exm_arName,
                           StudentId = o.stdGD_studentId,
                           StudentName = o.stdGD_Student.std_arName,
                           StudyCourseId = scheduleHed.stdGH_studyCourseId,
                           StudyCourseName = scheduleHed.stdGH_StudyCourse.cors_arName,
                           Degree = o.stdGD_degree,
                            Rate = o.stdGD_rate,
                           SchoolName = scheduleHed.stdGH_School.sch_arName,
                           TeacherName = scheduleHed.stdGH_Teacher.tech_arName,
                           StdRecord = scheduleHed.stdGH_stdRecord.ToString(),
                           Level= scheduleHed.stdGH_level.ToString()
                       }).ToList();
                        modelList.AddRange(model);
                    }
                }
               
                ReportDocument rd = new ReportDocument();
                rd.Load(Path.Combine(Server.MapPath("~/Reports"), "StudentExamGradTotal.rpt"));
                DataSet ds = new DataSet();
                ds = new LogoDataSet.LogoDataSet();
                var comlogo = _companyRepository.GetAll().FirstOrDefault().com_image;
                var complogo = _complexRepository.GetAll().FirstOrDefault().comp_image;

                ds.Tables[0].Rows.Add(comlogo, complogo);
                rd.Database.Tables[0].SetDataSource(modelList);
                rd.Database.Tables[1].SetDataSource(ds.Tables[0]);

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", " جدول المدرس.pdf");
            }
            catch (Exception ex)
            {
                var xxx = ex.InnerException;
                return RedirectToAction("Index");
            }

        }

        // GET: StudentExamesGrade/Create
        [System.Web.Http.HttpGet]
        [ScreenPermissionFilter(screenId = 111)]
        public ActionResult GradingMonitoring()
        {
            ViewBag.company = new SelectList(_companyRepository.GetAll(), "Id", "com_arName");
            ViewBag.complex = new SelectList(_complexRepository.GetAll(), "Id", "comp_arName");
            ViewBag.stdGH_teacherId = new SelectList(_teacherRepository.GetAll(), "Id", "tech_arName");

            ViewBag.stdGH_examId = new SelectList(_examRepository.GetAll(), "Id", "exm_arName");
            ViewBag.stdGH_schId = new SelectList(db.Schools, "Id", "sch_arName");
            ViewBag.stdGH_studyCourseId = new SelectList(db.StudyCourses, "Id", "cors_arName");
            return View();
        }


        // GET: StudentExamesGrade/Create
        [System.Web.Http.HttpGet]
        public ActionResult AnalysisOfTheResults()
        {
            ViewBag.company = new SelectList(_companyRepository.GetAll(), "Id", "com_arName");
            ViewBag.complex = new SelectList(_complexRepository.GetAll(), "Id", "comp_arName");
            ViewBag.stdGH_teacherId = new SelectList(_teacherRepository.GetAll(), "Id", "tech_arName");

            ViewBag.stdGH_examId = new SelectList(_examRepository.GetAll(), "Id", "exm_arName");
            ViewBag.stdGH_schId = new SelectList(db.Schools, "Id", "sch_arName");
            ViewBag.stdGH_studyCourseId = new SelectList(db.StudyCourses, "Id", "cors_arName");
            return View();
        }
    }
}
