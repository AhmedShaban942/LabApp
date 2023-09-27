using Laboratories.Application.Contracts;
using Laboratories.Domain;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
namespace Laboratories.Persistence.Repositories
{
    internal class StudentGradesHedRepository : BaseRepository<StudentGradesHed>, IStudentGradesHedRepository
    {
        public StudentGradesHedRepository(LaboratoryDbContext dbContext) : base(dbContext)
        {
        }
        public IEnumerable<StudentGradesHed> GetAllStudentGradesHedWithStudent()
        {
            var studentGradesHedList = _dbContext.StudentGradesHed.Include(x => x.stdGH_Exams).Include(x => x.stdGH_School).Include(x => x.stdGH_StudyCourse).Include(x => x.stdGH_Teacher);
            return studentGradesHedList;
        }

    

        public StudentGradesHed GetStudentGradesHedWithStudentById(long id)
        {
            var studentGradesHed = _dbContext.StudentGradesHed.Include(x => x.stdGH_Exams).Include(x => x.stdGH_School).Include(x => x.stdGH_StudyCourse).Include(x => x.stdGH_Teacher).Where(c => c.stdGH_Id == id).FirstOrDefault();
            return studentGradesHed;
        }

     
    }
}
