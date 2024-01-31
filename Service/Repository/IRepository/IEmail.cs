using Model;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Repository.IRepository
{
    public interface IEmail
    {
		Task<string> SendEmailAsync(UserRequestDto? request, EmailToUser? emailToUser);
		Task<string> GetSubscribersEmailAsync(PostNotificationDto? data, List<string?> subscriberEmail);
	
	}
}
