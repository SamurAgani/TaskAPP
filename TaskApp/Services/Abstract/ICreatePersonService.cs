using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Models;

namespace TaskApp.Services.Abstract
{
    public interface ICreatePersonService
    {
        Task<ResponseModel<long>> Create(string personJson);
    }
}
