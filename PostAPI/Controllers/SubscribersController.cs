using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.DTO;
using Service.Repository.IRepository;
using System.Security.Cryptography.X509Certificates;

namespace PostAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SubscribersController : ControllerBase
	{
		private readonly ICategory _category;
		private readonly ISubscriber _subscriber;
		
        public SubscribersController(ICategory category, ISubscriber subscriber)
        {
            _category = category;
			_subscriber = subscriber;	
		
        }
        [HttpPost]
		public async Task<IActionResult> AddSubscribers([FromBody]SubscribersDto subdata)
		{
			if(subdata == null)
			{
				return BadRequest(new
				{
					message = "please enter all data"
				});
			}
			var isExistSub = await _subscriber.GetSubscribersAsync(subdata.Email);
			if(isExistSub != null)
			{
				return BadRequest(new { 
						message = "you already subscribed"
				});
			}
			var subscribers = new Subcribers
			{
				Email = subdata.Email,
				Category = new List<Category>(),
			};
			foreach(var categoryId in subdata.Category)
			{
				var existCategory = await _category.GetCategoryById(categoryId);
				if(existCategory != null)
				{
					subscribers.Category.Add(existCategory);
				}
			}
			var message = await _subscriber.AddSubscribersAsync(subscribers); 

			return Ok(new
			{
				message = message
			});
		}

        [HttpGet("{categoryId:int}")]
        public async Task<IActionResult> GetSubscriberByCategoryId(int categoryId) 
		{
			var test = await _subscriber.GetSubscriberByCategoryIdAsync(categoryId);
			return Ok(test);
		}
    }
}
