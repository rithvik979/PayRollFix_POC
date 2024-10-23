using System.ComponentModel.DataAnnotations;

namespace Payrollfix_poc.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
