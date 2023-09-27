using Laboratories.Application.Contracts;
using Laboratories.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
namespace Laboratories.Persistence.Repositories
{
    internal class ScheduleHedRepository : BaseRepository<ScheduleHed>,IScheduleHedRepository
    {
        public ScheduleHedRepository(LaboratoryDbContext dbContext) : base(dbContext)
        {
        }
        public IEnumerable<ScheduleHed> GetAllScheduleHedWithTeacher()
        {
            var scheduleHed = _dbContext.ScheduleHed.Include(x => x.schd_teacher).Include(x => x.schd_teacher.tech_School);
            return scheduleHed;
        }

        public ScheduleHed GetScheduleHedWithTeacherById(int id)
        {
            var scheduleHed = _dbContext.ScheduleHed.Include(c => c.schd_teacher).Include(c => c.schd_teacher.tech_School).Where(c => c.schd_teachId == id).FirstOrDefault();
            return scheduleHed;
        }
    }
}
