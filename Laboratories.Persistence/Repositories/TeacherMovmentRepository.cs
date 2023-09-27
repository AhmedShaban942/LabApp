using Laboratories.Application.Contracts;
using Laboratories.Domain;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratories.Persistence.Repositories
{
   public class TeacherMovmentRepository : BaseRepository<TeacherMovment>, ITeacherMovmentRepository
    {
        private DbSet<TeacherMovment> table = null;
        public TeacherMovmentRepository(LaboratoryDbContext dbContext) : base(dbContext)
        {
            this.table = _dbContext.Set<TeacherMovment>();
        }

        public TeacherMovment DeleteUsingMultiKey(TeacherMovment existing)
        {
            
            table.Remove(existing);
            _dbContext.SaveChanges();
            return existing;
        }

        public IEnumerable<TeacherMovment> GetAllTeacherMovmentWithTeacher()
        {
            var teacherMovment = _dbContext.TeacherMovments.Include(x => x.mov_Teacher).Include(x => x.mov_Teacher.tech_School);
            return teacherMovment;
        }

        public TeacherMovment GetTeacherMovmentWithTeacherById(int id)
        {
            var teacherMovment = _dbContext.TeacherMovments.Include(c => c.mov_Teacher).Include(c=>c.mov_Teacher.tech_School).Where(c => c.Id == id).FirstOrDefault();
            return teacherMovment;
        }

    }

}
