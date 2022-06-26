using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Models;
using TaskApp.Repository.Abstract;

namespace TaskApp.Repositories.Abstract
{
    public interface IPersonRepository : IRepositoryBase<Person>
    {
        public Task<long> Create(Person person);
    }
}
