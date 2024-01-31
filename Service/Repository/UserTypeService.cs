using Data;
using Microsoft.EntityFrameworkCore;
using Model;
using Service.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Repository
{
    public class UserTypeService : IUserType
    {
        private PostDBContext _dbContext;
        public UserTypeService(PostDBContext postDBContext)
        {
            _dbContext = postDBContext;
        }
        public async Task<List<UserType>> GetAllUserTypes()
        {
            return await _dbContext.usersType.ToListAsync();
        }
    }
}
