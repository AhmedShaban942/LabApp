using Laboratories.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Laboratories.Application.Contracts;
using System.Linq.Expressions;

namespace Laboratories.Persistence.Repositories
{
    internal class ItemRepository : BaseRepository<Item>, IItemRepository
    {

        public ItemRepository(LaboratoryDbContext dbContext) : base(dbContext)
        {
        }


        public IEnumerable<Item> GetAllItemViewModel()
        {

            var items = _dbContext.Items.Include(x => x.itm_Unit).Include(x => x.itm_School).Include(x=>x.itm_School.sch_complex).Include(x => x.itm_School.sch_complex.comp_company);
            return items;
        }
        public IEnumerable<Item> GetAllItemViewModel(Expression<Func<Item, bool>> criteria)
        {

            var items = _dbContext.Items.Where(criteria).Include(x => x.itm_Unit).Include(x => x.itm_School).Include(x => x.itm_School.sch_complex).Include(x => x.itm_School.sch_complex.comp_company);
            return items;
        }

        public Item GetItemViewModel(int id)
        {
            var item = _dbContext.Items.Include(x => x.itm_Unit).Include(x => x.itm_School).Where(c => c.Id == id).FirstOrDefault();
            return item;
        }

        public IEnumerable<Item> GetAllItemWithUnit()
        {

            var items = _dbContext.Items.Include(x => x.itm_Unit);
            return items;
        }

        public IEnumerable<Item> GetAllItemWithUnit(Expression<Func<Item, bool>> criteria)
        {

            var items = _dbContext.Items.Where(criteria).Include(x => x.itm_Unit);
            return items;
        }
        public Item GetItemWithUnit(int id)
        {
            var item = _dbContext.Items.Include(x => x.itm_Unit).Where(c => c.Id == id).FirstOrDefault();
            return item;
        }
        public void AddRange(IEnumerable<Item> collection)
        {
            _dbContext.Items.AddRange(collection);
            _dbContext.SaveChanges();

        }

    }
}
