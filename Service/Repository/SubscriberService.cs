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
	public class SubscriberService : ISubscriber
	{
		private readonly PostDBContext _context;
        public SubscriberService(PostDBContext context)
        {
            _context = context;
        }
        public async Task<string?> AddSubscribersAsync(Subcribers subData)
		{
			if (subData == null)
			{
				return null;
			}
				
			await _context.subcribers.AddAsync(subData);
			await _context.SaveChangesAsync();
			return "subscription was success";

		}

		public async Task<List<string?>> GetSubscriberByCategoryIdAsync(int categoryId)
		{
			List<string> emails = new List<string>();
			
			var items = await _context.categories
										.Include(s => s.Subscriber)
										.Where(c => c.Id == categoryId)
										.Select(s => s.Subscriber)
										.SingleOrDefaultAsync();

			if (items.Count > 0)
			{
				foreach (var subcriber in items)
				{
					emails.Add(subcriber.Email);
				}
			}



			return emails;						
									 
		}

		public async Task<Subcribers?> GetSubscribersAsync(string email)
		{
			var subscriber = await _context.subcribers.FirstOrDefaultAsync(x => x.Email == email);
			if (subscriber == null)
			{
				return null;
			}
			return subscriber;
		}
	}
}
