using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Laboratories.Web.ViewModel
{
    public class StudentGradeViewModel
    {
        public int SchoolId { get; set; } = 1;


        public string SchoolName { get; set; }

        public int StudyCourseId { get; set; } = 1;


        public string StudyCourseName { get; set; }

        public int TeacherId { get; set; } = 1;


        public string TeacherName { get; set; }

        public int ExamId { get; set; } = 1;


        public string ExamName { get; set; }

        public string stdGH_Date { get; set; }
        public string Term { get; set; }
        public string Level { get; set; }
        public string StdRecord { get; set; }


        public int StudentId { get; set; } = 1;

        public string StudentName { get; set; }


        public double Degree { get; set; }

        public double Rate { get; set; }
    }
}