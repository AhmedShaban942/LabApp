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
   public class UserRepository : BaseRepository<User>, IUserRepository
    {

        public UserRepository(LaboratoryDbContext dbContext) : base(dbContext)
        {
       
        }

        public IEnumerable<User> GetAllUserWithSchool()
        {
            var users = _dbContext.Users.Include(x => x.UserSchools).Include(c => c.usr_Role);
            return users;
        }

        public User GetUserWithSchoolById(int id)
        {
            var user = _dbContext.Users.Include(c => c.UserSchools).Include(c => c.usr_Role).Where(c => c.Id == id).FirstOrDefault();
            return user;
        }
    }
}
