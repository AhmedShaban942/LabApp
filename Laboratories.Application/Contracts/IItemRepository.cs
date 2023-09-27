using Laboratories.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Laboratories.Application.Contracts
{
    public interface IItemRepository : IRepository<Item>
    {

        IEnumerable<Item> GetAllItemViewModel();
        IEnumerable<Item> GetAllItemViewModel(Expression<Func<Item, bool>> criteria);
        Item GetItemViewModel(int id);
        void AddRange(IEnumerable<Item> collection);

        IEnumerable<Item> GetAllItemWithUnit();
        IEnumerable<Item> GetAllItemWithUnit(Expression<Func<Item, bool>> criteria);
        Item GetItemWithUnit(int id);

    }
}
