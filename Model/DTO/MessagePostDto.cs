using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
	public class MessagePostDto
	{
        public string Message { get; set; }
        public PostNotificationDto? NotifiedPost { get; set; }


    }
}
