using Laboratories.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratories.Application.Contracts
{

      public interface IExperimentRepository : IRepository<Experiments>
    {
        IEnumerable<Experiments> GetAllExperimentsWithCourses();
        Experiments GetExperimentsWithCourseById(int id);
    }
}
