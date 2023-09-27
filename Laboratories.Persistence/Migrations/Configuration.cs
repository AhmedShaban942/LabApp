namespace Laboratories.Persistence.Migrations
{
    using Laboratories.Domain;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Laboratories.Persistence.LaboratoryDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Laboratories.Persistence.LaboratoryDbContext context)
        {

            IList<Screen> defaultScreen = new List<Screen>();
            defaultScreen.Add(new Screen() { screen_id = 1, screen_name = "الرئيسية " });


            defaultScreen.Add(new Screen() { screen_id = 2, screen_name = "عرض المستخدمين " });
            defaultScreen.Add(new Screen() { screen_id = 3, screen_name = "عرض بيانات مستخدم " });

            defaultScreen.Add(new Screen() { screen_id = 4, screen_name = " اضافة مستخدم " });

            defaultScreen.Add(new Screen() { screen_id = 5, screen_name = "تعديل مستخدم " });

            defaultScreen.Add(new Screen() { screen_id = 6, screen_name = "حذف مستخدم " });


            defaultScreen.Add(new Screen() { screen_id = 7, screen_name = "عرض الشركات " });

            defaultScreen.Add(new Screen() { screen_id = 8, screen_name = "عرض  بيانات شركة " });

            defaultScreen.Add(new Screen() { screen_id = 9, screen_name = " اضافة شركة " });

            defaultScreen.Add(new Screen() { screen_id = 10, screen_name = "تعديل شركة " });

            defaultScreen.Add(new Screen() { screen_id = 11, screen_name = "حذف شركة " });


            defaultScreen.Add(new Screen() { screen_id = 12, screen_name = "عرض المجمعات " });

            defaultScreen.Add(new Screen() { screen_id = 13, screen_name = "عرض بيانات مجمع " });

            defaultScreen.Add(new Screen() { screen_id = 14, screen_name = " اضافة مجمع " });

            defaultScreen.Add(new Screen() { screen_id = 15, screen_name = "تعديل مجمع " });

            defaultScreen.Add(new Screen() { screen_id = 16, screen_name = "حذف مجمع " });


            defaultScreen.Add(new Screen() { screen_id = 17, screen_name = "عرض المدارس " });

            defaultScreen.Add(new Screen() { screen_id = 18, screen_name = "عرض بيانات مدرسة " });

            defaultScreen.Add(new Screen() { screen_id = 19, screen_name = " اضافة مدرسة " });

            defaultScreen.Add(new Screen() { screen_id = 20, screen_name = "تعديل مدرسة " });

            defaultScreen.Add(new Screen() { screen_id = 21, screen_name = "حذف مدرسة " });

            defaultScreen.Add(new Screen() { screen_id = 22, screen_name = "عرض المعلمين " });

            defaultScreen.Add(new Screen() { screen_id = 23, screen_name = "عرض بيانات معلم " });

            defaultScreen.Add(new Screen() { screen_id = 24, screen_name = " اضافة معلم " });

            defaultScreen.Add(new Screen() { screen_id = 25, screen_name = "تعديل معلم " });

            defaultScreen.Add(new Screen() { screen_id = 26, screen_name = "حذف معلم " });
            defaultScreen.Add(new Screen() { screen_id = 27, screen_name = "عرض الوحدات " });

            defaultScreen.Add(new Screen() { screen_id = 28, screen_name = "عرض بيانات وحدة " });

            defaultScreen.Add(new Screen() { screen_id = 29, screen_name = "اضافة وحدة " });

            defaultScreen.Add(new Screen() { screen_id = 30, screen_name = "تعديل وحدة " });

            defaultScreen.Add(new Screen() { screen_id = 31, screen_name = "حذف وحدة " });
            defaultScreen.Add(new Screen() { screen_id = 32, screen_name = "عرض المقررات " });

            defaultScreen.Add(new Screen() { screen_id = 33, screen_name = "عرض بيانات مقرر " });

            defaultScreen.Add(new Screen() { screen_id = 34, screen_name = "اضافة مقرر " });

            defaultScreen.Add(new Screen() { screen_id = 35, screen_name = "تعديل مقرر " });

            defaultScreen.Add(new Screen() { screen_id = 36, screen_name = "حذف مقرر " });

            defaultScreen.Add(new Screen() { screen_id = 37, screen_name = "عرض الاصناف " });

            defaultScreen.Add(new Screen() { screen_id = 38, screen_name = "عرض بيانات صنف " });

            defaultScreen.Add(new Screen() { screen_id = 39, screen_name = "اضافة صنف " });

            defaultScreen.Add(new Screen() { screen_id = 40, screen_name = "تعديل صنف " });

            defaultScreen.Add(new Screen() { screen_id = 41, screen_name = "حذف صنف " });

            defaultScreen.Add(new Screen() { screen_id = 42, screen_name = "عرض اثاث المختبر " });

            defaultScreen.Add(new Screen() { screen_id = 43, screen_name = "عرض بيانات اثاث " });

            defaultScreen.Add(new Screen() { screen_id = 44, screen_name = "اضافة اثاث " });

            defaultScreen.Add(new Screen() { screen_id = 45, screen_name = "تعديل اثاث " });

            defaultScreen.Add(new Screen() { screen_id = 46, screen_name = "حذف اثاث " });

            defaultScreen.Add(new Screen() { screen_id = 47, screen_name = "عرض سجل تنفيذ التجارب " });

            defaultScreen.Add(new Screen() { screen_id = 48, screen_name = "عرض بيانات سجل " });

            defaultScreen.Add(new Screen() { screen_id = 49, screen_name = "اضافة سجل " });

            defaultScreen.Add(new Screen() { screen_id = 50, screen_name = "تعديل سجل " });

            defaultScreen.Add(new Screen() { screen_id = 51, screen_name = "حذف سجل " });

            defaultScreen.Add(new Screen() { screen_id = 52, screen_name = "عرض جدول حصص المختبر" });

            defaultScreen.Add(new Screen() { screen_id = 53, screen_name = "عرض بيانات جدول " });

            defaultScreen.Add(new Screen() { screen_id = 54, screen_name = "اضافة جدول " });

            defaultScreen.Add(new Screen() { screen_id = 55, screen_name = "تعديل جدول " });

            defaultScreen.Add(new Screen() { screen_id = 56, screen_name = "حذف جدول " });


            defaultScreen.Add(new Screen() { screen_id = 57, screen_name = "عرض عدد الحصص الفعلى" });

            defaultScreen.Add(new Screen() { screen_id = 58, screen_name = "عرض بيانات الحصص الفعلية " });

            defaultScreen.Add(new Screen() { screen_id = 59, screen_name = "اضافة الحصص الفعلية " });

            defaultScreen.Add(new Screen() { screen_id = 60, screen_name = "تعديل الحصص الفعلية " });

            defaultScreen.Add(new Screen() { screen_id = 61, screen_name = "حذف الحصص الفعلية " });

            defaultScreen.Add(new Screen() { screen_id = 62, screen_name = "نقل عهدة الاصناف" });


            defaultScreen.Add(new Screen() { screen_id = 63, screen_name = "تقرير الاصناف " });
            defaultScreen.Add(new Screen() { screen_id = 64, screen_name = "تقرير جرد المختبر " });
            defaultScreen.Add(new Screen() { screen_id = 65, screen_name = "تقرير الاصناف الغير موجودة " });
            defaultScreen.Add(new Screen() { screen_id = 66, screen_name = "تقرير الاحتياج لمواد توفر من المستودع " });
            defaultScreen.Add(new Screen() { screen_id = 67, screen_name = "تقرير الاحتياج لمواد توفر من المدرسة " });
            defaultScreen.Add(new Screen() { screen_id = 68, screen_name = "تقرير الاحتياج التكميلى " });
            defaultScreen.Add(new Screen() { screen_id = 69, screen_name = "تقرير الاصناف الزائدة " });
            defaultScreen.Add(new Screen() { screen_id = 70, screen_name = "التقارير العامة للاصناف " });
            defaultScreen.Add(new Screen() { screen_id = 71, screen_name = "تقرير الاصتاف لاثاث المختبر " });
            defaultScreen.Add(new Screen() { screen_id = 72, screen_name = "تقرير جرد اثاث المختبر" });
            defaultScreen.Add(new Screen() { screen_id = 73, screen_name = "تقرير الاصناف الغير موجودة " });
            defaultScreen.Add(new Screen() { screen_id = 74, screen_name = "تقرير الاحتياج التكميلى " });
            defaultScreen.Add(new Screen() { screen_id = 75, screen_name = " التقارير العامة للاثاث" });
            defaultScreen.Add(new Screen() { screen_id = 76, screen_name = " احصائية تفعيل المختبر" });
            defaultScreen.Add(new Screen() { screen_id = 77, screen_name = "تقارير التجارب المنفذة للمعلم " });
            defaultScreen.Add(new Screen() { screen_id = 78, screen_name = "تقارير التجارب المنفذة للمقرر " });
            defaultScreen.Add(new Screen() { screen_id = 79, screen_name = "تقارير التجارب المنفذة للمرحلة " });
            defaultScreen.Add(new Screen() { screen_id = 80, screen_name = "تقارير التجارب  الغير منفذة للكل " });
            defaultScreen.Add(new Screen() { screen_id = 81, screen_name = "تقارير التجارب  الغير منفذة للمعلم " });
            defaultScreen.Add(new Screen() { screen_id = 82, screen_name = "تقارير التجارب  الغير منفذة للمقرر " });
            defaultScreen.Add(new Screen() { screen_id = 83, screen_name = "تقارير التجارب  الغير منفذة للمرحلة " });
            defaultScreen.Add(new Screen() { screen_id = 84, screen_name = "تقارير التجارب  الغير منفذة للكل " });


            defaultScreen.Add(new Screen() { screen_id = 85, screen_name = "عرض الصلاحيات " });


            defaultScreen.Add(new Screen() { screen_id = 86, screen_name = " اضافة صلاحية " });

            defaultScreen.Add(new Screen() { screen_id = 87, screen_name = "تعديل صلاحية " });

            defaultScreen.Add(new Screen() { screen_id = 88, screen_name = "حذف صلاحية " });


            defaultScreen.Add(new Screen() { screen_id = 89, screen_name = "عرض اسماء الاختبارات " });

            defaultScreen.Add(new Screen() { screen_id = 90, screen_name = "عرض بيانات اختبار " });

            defaultScreen.Add(new Screen() { screen_id = 91, screen_name = " اضافة اختبار " });

            defaultScreen.Add(new Screen() { screen_id = 92, screen_name = "تعديل اختبار " });

            defaultScreen.Add(new Screen() { screen_id = 93, screen_name = "حذف اختبار " });

            defaultScreen.Add(new Screen() { screen_id = 94, screen_name = " صلاحيات الدخول عل الشاشات " });


            defaultScreen.Add(new Screen() { screen_id = 95, screen_name = "عرض  الاختبارات " });

            defaultScreen.Add(new Screen() { screen_id = 96, screen_name = "عرض تفاصيل  اختبار " });

            defaultScreen.Add(new Screen() { screen_id = 97, screen_name = " اضافة درجات اختبار " });

            defaultScreen.Add(new Screen() { screen_id = 98, screen_name = "تعديل درجات اختبار " });

            defaultScreen.Add(new Screen() { screen_id = 99, screen_name = "حذف درجات اختبار " });
            defaultScreen.Add(new Screen() { screen_id = 100, screen_name = "طباعة درجات اختبار " });



            defaultScreen.Add(new Screen() { screen_id = 101, screen_name = "عرض  الطلاب " });

            defaultScreen.Add(new Screen() { screen_id = 102, screen_name = "عرض تفاصيل  طالب " });

            defaultScreen.Add(new Screen() { screen_id = 103, screen_name = " اضافة طالب " });

            defaultScreen.Add(new Screen() { screen_id = 104, screen_name = "تعديل بيانات طالب  " });

            defaultScreen.Add(new Screen() { screen_id = 105, screen_name = "حذف  طالب " });



            defaultScreen.Add(new Screen() { screen_id = 106, screen_name = "عرض  المقررات الدراسية " });

            defaultScreen.Add(new Screen() { screen_id = 107, screen_name = "عرض تفاصيل  مقرر دراسى " });

            defaultScreen.Add(new Screen() { screen_id = 108, screen_name = " اضافة مقرر دراسى " });

            defaultScreen.Add(new Screen() { screen_id = 109, screen_name = "تعديل بيانات مقرر دراسى  " });

            defaultScreen.Add(new Screen() { screen_id = 110, screen_name = "حذف  مقرر دراسى " });


            defaultScreen.Add(new Screen() { screen_id = 111, screen_name = "رصد درجات الطلاب" });

            context.Screens.AddRange(defaultScreen);


            UserRole userRole = new UserRole();
            userRole.role_name = "مدير النظام";
            userRole.role_enName = "System Admin";
            context.userRoles.Add(userRole);

            User defaultUser = new User();
            defaultUser.usr_arName = "مدير النظام";
            defaultUser.usr_enName = "System Admin";
            defaultUser.usr_num = 1;
            defaultUser.usr_pass = "159";
            defaultUser.usr_roleId = userRole.role_id;
            context.Users.Add(defaultUser);


            IList<ScreenRole> screenRoles = new List<ScreenRole>();
            for (int i = 1; i < 112; i++)
            {
                screenRoles.Add(new ScreenRole() { role_id = userRole.role_id, screen_id = i });
            }

            context.ScreenRoles.AddRange(screenRoles);
            context.SaveChanges();





            //    IList<Unit> defaultUnit = new List<Unit>();
            //    defaultUnit.Add(new Unit() { unt_arName = "أنبوب", unt_enName = " " });
            //    defaultUnit.Add(new Unit() { unt_arName = "اصيص", unt_enName = " " });
            //    defaultUnit.Add(new Unit() { unt_arName = "العدد", unt_enName = " " });
            //    defaultUnit.Add(new Unit() { unt_arName = "باكت", unt_enName = " " });
            //    defaultUnit.Add(new Unit() { unt_arName = "جرام", unt_enName = " " });
            //    defaultUnit.Add(new Unit() { unt_arName = "جهاز", unt_enName = " " });
            //    defaultUnit.Add(new Unit() { unt_arName = "درزن", unt_enName = " " });
            //    defaultUnit.Add(new Unit() { unt_arName = "دفتر", unt_enName = " " });
            //    defaultUnit.Add(new Unit() { unt_arName = "شريحة", unt_enName = " " });
            //    defaultUnit.Add(new Unit() { unt_arName = "صورة", unt_enName = " " });
            //    defaultUnit.Add(new Unit() { unt_arName = "عبوة", unt_enName = " " });
            //    defaultUnit.Add(new Unit() { unt_arName = "علبة", unt_enName = " " });
            //    defaultUnit.Add(new Unit() { unt_arName = "عينة", unt_enName = " " });
            //    defaultUnit.Add(new Unit() { unt_arName = "قطعة", unt_enName = " " });
            //    defaultUnit.Add(new Unit() { unt_arName = "كأس", unt_enName = " " });
            //    defaultUnit.Add(new Unit() { unt_arName = "كجم", unt_enName = " " });
            //    defaultUnit.Add(new Unit() { unt_arName = "كرة", unt_enName = " " });
            //    defaultUnit.Add(new Unit() { unt_arName = "نموذج", unt_enName = " " });
            //    defaultUnit.Add(new Unit() { unt_arName = "لفة", unt_enName = " " });
            //    defaultUnit.Add(new Unit() { unt_arName = "متر", unt_enName = " " });
            //    defaultUnit.Add(new Unit() { unt_arName = "مكعب", unt_enName = " " });
            //    defaultUnit.Add(new Unit() { unt_arName = "مل", unt_enName = " " });
            //    defaultUnit.Add(new Unit() { unt_arName = "أنبوب", unt_enName = " " });
            //    defaultUnit.Add(new Unit() { unt_arName = "اصيص", unt_enName = " " });
            //    defaultUnit.Add(new Unit() { unt_arName = "العدد", unt_enName = " " });
            //    defaultUnit.Add(new Unit() { unt_arName = "باكت", unt_enName = " " });
            //    defaultUnit.Add(new Unit() { unt_arName = "جرام", unt_enName = " " });
            //    defaultUnit.Add(new Unit() { unt_arName = "جهاز", unt_enName = " " });
            //    defaultUnit.Add(new Unit() { unt_arName = "حذاء", unt_enName = " " });
            //    defaultUnit.Add(new Unit() { unt_arName = "حوض", unt_enName = " " });
            //    defaultUnit.Add(new Unit() { unt_arName = "درزن", unt_enName = " " });
            //    defaultUnit.Add(new Unit() { unt_arName = "دفتر", unt_enName = " " });
            //    defaultUnit.Add(new Unit() { unt_arName = "سلك", unt_enName = " " });
            //    defaultUnit.Add(new Unit() { unt_arName = "شريحة", unt_enName = " " });
            //    defaultUnit.Add(new Unit() { unt_arName = "شوكة", unt_enName = " " });
            //    defaultUnit.Add(new Unit() { unt_arName = "صورة", unt_enName = " " });
            //    defaultUnit.Add(new Unit() { unt_arName = "ضفدع", unt_enName = " " });
            //    defaultUnit.Add(new Unit() { unt_arName = "عبوة", unt_enName = " " });
            //    defaultUnit.Add(new Unit() { unt_arName = "علبة", unt_enName = " " });
            //    defaultUnit.Add(new Unit() { unt_arName = "عينة", unt_enName = " " });
            //    defaultUnit.Add(new Unit() { unt_arName = "قطعة", unt_enName = " " });
            //    defaultUnit.Add(new Unit() { unt_arName = "كأس", unt_enName = " " });
            //    defaultUnit.Add(new Unit() { unt_arName = "كجم", unt_enName = " " });
            //    defaultUnit.Add(new Unit() { unt_arName = "كرة", unt_enName = " " });
            //    defaultUnit.Add(new Unit() { unt_arName = "نموذج", unt_enName = " " });
            //    defaultUnit.Add(new Unit() { unt_arName = "لفة", unt_enName = " " });
            //    defaultUnit.Add(new Unit() { unt_arName = "متر", unt_enName = " " });
            //    defaultUnit.Add(new Unit() { unt_arName = "مخبار", unt_enName = " " });
            //    defaultUnit.Add(new Unit() { unt_arName = "مكعب", unt_enName = " " });
            //    defaultUnit.Add(new Unit() { unt_arName = "مل", unt_enName = " " });
            //    defaultUnit.Add(new Unit() { unt_arName = "كيس", unt_enName = " " });
            //    defaultUnit.Add(new Unit() { unt_arName = "لتر", unt_enName = " " });
            //    context.Units.AddRange(defaultUnit);
            //    //////////////////////////////////////
        }
    }
}
