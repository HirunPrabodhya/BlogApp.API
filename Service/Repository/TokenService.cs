using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Model;
using Service.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service.Repository
{
	public class TokenService : IToken
	{
		private readonly PostDBContext _dbContext;
        public TokenService(PostDBContext dBContext)
        {
            _dbContext = dBContext;
        }
        public async Task<string> GetToken(int userId)
		{
			var result = await _dbContext.users.Include(c => c.UserType).FirstOrDefaultAsync(x => x.Id == userId);
			if (result == null)
			{
				return "user with type does not exist";
			}
			return generateToken(result);
			
		}
		private string generateToken(User user)
		{
			var jwtTokenHandler = new JwtSecurityTokenHandler();
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
			var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


			var claims = new ClaimsIdentity(
					new Claim[]
					{
						new Claim(ClaimTypes.Role,user.UserType!.Name),
						new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
					}
				);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = claims,
				Expires = DateTime.Now.AddDays(1),
				SigningCredentials = signingCredentials
			};
			var tokenString = jwtTokenHandler.CreateToken(tokenDescriptor);
			return jwtTokenHandler.WriteToken(tokenString);
		}
	}
}
