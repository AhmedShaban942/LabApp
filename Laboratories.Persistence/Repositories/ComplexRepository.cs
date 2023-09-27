using Laboratories.Application.Contracts;
using Laboratories.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratories.Persistence.Repositories
{
    public class ComplexRepository : BaseRepository<Complex>, IComplexRepository
    {

        public ComplexRepository(LaboratoryDbContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<Complex> GetAllComplexWithCompany()
        {
            var complices = _dbContext.Complices.Include(x=>x.comp_company);
             return complices;
        }

        public Complex GetComplexWithCompanyById(int id)
        {
            var complex = _dbContext.Complices.Include(c => c.comp_company).Where(c => c.Id == id).FirstOrDefault();
            return complex;
        }
    }
}
