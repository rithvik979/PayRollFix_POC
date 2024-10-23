using System.ComponentModel.DataAnnotations;

namespace Payrollfix_poc.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Key]
        public int Id { get; set; } // Employee ID input field

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; } // Optional: for persistent login
    }

}
