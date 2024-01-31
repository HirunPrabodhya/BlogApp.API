using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Repository.IRepository;

namespace PostAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTypesController : ControllerBase
    {
        private readonly IUserType _userType;
        public UserTypesController(IUserType userType)
        {
            _userType= userType;
        }
        [HttpGet]
        public async Task<IActionResult> GetTypes()
        {
            var userType = await _userType.GetAllUserTypes();
            if(userType == null)
            {
                return NotFound(
                        new
                        {
                            message = "userType does not exist"
                        }
                    );
            }
            return Ok( userType );
        }
    }
}
