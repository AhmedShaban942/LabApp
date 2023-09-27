using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using Laboratories.Application.Contracts;
using Laboratories.Domain;
using Laboratories.Persistence;
using Laboratories.Web.Models;
using Laboratories.Web.ViewModel;

namespace Laboratories.Web.Controllers
{
    [PermClass]
    public class SchedulReportController : Controller
    {
        private IItemRepository _itemRepository;
        private IComplexRepository _complexRepository;
        private IRepository<Company> _companyRepository;
        private ISchoolRepository _schoolRepository;
        private IRepository<Teacher> _teacherRepository;
        private IScheduleHedRepository _scheduleHedRepository;
        private IRepository<ScheduleDet> _scheduleDetRepository;
        private ITeacherMovmentRepository _teacherMovmentRepository;
        public SchedulReportController(IRepository<Teacher> teacherRepository, IItemRepository itemRepository, IComplexRepository complexRepository, IRepository<Company> companyRepository, ISchoolRepository schoolRepository, IScheduleHedRepository scheduleHedRepository, IRepository<ScheduleDet> scheduleDetRepository, ITeacherMovmentRepository teacherMovmentRepository)
        {
            this._teacherRepository = teacherRepository;
            this._itemRepository = itemRepository;
            this._complexRepository = complexRepository;
            this._companyRepository = companyRepository;
            this._schoolRepository = schoolRepository;
            this._scheduleHedRepository = scheduleHedRepository;
            this._scheduleDetRepository = scheduleDetRepository;
            this._teacherMovmentRepository = teacherMovmentRepository;

        }
        #region Custom Report
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

        public JsonResult GetTeacher(int schoolid)
        {
            try
            {
                var schoolList = _teacherRepository.GetAll().Where(x => x.tech_schId == schoolid);
                return Json(schoolList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json(null);
            }


        }

        public ActionResult DownloadCustomData(string reportType, int companyId, int? complexId, int? schoolId,int? month=0 ,int? week=0)
        {
            try
            {
           
                if ((User)Session["UserInfo"] != null)
                {
                    User oUser = (User)Session["UserInfo"];
                    var items = _teacherMovmentRepository.GetAllTeacherMovmentWithTeacher();
                    //if (complexId == null && oUser.usr_role == Domain.User.Role.مدير_نظام)
                    //{
                    //    items = _teacherMovmentRepository.GetAllTeacherMovmentWithTeacher();
                    //}
                    //else if (complexId != null && schoolId == null && (oUser.usr_role == Domain.User.Role.مدير_نظام || oUser.usr_role == Domain.User.Role.مشرف_مشرفة))

                    //{
                    //    items = _teacherMovmentRepository.GetAllTeacherMovmentWithTeacher().Where(x => x.mov_Teacher.tech_School.sch_comp_id == complexId).ToList();
                    //}
                    //else if (complexId != null && schoolId != null)
                    //{
                        items = _teacherMovmentRepository.GetAllTeacherMovmentWithTeacher().Where(x => x.mov_Teacher.tech_schId == schoolId).ToList();
                   // }
              
                    List<ScheduleViewModel> model = new List<ScheduleViewModel>();
                    if (reportType == "TeacherScheduleYear")
                    {
                        model = items.Select(o => new ScheduleViewModel
                        {
                            schd_scool = _schoolRepository.GetByNum(o.mov_Teacher.tech_schId).sch_arName,
                            schd_teacher = o.mov_Teacher.tech_arName,
                            schd_laboratoryRecordName = _schoolRepository.GetByNum(o.mov_Teacher.tech_schId).sch_laboratoryRecordName,
                            schd_department = _schoolRepository.GetByNum(o.mov_Teacher.tech_schId).sch_department.ToString(),
                            schd_chapter = " ",
                            
                            schd_lessonCount =(int) o.mov_lessonCount,


                            schd_lessonCountActual =(int) o.mov_lessonCountActual,

                            schd_weekNumber = o.mov_weekNumber,

                            schd_monthNumberr =o.mov_monthNumberr,

                            mov_note = o.mov_note,
                            mov_percent =(((int)o.mov_lessonCountActual/(int)o.mov_lessonCount)*100)
                        }).ToList();

                    }
                    else if (reportType == "TeacherScheduleMonth")
                    {
                        model = items.Where(x=>x.mov_monthNumberr==month).Select(o => new ScheduleViewModel
                        {
                            schd_scool = _schoolRepository.GetByNum(o.mov_Teacher.tech_schId).sch_arName,
                            schd_teacher = o.mov_Teacher.tech_arName,
                            schd_laboratoryRecordName = _schoolRepository.GetByNum(o.mov_Teacher.tech_schId).sch_laboratoryRecordName,
                            schd_department = _schoolRepository.GetByNum(o.mov_Teacher.tech_schId).sch_department.ToString(),
                            schd_chapter = " ",

                            schd_lessonCount = (int)o.mov_lessonCount,


                            schd_lessonCountActual = (int)o.mov_lessonCountActual,

                            schd_weekNumber = o.mov_weekNumber,

                            schd_monthNumberr = o.mov_monthNumberr,

                            mov_note = o.mov_note,
                            mov_percent = (((int)o.mov_lessonCountActual / (int)o.mov_lessonCount) * 100)
                        }).ToList();

                    }
                    else if (reportType == "TeacherScheduleWeek")
                    {
                        model = items.Where(x => x.mov_monthNumberr == month && x.mov_weekNumber==week).Select(o => new ScheduleViewModel
                        {
                            schd_scool = _schoolRepository.GetByNum(o.mov_Teacher.tech_schId).sch_arName,
                            schd_teacher = o.mov_Teacher.tech_arName,
                            schd_laboratoryRecordName = _schoolRepository.GetByNum(o.mov_Teacher.tech_schId).sch_laboratoryRecordName,
                            schd_department = _schoolRepository.GetByNum(o.mov_Teacher.tech_schId).sch_department.ToString(),
                            schd_chapter = " ",

                            schd_lessonCount = (int)o.mov_lessonCount,


                            schd_lessonCountActual = (int)o.mov_lessonCountActual,

                            schd_weekNumber = o.mov_weekNumber,

                            schd_monthNumberr = o.mov_monthNumberr,

                            mov_note = o.mov_note,
                            mov_percent = (((int)o.mov_lessonCountActual / (int)o.mov_lessonCount) * 100)
                        }).ToList();

                    }
                    if (model.Count>0)
                    {
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
                        return File(stream, "application/pdf", "تقرير تفعيل المختبر.pdf");
                    }
                    else
                    {
                        return RedirectToAction("CustomReport", "SchedulReport");
                    }
        
                  
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
    }
}
