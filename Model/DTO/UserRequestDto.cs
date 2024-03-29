﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
	public class UserRequestDto
	{
        public string FirstName { get; set; }
		public string LastName { get; set; }
        public string Email { get; set; }
        public string Bio { get; set; }
        public IFormFile CvFile { get; set; }
    }
}
