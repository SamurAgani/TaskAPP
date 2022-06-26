using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Models;

namespace TaskApp.Services.Abstract
{
    public interface IGetAllRequestService
    {
        Task<ResponseModel<string>> GetAllRequest(GetAllRequest getAllRequest);
    }
}
