using Laboratories.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratories.Application.Contracts
{
    public interface ICompanyRepository: IRepository<Company>
    {
        IEnumerable<Company> GetAllCompany();
        Company GetCompanyById(int id);
    }
}
