using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Laboratories.Application.Contracts
{
    public interface IRepository<T> where T : class
    {


        List<T> GetAll();
        T GetByNum(object num);
        int GetMaxNum(Expression<Func<T, int>> criteria);
        List<T> MultiSearch(Expression<Func<T, bool>> criteria);
        T SingleSearch(Expression<Func<T, bool>> criteria);
        T Insert(T obj);
        T Update(T obj);
        T Delete(object num);
        bool AddRange(IEnumerable<T> entities);
        bool RemoveRange(IEnumerable<T> entities);

    }
}
