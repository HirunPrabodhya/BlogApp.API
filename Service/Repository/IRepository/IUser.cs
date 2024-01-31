using Model;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Repository.IRepository
{
    public interface IUser
    {
        Task<string> AddUserDb(UserDto userDto);
        Task<int> AuthenicateDb(LoginDto loginDto);
        Task<List<User>> GetAllUsers();
        Task<UserDto?> GetUserDB(int id);
        Task<string?> GetUserNameAsync(int id);
        Task<string> UpdateUserDb(int id, UserUpdateDto userDto);
        Task<string> DeleteUserDb(int id);
    }
}
