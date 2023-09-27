using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Laboratories.Web.ViewModel
{
    public class ItemViewModel
    {

        public int itm_ministerialNo { get; set; }
        public string itm_code { get; set; }

        public string itm_arName { get; set; }
        public string itm_enName { get; set; }
        public string itm_desc { get; set; }

        public string itm_department { get; set; }

        public string itm_level { get; set; }

        public string itm_type { get; set; }



        public string itm_Unit { get; set; }

        public double itm_sugQty { get; set; }  //الكمية مقترحة

        public double itm_presentQty { get; set; }  //الكمية الموجودة

        public string itm_isExisting { get; set; } //  الصنف موجود او لا 
        public string itm_availableMethod { get; set; } //(امكانية التوفير : يمكن توفيره  -يطلب)

        public int itm_chapter { get; set; }  //الوحدات 

        public string itm_term { get; set; }  //الفصل الدراسى

        public string itm_School { get; set; }

        public string itm_ValidState { get; set; }  //صالح او غير
        public int itm_completionYear { get; set; } = 0; //سنة الانتهاء

        public double itm_excessiveQty { get; set; } = 0;  //احتياج تكميلى

        public string itm_note { get; set; } = "";

        public string itm_over { get; set; }

        public string sch_type { get; set; }

        public string  company { get; set; }
        public string complex { get; set; }

        public string test { get; set; }


        //public byte[] com_image { get; set; }

        //public byte[] comp_image { get; set; }

        //public byte[] sch_image { get; set; }

    }
}