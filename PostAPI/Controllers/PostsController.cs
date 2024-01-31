
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Model.DTO;
using Model.Role;
using Service.Repository.IRepository;

namespace PostAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPost _post;
        private readonly IEmail _email;
        private readonly ISubscriber _subscriber;
        
        public PostsController(IPost post, IEmail email, ISubscriber subscriber)
        {
            _post= post;
            _email= email;
           _subscriber= subscriber;
        }
        [HttpPost]
		[Authorize(Roles = UserRole.author)]
		public async Task<IActionResult> InsertPost(PostDto post)
        {
            var messagePost = await _post.AddPost(post);
            var subscribersMails = await _subscriber.GetSubscriberByCategoryIdAsync(post.CategoryId);
            if (subscribersMails.Count > 0 && messagePost.NotifiedPost is not null)
            {
                var sendEmail = await _email.GetSubscribersEmailAsync(messagePost.NotifiedPost, subscribersMails);

            }
           

            return Ok(new {
                            message = messagePost.Message 
                        }
                    );
        }
        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            var post = await _post.GetAllPost();
           if(post.Count == 0)
            {
                return NotFound(
                        new
                        {
                            message = "Post does not exist"
                        }
                    );
            }
           return Ok(post);
        }
        [HttpGet("[action]/{id:int}")]
        public async Task<IActionResult> GetPostByCategoryId([FromRoute]int id)
        {
            var specificPostList = await _post.GetPostByCategoryId(id);
            if(specificPostList.Count == 0)
            {
				return NotFound(
					   new
					   {
						   message = "Post does not exist"
					   }
				   );
			}
            return Ok(specificPostList);
        }
        [HttpGet("[action]/{userId:int}")]
          public async Task<IActionResult> GetPostByUserId([FromRoute]int userId) 
           {
                var userPosts = await _post.GetPostByUserId(userId);
                if(userPosts.Count == 0)
                {
				    return NotFound(
					       new
					       {
						       message = "Post does not exist"
					       }
				       );
			    }
                    return Ok(userPosts);
           }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetPost([FromRoute]int id)
        {
            var post = await _post.GetPostDB(id);
            if(post == null)
            {
                return NotFound(
                        new
                        {
                            message = "post does not exist"
                        }
                    );
            }
            return Ok(post);
        }
        
        [HttpPut("{id:int}")]
		[Authorize(Roles = UserRole.author)]
		public async Task<IActionResult> UpdatePost([FromRoute] int id, PostDto post)
        {
            var message = await _post.EditPost(id, post);
            
            return Ok(new
			{
				message = message
			});
        }
        [HttpDelete("{id:int}")]
		[Authorize(Roles = UserRole.author)]
		public async Task<IActionResult> DeletePost([FromRoute] int id)
        {
            var message = await _post.DeletePost(id);
            return Ok(
                    new
                    {
                        message = message
                    }
                );
        }

    }
}
