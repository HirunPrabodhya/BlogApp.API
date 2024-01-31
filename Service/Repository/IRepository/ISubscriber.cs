using Model;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Repository.IRepository
{
	public interface ISubscriber
	{
		Task<string?> AddSubscribersAsync(Subcribers subData);
		Task<Subcribers?> GetSubscribersAsync(string email);
		Task<List<string?>> GetSubscriberByCategoryIdAsync(int categoryId);
	}
}
