﻿using System.ComponentModel.DataAnnotations;

namespace Payrollfix_poc.Models
{
	public class LoginViewModel
	{
		[Required]
		[Key]
		public int EmployeeId { get; set; } // Employee ID input field

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		public bool RememberMe { get; set; } // Optional: for persistent login
	}

}
