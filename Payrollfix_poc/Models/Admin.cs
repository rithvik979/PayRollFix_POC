﻿using System.ComponentModel.DataAnnotations;

namespace Payrollfix_poc.Models
{
	public class Admin
	{
		[Key]
		public int AdminId { get; set; }
		[Required]
		public string Password { get; set; }
	}
}
