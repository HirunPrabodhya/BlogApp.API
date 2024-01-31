using Data;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.DTO;
using Service.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Repository
{
    public class CategoryService : ICategory
    {
        private PostDBContext dbContext;
        public CategoryService(PostDBContext postDBContext)
        {
            dbContext = postDBContext;
        }
        public async Task<List<Category>> GetAllCategory()
        {
            return await dbContext.categories.ToListAsync();
        }
        public async Task<Category?> GetCategoryById(int id)
        {
            return await dbContext.categories.FirstOrDefaultAsync(c => c.Id == id);

        }
        public async Task<string> AddCategoryDB(CategoryDto categoryDto)
        {
            if (categoryDto == null)
            {
                return "category is incompleted";
            }
            var category = new Category
            {
                Name = categoryDto.Name,
                Description = categoryDto.Description,
                ImageUrl = categoryDto.ImageUrl
            };
            await dbContext.categories.AddAsync(category);
            await dbContext.SaveChangesAsync();
            return "category is added";
        }

        public async Task<string> EditCategory(int id, CategoryDto categoryDto)
        {
            var exitCategory = await dbContext.categories.FirstOrDefaultAsync(c => c.Id == id);
            if (exitCategory == null)
            {
                return id + "st category does not exist.";
            }
            exitCategory.Name = categoryDto.Name;
            exitCategory.Description = categoryDto.Description;
            exitCategory.ImageUrl = categoryDto.ImageUrl;
            await dbContext.SaveChangesAsync();
            return "category is updated";
        }



        public async Task<string> RemoveCategory(int id)
        {
            var deleteCategory = await dbContext.categories.FirstOrDefaultAsync(x => x.Id == id);
            if (deleteCategory == null)
            {
                return id + "st category is not existed";
            }
            dbContext.categories.Remove(deleteCategory);
            await dbContext.SaveChangesAsync();
            return "category is deleted";
        }


    }
}
