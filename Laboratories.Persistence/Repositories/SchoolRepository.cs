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
    public class SchoolRepository : BaseRepository<School>, ISchoolRepository
    {

        public SchoolRepository(LaboratoryDbContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<School> GetAllschoolWithComplex()
        {
            var shools = _dbContext.Schools.Include(x => x.sch_complex);
            return shools;
        }

        public School GetschoolWithComplexById(int id)
        {
            var shool = _dbContext.Schools.Include(c => c.sch_complex).Where(c => c.Id == id).FirstOrDefault();
            return shool;
        }
    }
}
