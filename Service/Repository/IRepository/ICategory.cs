using Model;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Repository.IRepository
{
    public interface ICategory
    {
        Task<List<Category>> GetAllCategory();
        Task<Category?> GetCategoryById(int id);
        Task<string> AddCategoryDB(CategoryDto categoryDto);
        Task<string> EditCategory(int id, CategoryDto categoryDto);
        Task<string> RemoveCategory(int id);
    }
}
