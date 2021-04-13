using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IntelviaStoreAPI.Models;

namespace IntelviaStoreAPI.Services
{
    public interface IUserService
    {
        Task<IEnumerable<Register>> GetUser();
        Task<Register> EditUser(Register register, string id);
        Task<string> DeleteUser(string id);


    }
}
