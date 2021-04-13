using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IntelviaStoreAPI.Helpers;
using IntelviaStoreAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace IntelviaStoreAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<UserData> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWT _jwt;

        public AuthService()
        {
            
        }
        public AuthService(UserManager<UserData> userManager, RoleManager<IdentityRole> roleManager, IOptions<JWT> jwt)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwt = jwt.Value;
        }

        public async Task<Auth> Register(Register register)
        {
            if (await _userManager.FindByEmailAsync(register.Email) is not null || await _userManager.FindByNameAsync(register.UserName) is not null)
            {
                return new Auth {Message = "Email Already registered"};
            }

            

            var usr = new UserData
            {
                UserName = register.UserName,
                Email = register.Email,
                FirstName = register.FirstName,
                LastName = register.LastName,
                ProfilePhoto = register.Photo
            };

            var data = await _userManager.CreateAsync(usr, register.Password);

            if (!data.Succeeded)
            {
                var errors = string.Empty;
                foreach (var error in data.Errors)
                {
                    errors += ("  ", error.Description);
                     
                }

                return new Auth {Message = errors};
            }

            await _userManager.AddToRoleAsync(usr, "User");

            var jwtSecurityToken = await CreateJwtToken(usr);

            return new Auth
            {
                Email = usr.Email,
                Expire = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { "User" },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                UserName = usr.UserName
            };
        }

        public async Task<Auth> Login(Login login)
        {
            var auth = new Auth();

            var user = await _userManager.FindByEmailAsync(login.Email);
            if (user is null || !await _userManager.CheckPasswordAsync(user, login.Password))
            {
                auth.Message = "email or Password incorrect";
                return auth;
            }

            var jwtSecurityToken = await CreateJwtToken(user);
            var roles = await _userManager.GetRolesAsync(user);
            auth.IsAuthenticated = true;
            auth.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            auth.Email = user.Email;
            auth.UserName = user.UserName;
            auth.Expire = jwtSecurityToken.ValidTo;
            auth.Roles = roles.ToList();

            return auth;
        }

        public async Task<string> UserToRoleAssign(UserToRole userRole)
        {
            var usr = await _userManager.FindByIdAsync(userRole.UserId);

            if (usr is null || !await _roleManager.RoleExistsAsync(userRole.roleName))
            {
                return "Something went wrong";
            }

            //return usr is null || !await _roleManager.RoleExistsAsync(userRole.roleName) ? "Something went wrong" : 

            if (await _userManager.IsInRoleAsync(usr, userRole.roleName))
            {
                return "the user is already assigned to this role";
            }

            var assigning = await _userManager.AddToRoleAsync(usr, userRole.roleName);

            return assigning.Succeeded ? string.Empty : "something wrong";
        }


        private async Task<JwtSecurityToken> CreateJwtToken(UserData user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("uid", user.Id)
                }
                .Union(userClaims)
                .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
    }
}
