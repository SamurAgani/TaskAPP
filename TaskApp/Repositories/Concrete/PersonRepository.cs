using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TaskApp.DataContextModel;
using TaskApp.Models;
using TaskApp.Repositories.Abstract;
using TaskApp.Repository.Concrete;

namespace TaskApp.Repositories.Concrete
{
    public class PersonRepository : RepositoryBase<Person>,IPersonRepository
    {
        private DataContext _dataContext;
        public PersonRepository(DataContext dataContext):base(dataContext)
        {
            _dataContext = dataContext;
        }

        public IQueryable<Person> FindByCondition(Expression<Func<Person, bool>> expression)
        {
            return _dataContext.Set<Person>().Where(expression).Include(x=>x.Address);
        }
        public async Task<long> Create(Person person)
        {
            await _dataContext.Set<Person>().AddAsync(person);
            _dataContext.SaveChanges();
            return person.Id;
        }
    }
}
