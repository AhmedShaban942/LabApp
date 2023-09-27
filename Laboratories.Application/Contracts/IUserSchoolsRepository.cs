using Laboratories.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratories.Application.Contracts
{
    public interface IUserSchoolsRepository : IRepository<UserSchools>
    {
        IEnumerable<UserSchools> GetAllUserWithSchool(int userId);
        UserSchools GetUserWithSchoolById(int id);
    }
}
