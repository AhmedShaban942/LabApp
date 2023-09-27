using Laboratories.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratories.Application.Contracts
{
    public interface IScheduleHedRepository : IRepository<ScheduleHed>
    {
        IEnumerable<ScheduleHed> GetAllScheduleHedWithTeacher();
        ScheduleHed GetScheduleHedWithTeacherById(int id);
    
}
}
