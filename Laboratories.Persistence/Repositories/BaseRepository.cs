using Laboratories.Application.Contracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Laboratories.Persistence.Repositories
{
    public partial class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly LaboratoryDbContext _dbContext;
        private DbSet<T> table = null;
        public BaseRepository(LaboratoryDbContext dbContext)
        {
            this._dbContext = dbContext;
            this.table = _dbContext.Set<T>();
        }
        #region Get Data
        public  List<T> GetAll()
        {
            return  _dbContext.Set<T>().ToList();
        }
        public  T GetByNum(object num)
        {
            return  table.Find(num);
        }

        public  int GetMaxNum(Expression<Func<T, int>> criteria)
        {
            return  table.Max(criteria) + 1;

        }
        public List<T> MultiSearch(Expression<Func<T, bool>> criteria)
        {
            return  table.Where(criteria).ToList();
        }
        public T SingleSearch(Expression<Func<T, bool>> criteria)
        {
            return  table.FirstOrDefault(criteria);
        }
        #endregion



        #region Operation
        public  T Insert(T obj)
        {
             table.Add(obj);
             _dbContext.SaveChanges();
            return obj;
        }
        public T Update(T obj)
        {
            _dbContext.Entry(obj).State = EntityState.Modified;
             _dbContext.SaveChanges();
            return obj;
        }
        public T Delete(object num)
        {
            T existing =  table.Find(num);
            table.Remove(existing);
             _dbContext.SaveChanges();
            return existing;
        }
        public bool AddRange(IEnumerable<T> obj)
        {
            using (_dbContext)
            {


                using (DbContextTransaction transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        _dbContext.Set<T>().AddRange(obj);
                        var x = _dbContext.SaveChanges();
                        transaction.Commit();
                       
                        if (x > 0)
                        {
                        
                            return true;
                        }
                        else
                            return false;

                
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }


        }
        public bool RemoveRange(IEnumerable<T> entities)
        {

            using (_dbContext)
            {


                using (DbContextTransaction transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        _dbContext.Set<T>().RemoveRange(entities);
                        var x = _dbContext.SaveChanges();
                        transaction.Commit();

                        if (x > 0)
                        {

                            return true;
                        }
                        else
                            return false;


                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }

        }
        #endregion



    }
}
