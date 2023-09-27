using Laboratories.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratories.Application.Contracts
{
    public interface IComplexRepository : IRepository<Complex>
    {
        IEnumerable<Complex> GetAllComplexWithCompany();
        Complex GetComplexWithCompanyById(int id);
    }
}
