using Laboratories.Application.Contracts;
using Laboratories.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Laboratories.Persistence.Repositories
{
    public class StudentRepository : BaseRepository<Student>, IStudentRepository
    {
        public StudentRepository(LaboratoryDbContext dbContext) : base(dbContext)
        {
        }
        public IEnumerable<Student> GetAllStudentWithSchool()
        {
            var students = _dbContext.Students.Include(x => x.std_School);
            return students;
        }

        public Student GetStudentWithSchoolById(int id)
        {
            var student = _dbContext.Students.Include(c => c.std_School).Where(c => c.Id == id).FirstOrDefault();
            return student;
        }
    }
}
