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
    public class ExperimentRepository : BaseRepository<Experiments>, IExperimentRepository
    {

        public ExperimentRepository(LaboratoryDbContext dbContext) : base(dbContext)
        {
        }


        public IEnumerable<Experiments> GetAllExperimentsWithCourses()
        {
            var experiments = _dbContext.Experiments.Include(x => x.expr_course).Include(x => x.expr_teacher).Include(x=>x.expr_teacher.tech_School);
            return experiments;
        }

     
        public Experiments GetExperimentsWithCourseById(int id)
        {
            var experiment = _dbContext.Experiments.Include(c => c.expr_course).Include(x => x.expr_teacher).Include(x => x.expr_teacher.tech_School).Where(c => c.Id == id).FirstOrDefault();
            return experiment;
        }
    }
}
