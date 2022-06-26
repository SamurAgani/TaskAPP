using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TaskApp.DataContextModel;
using TaskApp.Repository.Abstract;

namespace TaskApp.Repository.Concrete
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private DataContext _dataContext { get; set; }

        private DbSet<T> dbSet { get; set; }
        public RepositoryBase(DataContext dataContext)
        {
            _dataContext = dataContext;
            dbSet = dataContext.Set<T>();
        }

        public void Create(T entity)
        {
            dbSet.Add(entity);
            _dataContext.SaveChanges();

        }

        public void Delete(T entity)
        {
            dbSet.Remove(entity);
            _dataContext.SaveChanges();
        }

        public IQueryable<T> FindAll()
        {
            return dbSet;
        }

        public  IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return dbSet.Where(expression);
        }

        public void Update(T entity)
        {
            dbSet.Update(entity);
            _dataContext.SaveChanges();
        }
    }
}
