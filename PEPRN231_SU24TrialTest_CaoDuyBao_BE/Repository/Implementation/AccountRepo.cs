using Microsoft.IdentityModel.Tokens;
using Repository.Interface;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class AccountRepo : GenericRepo<UserAccount>, IAccountRepository
    {
        public AccountRepo(WatercolorsPainting2024DBContext dbContext) : base(dbContext)
        {
        }

        public string GenerateToken(UserAccount user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("_FbWR3vgJnudmwPa0aPeFMS2AOPhpWDI2OIV8Npw0eQ"));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserFullName),
                new Claim(ClaimTypes.Role, user.Role == 1 ? "Admin" :  user.Role == 2 ? "Staff" : "User"),
                //new Claim(ClaimTypes.Role, "User"),
            };
            var securityToken = new JwtSecurityToken(
                expires: DateTime.UtcNow.AddDays(5),
                claims: claims,
                signingCredentials: signingCredentials);
            var jwt = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return jwt;
        }
    }
}
