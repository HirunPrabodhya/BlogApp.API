using Data;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Model;
using Model.DTO;
using Service.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Service.Repository
{
    public class UserService : IUser
    {
        //constructor dependency
        private readonly PostDBContext _dbContext;
        public UserService(PostDBContext postDBContext)
        {
            _dbContext = postDBContext;
        }
        // get all users
        public async Task<List<User>> GetAllUsers()
        {
            return await _dbContext.users.ToListAsync();
        }
        // get specific user
        public async Task<UserDto?> GetUserDB(int id)
        {
            var result = await (from user in _dbContext.users
                                where user.Id == id
                                select new UserDto
                                {
                                    FirstName = user.FirstName,
                                    LastName = user.LastName,
                                    Bio = user.Bio,
                                    Email = user.Email,
                                    Password = user.Password,
                                    UserTypeId = user.UserTypeId,
                                }).SingleAsync();
            return result; ;
        }
        // add user
        public async Task<string> AddUserDb(UserDto userDto)
        {

            if (userDto == null)
            {
                return "user data is incompleted";
            }
            var existUser = await _dbContext.users.FirstOrDefaultAsync(x => x.Email == userDto.Email);
            if (existUser != null)
            {
                return "user is existed";
            }
            var user = new User
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Bio = userDto.Bio,
                Email = userDto.Email,
                Password = userDto.Password,
                UserTypeId = userDto.UserTypeId,
            };
            await _dbContext.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return "user is added";
        }

        // update user
        public async Task<string> UpdateUserDb(int id, UserUpdateDto userDto)
        {
            var existUser = await _dbContext.users.FirstOrDefaultAsync(x => x.Id == id);
            if (existUser == null)
            {
                return id + " st user does not exist";
            }
            existUser.FirstName = userDto.FirstName;
            existUser.LastName = userDto.LastName;
            existUser.Bio = userDto.Bio;
            existUser.Email = userDto.Email;
            existUser.Password = userDto.Password;


            await _dbContext.SaveChangesAsync();
            return id + " st user is updated";
        }
        // delete user
        public async Task<string> DeleteUserDb(int id)
        {
            var user = await _dbContext.users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return "user does not exist";
            }
            _dbContext.users.Remove(user);
            await _dbContext.SaveChangesAsync();
            return id + " st user was deleted";
        }

        public async Task<int> AuthenicateDb(LoginDto loginDto)
        {

            int userId = await (from user in _dbContext.users
                                   where user.Email == loginDto.Email
                                   && user.Password == loginDto.Password
                                   select user.Id
                                ).SingleOrDefaultAsync();
            return userId;
        }
  

        public async Task<string?> GetUserNameAsync(int id)
        {
            var existingUser = await _dbContext.users.FirstOrDefaultAsync(x => x.Id == id);
            if (existingUser == null)
            {
                return null;
            }
            return await (
                           from user in _dbContext.users
                           where user.Id == id
                           select user.FirstName
                          ).SingleOrDefaultAsync();
        }
    }

}
