using IntelviaStoreAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IntelviaStoreAPI.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace IntelviaStoreAPI.Services
{
    public class UserService : IUserService
    {
        //private readonly StoreContext _storeContext;
        private readonly UserManager<UserData> _userManager;
        private readonly JWT _jwt;

        public UserService(UserManager<UserData> userManager, IOptions<JWT> jwt)
        {
            _userManager = userManager;
            _jwt = jwt.Value;
        }
        public async Task<IEnumerable<Register>> GetUser()
        {
            //return await _storeContext.UserInfo.ToListAsync();
            //return await _storeContext.UserInfo.Except()
            return (from u in _userManager.Users
                select new Register()
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    UserName = u.UserName
                });
        }

        public async Task<Register> EditUser([FromBody]Register register, string id)
        {
            var findUser = await _userManager.FindByIdAsync(id);



            if (findUser is not null)
            {
                if (!string.IsNullOrEmpty(register.Email) && string.IsNullOrEmpty(register.Password))
                {
                    findUser.Email = register.Email;
                    findUser.FirstName = register.FirstName;
                    findUser.LastName = register.LastName;
                    findUser.UserName = register.UserName;
                    findUser.ProfilePhoto = register.Photo;
                }
                else if (!string.IsNullOrEmpty(register.Email) && !string.IsNullOrEmpty(register.Password))
                {
                    findUser.Email = register.Email;
                    findUser.FirstName = register.FirstName;
                    findUser.LastName = register.LastName;
                    findUser.UserName = register.UserName;
                    findUser.ProfilePhoto = register.Photo;
                    var updatedData = await _userManager.CreateAsync(findUser, register.Password);

                    if (!updatedData.Succeeded)
                    {
                        var errors = string.Empty;
                        foreach (var error in updatedData.Errors)
                        {
                            errors += ("  ", error.Description);

                        }

                       
                    }
                }
                
            }

            return new Register
            {
                FirstName = findUser.FirstName,
                LastName = findUser.LastName,
                Email = findUser.Email,
                UserName = findUser.UserName
            };

        }

        public async Task<string> DeleteUser(string id)
        {
            var usr = await _userManager.FindByIdAsync(id);
            if (usr is not null)
            {
                var del = await _userManager.DeleteAsync(usr);
                if (!del.Succeeded)
                {
                    var errors = string.Empty;
                    foreach (var error in del.Errors)
                    {
                        errors += ("  ", error.Description);

                    }
                }

            }
            return $"the User {usr.FirstName} {usr.LastName} has been deleted";
        }
    }
}
