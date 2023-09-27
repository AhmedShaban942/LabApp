using Laboratories.Application.Contracts;
using Laboratories.Domain;
using Laboratories.Persistence;
using Laboratories.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Laboratories.Web.Controllers
{
    [PermClass]
    public class HomeController : Controller
    {
        private IComplexRepository _complexRepository;
        private IRepository<Company> _companyRepository;
        private ISchoolRepository _schoolRepository;

        private IRepository<Item> _itemRepository;
        private IUserSchoolsRepository _userSchoolsRepository;
        public HomeController(IRepository<Item> itemRepository, IComplexRepository complexRepository, IRepository<Company> companyRepository, ISchoolRepository schoolRepository, IUserSchoolsRepository userSchoolsRepository)
        {
            this._itemRepository = itemRepository;
            this._complexRepository = complexRepository;
            this._companyRepository = companyRepository;
            this._schoolRepository = schoolRepository;
            this._userSchoolsRepository = userSchoolsRepository;
        }
        public ActionResult Test()
        {
            var items=_itemRepository.GetAll().Take(50).ToList(); 
            return View(items);
        }

        //[ScreenPermissionFilter(screenId = 1)]
        public ActionResult Index()
        {
            try
            {
                if (Session["UserInfo"] != null)
                {
                    User oUser = (User)Session["UserInfo"];
                    //if (_companyRepository.GetAll().Count > 0)
                    //{
                    //    if (_companyRepository.GetAll().FirstOrDefault().com_image != null)
                    //    {

                    //    }
                    //    ViewBag.companyimage = _companyRepository.GetAll().FirstOrDefault().com_image;

                    //}
                    //if (_complexRepository.GetAll().Count > 0)
                    //{
                    //    if ((int)Session["ScoolId"] != null)
                    //    {
                    //        ViewBag.compleximage = _complexRepository.GetAll().Where(x => x.Id == _schoolRepository.GetByNum((int)Session["ScoolId"]).sch_comp_id).FirstOrDefault().comp_image;

                    //    }


                    //}
                    //if (_schoolRepository.GetAll().Count > 0)
                    //{
                    //    if ((int)Session["ScoolId"] != null)
                    //    {
                    //        ViewBag.schoolimage = _schoolRepository.GetAll().Where(x => x.Id == (int)Session["ScoolId"]).FirstOrDefault().sch_image;

                    //    }

                    //}
                    if (_itemRepository.GetAll().Count > 0)
                    {

                        int schoolId = 0;
                        schoolId = (int)Session["ScoolId"];

                        if (schoolId != null)
                        {
                            double item_count = _itemRepository.MultiSearch(x => x.itm_schId == schoolId&& x.itm_type !=Domain.Type.اثاث_مختبر).Count();
                            ViewBag.presentItemValid = ((_itemRepository.MultiSearch(x => x.itm_schId == schoolId &&x.itm_type!=Domain.Type.اثاث_مختبر &&x.itm_isExisting==ExsistState.موجود && x.itm_ValidState==ValidState.صالح).Count())/ item_count)*100;
                            ViewBag.presentItemUnValid = ((_itemRepository.MultiSearch(x => x.itm_schId == schoolId && x.itm_type != Domain.Type.اثاث_مختبر && x.itm_isExisting == ExsistState.موجود && x.itm_ValidState == ValidState.غير_صالح).Count()) / item_count) * 100;

                            ViewBag.wantedItem = ((_itemRepository.MultiSearch(x => x.itm_schId == schoolId && x.itm_type != Domain.Type.اثاث_مختبر && x.itm_isExisting == ExsistState.غير_موجود && x.itm_availableMethod == AvailableMethod.المستودع).Count()) / item_count) * 100;
                            ViewBag.canAvalableItem = ((_itemRepository.MultiSearch(x => x.itm_schId == schoolId && x.itm_type != Domain.Type.اثاث_مختبر && x.itm_isExisting == ExsistState.غير_موجود && x.itm_availableMethod == AvailableMethod.المدرسة).Count()) / item_count) * 100;



                            //lab
                            double itemLab_count = _itemRepository.MultiSearch(x => x.itm_schId == schoolId && x.itm_type == Domain.Type.اثاث_مختبر).Count();
                            ViewBag.presentItemLabValid = ((_itemRepository.MultiSearch(x => x.itm_schId == schoolId && x.itm_type == Domain.Type.اثاث_مختبر && x.itm_isExisting == ExsistState.موجود && x.itm_ValidState == ValidState.صالح).Count()) / item_count) * 100;

                            ViewBag.wantedItemLab = ((_itemRepository.MultiSearch(x => x.itm_schId == schoolId && x.itm_type == Domain.Type.اثاث_مختبر && x.itm_isExisting == ExsistState.غير_موجود).Count()) / item_count) * 100;
                        
                        }
                        else
                        {
                            ViewBag.presentItemValid = 0;
                            ViewBag.presentItemUnValid = 0;
                            ViewBag.wantedItem = 0;
                            ViewBag.canAvalableItem = 0;


                            //lab

                            ViewBag.presentItemLabValid = 0;
                            ViewBag.wantedItemLab = 0;
                        }

                    }

                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception)
            {

                return RedirectToAction("Login", "Account");
            }
          


        }

        public ActionResult SelectSchool()
        {

            User oUser = (User)Session["UserInfo"];
            IEnumerable<UserSchools> userSchools= _userSchoolsRepository.GetAllUserWithSchool(oUser.Id);
            ViewBag.UserSchoolsId = new SelectList(userSchools, " School.Id", " School.sch_arName");
            if (userSchools.Count() != 0)
            {
                return View();
            }
            return View("Index");
        }
        [HttpPost]
        public JsonResult SaveSession(int schoolId)
        {
            try
            {
                if (schoolId !=null)
                {
                    Session["ScoolId"] = schoolId;

                    int department =(int) _schoolRepository.GetByNum(schoolId).sch_department;
                    Session["Department"] = department;
                    return Json("1", JsonRequestBehavior.AllowGet);

                }
                return Json("0", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json("0", JsonRequestBehavior.AllowGet);
            }
          

        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}