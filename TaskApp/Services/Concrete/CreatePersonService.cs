using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Models;
using TaskApp.Repositories.Abstract;
using TaskApp.Services.Abstract;

namespace TaskApp.Services.Concrete
{
    public class CreatePersonService : ICreatePersonService
    {
        public ISamurSoft samurSoft { get; set; }

        private IPersonRepository repository { get; set; }
        public CreatePersonService(IPersonRepository personRepository)
        {
            this.samurSoft = new SamurSoft((new Person()).GetType());
            repository = personRepository;
        }

        public async Task<ResponseModel<long>> Create(string personJson)
        {            
            var person = samurSoft.Deserialize(personJson) as Person;
            long id = await repository.Create(person);
            return ResponseModel<long>.Success(id, 204);

        }
    }
}
