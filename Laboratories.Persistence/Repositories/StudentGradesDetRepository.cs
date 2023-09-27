using Laboratories.Application.Contracts;
using Laboratories.Domain;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
namespace Laboratories.Persistence.Repositories
{

    internal class StudentGradesDetRepository : BaseRepository<StudentGradesDet>, IStudentGradesDetRepository
    {
        public StudentGradesDetRepository(LaboratoryDbContext dbContext) : base(dbContext)
        {
        }
        public IEnumerable<StudentGradesDet> GetAllStudentGradesDetWithStudent(long id)
        {
            var studentGradesDetList = _dbContext.StudentGradesDet.Include(x => x.stdGD_Student).Where(s=>s.stdGD_studentGradesHedId==id);
            return studentGradesDetList;
        }



        public StudentGradesDet GetStudentGradesDetWithStudentById(long id,int studentId)
        {
            var studentGradesDet = _dbContext.StudentGradesDet.Include(x => x.stdGD_Student).Where(c => c.stdGD_studentGradesHedId == id &&c.stdGD_studentId== studentId).FirstOrDefault();
            return studentGradesDet;
        }


    }
}
