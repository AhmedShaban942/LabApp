using Laboratories.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratories.Application.Contracts
{

       public interface IStudentGradesDetRepository : IRepository<StudentGradesDet>
    {
        IEnumerable<StudentGradesDet> GetAllStudentGradesDetWithStudent(long id);
        StudentGradesDet GetStudentGradesDetWithStudentById(long id , int studentId);
    }
}
