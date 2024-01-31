using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.DTO;
using Model.Role;
using Service.Repository.IRepository;

namespace PostAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUser _user;
        private readonly IEmail _email;
        private readonly IToken _token;
        public UsersController(IUser user, IEmail email,IToken token) 
        { 
            _user= user;
            _email= email;
            _token= token;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Authenticate([FromBody] LoginDto loginDto)
        {
            var userId = await _user.AuthenicateDb(loginDto);
            if (userId == 0)
            {
               return NotFound(
                    new
                    {
                        message = "user does not exist"
                    }
                   );
             }
            string token = await _token.GetToken(userId);

            return Ok(new
                        {
                            token = token
                        }
            );

        }
        [HttpPost]
        [Authorize(Roles = UserRole.admin)]
        public async Task<IActionResult> AddUser([FromBody] UserDto userDto)
        {
            var message = await _user.AddUserDb(userDto);
        
            if (message is not null && userDto.UserTypeId == 2)
            {
                message += ", ";
               message +=  await _email.SendEmailAsync(null, new EmailToUser {
				                                   FirstName = userDto.FirstName,
				                                   LastName = userDto.LastName,
												   Email = userDto.Email, 
                                                   Password = userDto.Password
                                                    }
                         );

			}
            
            return Ok(
                new
                {
                    message = message
                }
               
                );

        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUser([FromRoute]int id)
        {
            var user = await _user.GetUserDB(id);
            if (user == null)
            {
                return NotFound(
                    new {
                            message = id + " st user does not exist"
                    }
                    );
            }
            return Ok(user);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _user.GetAllUsers();  
            if(users.Count == 0)
            {
                return NotFound(
                        new
                        {
                            message = "user does not exist"
                        }
                    ); 
            }
            return Ok(users);
        }
        [HttpGet("[action]/{id:int}")]
        public async Task<IActionResult> GetUserFirstName([FromRoute] int id)
        {
            var firstname = await _user.GetUserNameAsync(id);
            if(firstname == null)
            {
                return NotFound(new
                {
                    message = "user does not exist"
                });
            }
            return Ok(new { 
                        firstName = firstname
            });
        }
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,Author")]
        public async Task<IActionResult> UpdateUser([FromRoute]int id ,UserUpdateDto userUpdateDto)
        {
            var message = await _user.UpdateUserDb(id, userUpdateDto);
            return Ok(new
            {
                message = message
			});

        }
        [HttpDelete("{id:int}")]
		[Authorize(Roles = "Admin,Author")]
		public async Task<IActionResult> DeleteUser([FromRoute]int id)
        {
            var message = await _user.DeleteUserDb(id);
            return Ok(
                    new
                    {
                        message = message
                    }
                );
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> UserRequiest([FromForm] UserRequestDto request)
        {
            var message = await _email.SendEmailAsync(request,null);
            if(message == "") 
            {
                return BadRequest();
            }

            return Ok(new
            {
                message = message
            });
        }
        
    }
}
