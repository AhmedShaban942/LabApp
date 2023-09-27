using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratories.Domain
{
    public enum Department
    {
        ابتدائى,
        متوسط,
        ثانوى
    }
    public enum Level
    {
        اول,
        ثانى,
        ثالث,
        رابع,
        خامس,
        سادس
    }
    public enum Term
    {
        الاول,
        الثانى,
        الثالث
    }
    public enum Type
    {
        علوم,
         //البيئة,
        احياء,
        كيمياء,
        فيزياء,
        اثاث_مختبر
    }
    public enum AvailableMethod
    {
        المستودع,
        المدرسة
    }
    public enum ValidState
    {
       صالح ,
        غير_صالح
    }
    public enum ExsistState
    {
        موجود,
        غير_موجود
    }
    public enum Over
    {
        زائد,
        غير_زائد
    }
    public class Item : BaseEntity
    {


        [Required]
        public string itm_code { get; set; }

        public int? itm_ministerialNo { get; set; } = 0;  //رقم الصنف الوزارى


        [Required]
        public string itm_arName { get; set; }
        public string itm_enName { get; set; } = "";
        public string itm_desc { get; set; } = "";
        [Required]
        public Department itm_department { get; set; }

        public Level itm_level { get; set; } = 0;
        [Required]
        public Type itm_type { get; set; }
        public byte[] itm_image { get; set; }
        public int? itm_unitId { get; set; }

        [ForeignKey("itm_unitId")]
        public Unit itm_Unit { get; set; }

        public double? itm_sugQty { get; set; } = 0.0; //الكمية مقترحة

        public double? itm_presentQty { get; set; } = 0.0; //الكمية الموجودة

        public ExsistState itm_isExisting { get; set; } = ExsistState.غير_موجود; //  الصنف موجود او لا 
        public AvailableMethod itm_availableMethod { get; set; } = AvailableMethod.المدرسة; //(امكانية التوفير : يمكن توفيره  -المستودع)

        public int? itm_chapter { get; set; } = 0;  //الوحدات 

        public Term itm_term { get; set; } = 0;  //الفصل الدراسى

        public int itm_schId { get; set; } = 1;

        [ForeignKey("itm_schId")]
        public School itm_School { get; set; }
        
        public ValidState itm_ValidState { get; set; } = ValidState.صالح;  //صالح او غير
        public int? itm_completionYear { get; set; } = 0; //سنة الانتهاء

        public double? itm_excessiveQty { get; set; } = 0;  //احتياج تكميلى

        public string itm_note { get; set; } = "";

        public Over itm_over { get; set; } = Over.غير_زائد;  //زائد او غير


        //to Basic only 
        public double? itm_validQty { get; set; } = 0.0; //الكمية الصالحة
        public double? itm_unValidQty { get; set; } = 0.0; //الكمية الغير الصالحة
        public double? itm_overQty { get; set; } = 0.0; //الزيادة

        public int itm_year { get; set; } = 2023;


    }
    //public class Item: BaseEntity
    // {
    //     [Required]
    //     public int itm_code { get; set; }
    //     [Required]
    //     public string itm_arName { get; set; }
    //     public string itm_enName { get; set; }
    //     public string itm_desc { get; set; }
    //     [Required]
    //     public int itm_department { get; set; }
    //     [Required]
    //     public int itm_level { get; set; }
    //     public int itm_unitId { get; set; }

    //     [ForeignKey("itm_unitId")]
    //     public Unit itm_Unit { get; set; }
    //     [Required]
    //     public double itm_sugQty { get; set; }  //الكمية مقترحة
    //     [Required]
    //     public double itm_presentQty { get; set; }  //الكمية الموجودة

    //     public bool itm_isExisting { get; set; } //  الصنف موجود او لا 
    //     public bool itm_availableMethod { get; set; } //(امكانية التوفير : يمكن توفيره  -المستودع)
    //     [Required]
    //     public int itm_chapter { get; set; }  //الوحدات 
    //     [Required]
    //     public int itm_term { get; set; }  //الفصل الدراسى

    //     public int itm_schId { get; set; }

    //     [ForeignKey("itm_schId")]
    //     public School itm_School { get; set; }


    // }
}
