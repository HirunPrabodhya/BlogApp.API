using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.DTO;
using Model.Role;
using Service.Repository.IRepository;

namespace PostAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategory _category;
        public CategoriesController(ICategory category)
        {
            _category= category;
        }
        [HttpGet]
       
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _category.GetAllCategory();
            if(categories.Count == 0)
            {
                return NotFound(
                        new
                        {
                            message="categories do not exist"
                        }
                    );
            }
            return Ok(categories);
        }
        [HttpGet("{id:int}")]
        
        public async Task<IActionResult> GetCategory([FromRoute] int id)
        {
                var existCategory = await _category.GetCategoryById(id);
            if(existCategory == null) 
            {
                return NotFound(new
                {
                    message = id + "st category does not exist"
                });
            }
            return Ok(existCategory);
        }
        [HttpPost]
        [Authorize(Roles = UserRole.admin)]
        public async Task<IActionResult> AddCategory([FromBody]CategoryDto categoryDto)
        {
            var message = await _category.AddCategoryDB(categoryDto);
            return Ok(
                new
                {
                    message = message   
                });
        }
        [HttpPut("{id:int}")]
		[Authorize(Roles = UserRole.admin)]
		public async Task<IActionResult> UpdateCategory([FromRoute]int id,CategoryDto categoryDto)
        {
            var messaage = await _category.EditCategory(id,categoryDto);
            return Ok(new
            {
                message = messaage
            });
        }
        [HttpDelete("{id:int}")]
		[Authorize(Roles = UserRole.admin)]
		public async Task<IActionResult> DeleteCategory([FromRoute]int id)
        {
            var message = await _category.RemoveCategory(id);
            return Ok(
                    new
                    {
                        message = message
                    }
                );
        }
    }
}
