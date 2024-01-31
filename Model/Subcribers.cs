using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
	public class Subcribers
	{
		[Key]
        public Guid Id { get; set; }
        public string Email { get; set; }
        public List<Category> Category { get; set; }
    }
}
