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
    public class UserSchoolsRepository : BaseRepository<UserSchools>, IUserSchoolsRepository
    {
       public UserSchoolsRepository(LaboratoryDbContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<UserSchools> GetAllUserWithSchool(int userId)
        {
            var userSchools = _dbContext.userSchools.Include(x => x.School).Where(u=>u.User_Id==userId);
            return userSchools;
        }

        public UserSchools GetUserWithSchoolById(int id)
        {
            var userSchools = _dbContext.userSchools.Include(c => c.School).Where(c => c.Id == id).FirstOrDefault();
            return userSchools;
        }
    }
}
