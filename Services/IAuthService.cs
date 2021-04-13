using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IntelviaStoreAPI.Models;

namespace IntelviaStoreAPI.Services
{
    public interface IAuthService
    {
        Task<Auth> Register(Register register);
        Task<Auth> Login(Login login);
        Task<string> UserToRoleAssign(UserToRole userRole);

    }
}
