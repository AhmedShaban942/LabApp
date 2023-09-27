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
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Laboratories.Web.Controllers
{
    [PermClass]
    public class ScheduleController : Controller
    {
        private IRepository<Teacher> _teacherRepository;
        private ISchoolRepository _schoolRepository;
        private IScheduleHedRepository _scheduleHedRepository;
        private IRepository<ScheduleDet> _scheduleDetRepository;
        private IComplexRepository _complexRepository;
        private IRepository<Company> _companyRepository;
        public ScheduleController(IRepository<Teacher> teacherRepository, IComplexRepository complexRepository, IRepository<Company> companyRepository, ISchoolRepository schoolRepository, IScheduleHedRepository scheduleHedRepository, IRepository<ScheduleDet> scheduleDetRepository)
        {
            this._teacherRepository = teacherRepository;
            this._schoolRepository = schoolRepository;
            this._scheduleHedRepository = scheduleHedRepository;
            this._scheduleDetRepository = scheduleDetRepository;
            this._complexRepository = complexRepository;
            this._companyRepository = companyRepository;
        }


        //
        // GET: /Sales/
        [ScreenPermissionFilter(screenId = 52)]
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



                var model = from s in _scheduleHedRepository.GetAllScheduleHedWithTeacher() select s;

                //Search and match data, if search string is not null or empty
                if (!String.IsNullOrEmpty(searchString))
                {
                    model = model.Where(s => s.schd_teacher.tech_arName.Contains(searchString));
                }
                var ModelList = model;
                switch (sortOrder)
                {
                    case "name_desc":
                        ModelList = model.OrderBy(x => x.schd_teacher.tech_arName);
                        break;

                    default:
                        ModelList = model.OrderBy(x => x.schd_teacher.tech_arName);
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
        [ScreenPermissionFilter(screenId = 54)]
        public ActionResult Create()
        {
            try
            {
   if (Session["UserInfo"] != null)
            {
                    User oUser = (User)Session["UserInfo"];
                    ViewBag.Teachers = new SelectList(_teacherRepository.GetAll().Where(x => x.tech_schId == (int)Session["ScoolId"]), "Id", "tech_arName");
            return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Schedule");
            }
         
        }
        //

        public JsonResult SaveSchedule(int teatcher, int term, int lessonCount, [FromBody] List<ScheduleDet> ScheduleDet)
        {
            try
            {
                ScheduleHed scheduleHed = new ScheduleHed();
                List<ScheduleDet> _scheduleDets = new List<ScheduleDet>();
                scheduleHed.schd_teachId = teatcher;
                scheduleHed.schd_chapter = (Chapter)term;
                scheduleHed.schd_lessonCount = lessonCount;
                var retHead = _scheduleHedRepository.Insert(scheduleHed);
                if (retHead != null)
                {
                    foreach (var item in ScheduleDet)
                    {
                        item.schd_scheduleHedId = scheduleHed.schd_Id;
                        _scheduleDets.Add(item);
                    }
                    var retDet = _scheduleDetRepository.AddRange(_scheduleDets);
                    if (retDet == true)
                    {
                        // return RedirectToAction("Index");
                        return Json("تم اضافة  جدول المدرس", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        _scheduleHedRepository.Delete(scheduleHed.schd_Id);
                        return Json("0", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }
        }
        [ScreenPermissionFilter(screenId = 53)]
        public ActionResult Details(int id)
        {
            try
            {
       if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ICollection<ScheduleDet> scheduleDet = _scheduleDetRepository.GetAll().Where(x => x.schd_Id == id).ToList();
            if (scheduleDet == null)
            {
                return HttpNotFound();
            }
            return View(scheduleDet);
            }
            catch (Exception)
            {


                return RedirectToAction("Index", "Schedule");
            }
     
        }

        [ScreenPermissionFilter(screenId = 55)]
        public ActionResult Edit(int schd_Id, Days day)
        {
            try
            {
  if (schd_Id == null || day == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScheduleDet scheduleDet = _scheduleDetRepository.GetAll().Where(x => x.schd_Id == schd_Id && x.schd_day == day).FirstOrDefault();
            if (scheduleDet == null)
            {
                return HttpNotFound();
            }
            ViewBag.schd_scheduleHedId = new SelectList(_scheduleHedRepository.GetAll(), "schd_Id", "schd_Id", scheduleDet.schd_scheduleHedId);
            return View(scheduleDet);
            }
            catch (Exception)
            {


                return RedirectToAction("Index", "Schedule");
            }
          
        }

        [System.Web.Http.HttpPost]
        public ActionResult Update([Bind(Include = "schd_Id,schd_day,schd_lessonNum1,schd_subject1,schd_lessonNum2,schd_subject2,schd_lessonNum3,schd_subject3,schd_scheduleHedId")] ScheduleDet scheduleDet)
        {
            try
            {
      if (ModelState.IsValid)
            {
                if ((scheduleDet.schd_lessonNum1 > 0 && scheduleDet.schd_subject1 == 0) || (scheduleDet.schd_lessonNum2 > 0 && scheduleDet.schd_subject2 == 0) || (scheduleDet.schd_lessonNum3 > 0 && scheduleDet.schd_subject3 == 0))
                {
                    ViewBag.schd_scheduleHedId = new SelectList(_scheduleHedRepository.GetAll(), "schd_Id", "schd_Id", scheduleDet.schd_scheduleHedId);
                    return RedirectToAction("Index");
                }
                _scheduleDetRepository.Update(scheduleDet);
                UpdateesonCount(scheduleDet.schd_scheduleHedId);
                return RedirectToAction("Index");
            }
            ViewBag.schd_scheduleHedId = new SelectList(_scheduleHedRepository.GetAll(), "schd_Id", "schd_Id", scheduleDet.schd_scheduleHedId);
            return RedirectToAction("Index");
            }
            catch (Exception)
            {

              
                return RedirectToAction("Index", "Schedule");
            }

      
        }
        [ScreenPermissionFilter(screenId = 56)]
        public ActionResult Delete(int id)
        {
            try
            {
                var _scheduleHed = _scheduleHedRepository.GetAll().Where(h => h.schd_teachId == id).FirstOrDefault();
                var retHead = _scheduleHedRepository.Delete(_scheduleHed.schd_Id);
                if (retHead != null)
                {
                    return RedirectToAction("Index");


                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult Print(int id)
        {
            try
            {
       if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var scheduleHed = _scheduleHedRepository.GetScheduleHedWithTeacherById(id);

            List<ScheduleViewModel> model = _scheduleDetRepository.GetAll().Where(x => x.schd_Id == id).Select(o => new ScheduleViewModel
            {

                schd_scool = scheduleHed.schd_teacher.tech_School.sch_arName,
                schd_teacher = scheduleHed.schd_teacher.tech_arName,
                schd_laboratoryRecordName = _schoolRepository.GetByNum(scheduleHed.schd_teacher.tech_School.Id).sch_laboratoryRecordName  ,
                schd_department = _schoolRepository.GetByNum(scheduleHed.schd_teacher.tech_School.Id).sch_department.ToString()  ,
                schd_chapter =scheduleHed.schd_chapter.ToString()  ,
                schd_day =o.schd_day.ToString(),

                schd_lessonNum1 = o.schd_lessonNum1>0 ? o.schd_lessonNum1.ToString() : " "   ,
                schd_subject1 =o.schd_subject1 > 0 ? o.schd_subject1.ToString() : " ",

                schd_lessonNum2 =o.schd_lessonNum2 > 0 ? o.schd_lessonNum2.ToString() : " ",
                schd_subject2 = o.schd_subject2 > 0 ? o.schd_subject2.ToString() : " ",
                schd_lessonNum3 = o.schd_lessonNum3 > 0 ? o.schd_lessonNum3.ToString() : " ",
                schd_subject3 = o.schd_subject3 > 0 ? o.schd_subject3.ToString() : " ",
                schd_lessonNum4 = o.schd_lessonNum4 > 0 ? o.schd_lessonNum4.ToString() : " ",
                schd_subject4 = o.schd_subject4 > 0 ? o.schd_subject4.ToString() : " ",
                schd_lessonNum5 = o.schd_lessonNum5 > 0 ? o.schd_lessonNum5.ToString() : " ",
                schd_subject5 = o.schd_subject5 > 0 ? o.schd_subject5.ToString() : " ",
                schd_lessonCount =(int)scheduleHed.schd_lessonCount  ,


                schd_lessonCountActual =0  ,

                schd_weekNumber =0  ,

                schd_monthNumberr =0  ,

                mov_note =""  ,
                mov_percent =0
    }).ToList();

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "TeacherSchedule.rpt"));
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
               var xxx= ex.InnerException;
                return RedirectToAction("Index");
            }
           
        }

        public ActionResult PrintAll()
        {
            try
            {
             


                List<ScheduleViewModel> model = _scheduleDetRepository.GetAll().Select(o => new ScheduleViewModel
                {

                    schd_scool = _scheduleHedRepository.GetAllScheduleHedWithTeacher().Where(x=>x.schd_Id==o.schd_scheduleHedId).FirstOrDefault().schd_teacher.tech_School.sch_arName,
                    schd_teacher = _scheduleHedRepository.GetAllScheduleHedWithTeacher().Where(x => x.schd_Id == o.schd_scheduleHedId).FirstOrDefault().schd_teacher.tech_arName,
                    schd_laboratoryRecordName = _scheduleHedRepository.GetAllScheduleHedWithTeacher().Where(x => x.schd_Id == o.schd_scheduleHedId).FirstOrDefault().schd_teacher.tech_School.sch_laboratoryRecordName,
                    schd_department = _scheduleHedRepository.GetAllScheduleHedWithTeacher().Where(x => x.schd_Id == o.schd_scheduleHedId).FirstOrDefault().schd_teacher.tech_School.sch_department.ToString(),
                    schd_chapter = _scheduleHedRepository.GetAllScheduleHedWithTeacher().Where(x => x.schd_Id == o.schd_scheduleHedId).FirstOrDefault().schd_chapter.ToString(),
                    schd_day = o.schd_day.ToString(),

                    schd_lessonNum1 = o.schd_lessonNum1 > 0 ? o.schd_lessonNum1.ToString() : " ",
                    schd_subject1 = o.schd_subject1 > 0 ? o.schd_subject1.ToString() : " ",

                    schd_lessonNum2 = o.schd_lessonNum2 > 0 ? o.schd_lessonNum2.ToString() : " ",
                    schd_subject2 = o.schd_subject2 > 0 ? o.schd_subject2.ToString() : " ",
                    schd_lessonNum3 = o.schd_lessonNum3 > 0 ? o.schd_lessonNum3.ToString() : " ",
                    schd_subject3 = o.schd_subject3 > 0 ? o.schd_subject3.ToString() : " ",
                    schd_lessonNum4 = o.schd_lessonNum4 > 0 ? o.schd_lessonNum4.ToString() : " ",
                    schd_subject4 = o.schd_subject4 > 0 ? o.schd_subject4.ToString() : " ",
                    schd_lessonNum5 = o.schd_lessonNum5 > 0 ? o.schd_lessonNum5.ToString() : " ",
                    schd_subject5 = o.schd_subject5 > 0 ? o.schd_subject5.ToString() : " ",
                    schd_lessonCount = (int)_scheduleHedRepository.GetAllScheduleHedWithTeacher().Where(x => x.schd_Id == o.schd_scheduleHedId).FirstOrDefault().schd_lessonCount,


                    schd_lessonCountActual = 0,

                    schd_weekNumber = 0,

                    schd_monthNumberr = 0,

                    mov_note = "",
                    mov_percent = 0
                }).ToList();

                ReportDocument rd = new ReportDocument();
                rd.Load(Path.Combine(Server.MapPath("~/Reports"), "AllTeacherSchedule.rpt"));
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
        public void UpdateesonCount(int schh_id)
        {
            try
            {
                int leson1Count = _scheduleDetRepository.GetAll().Where(x => x.schd_lessonNum1 != 0 && x.schd_scheduleHedId == schh_id).Count();
                int leson2Count = _scheduleDetRepository.GetAll().Where(x => x.schd_lessonNum2 != 0 && x.schd_scheduleHedId == schh_id).Count();
                int leson3Count = _scheduleDetRepository.GetAll().Where(x => x.schd_lessonNum3 != 0 && x.schd_scheduleHedId == schh_id).Count();

                ScheduleHed _scheduleHed = _scheduleHedRepository.GetByNum(schh_id);
                _scheduleHed.schd_lessonCount = leson1Count + leson2Count + leson3Count;
                _scheduleHedRepository.Update(_scheduleHed);

            }
            catch (Exception)
            {

                throw;
            }
     
      
        }
    }
}