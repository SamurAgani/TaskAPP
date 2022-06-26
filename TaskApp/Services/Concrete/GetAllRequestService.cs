using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Models;
using TaskApp.Repositories.Abstract;
using TaskApp.Services.Abstract;

namespace TaskApp.Services.Concrete
{
    public class GetAllRequestService : IGetAllRequestService
    {
        public ISamurSoft samurSoft { get; set; }

        private IPersonRepository personRepository { get; set; }
        public GetAllRequestService(IPersonRepository personRepository)
        {
            this.samurSoft = new SamurSoft((new Person()).GetType());
            this.personRepository = personRepository;
        }
        public async Task<ResponseModel<string>> GetAllRequest(GetAllRequest getAllRequest)
        {
            return await Task.Run(()=> {
                var persons = personRepository.FindByCondition(x => (!string.IsNullOrWhiteSpace(getAllRequest.FirstName) ? x.FirstName == getAllRequest.FirstName : true) &&
                                                                    (!string.IsNullOrWhiteSpace(getAllRequest.LastName) ? x.LastName == getAllRequest.LastName : true) &&
                                                                    (!string.IsNullOrWhiteSpace(getAllRequest.City) ? x.Address.City == getAllRequest.City : true)).FirstOrDefault();
               
               
                if(string.IsNullOrEmpty(getAllRequest.City) && string.IsNullOrEmpty(getAllRequest.FirstName) && string.IsNullOrEmpty(getAllRequest.LastName))
                {
                    return ResponseModel<string>.Fail("Search datas are empty!", 400); ;
                }
                if (persons is null)
                    return ResponseModel<string>.Fail("Record not found!", 404); ;
                var json = samurSoft.Serialize(persons);
                    return ResponseModel<string>.Success(json, 200);
            });
            
        }
    }
}
