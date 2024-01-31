using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
	public class SubscribersDto
	{
		public string Email { get; set; }
		public List<int> Category { get; set; }
	}
}
