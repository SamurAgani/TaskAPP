using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Models;
using TaskApp.Repositories.Abstract;
using TaskApp.Services.Abstract;

namespace TaskApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : CustomBaseController
    {
        private IGetAllRequestService getAllRequest { get; set; }
        private ICreatePersonService createPerson { get; set; }
        public PersonController( IGetAllRequestService getAllRequestService, ICreatePersonService createPersonService )
        {
            createPerson = createPersonService;
            getAllRequest = getAllRequestService;
        }
        [Route("Create")]
        [HttpPost]
        public async Task<IActionResult> Post(string person)
        {
            var id = await createPerson.Create(person);
            return CreateActionResultInstance(id);
        }
        [HttpPost]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll(GetAllRequest request)
        {
            var persons = await getAllRequest.GetAllRequest(request);
            return CreateActionResultInstance(persons);
        }

    }
}
