using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Payrollfix_poc.Models
{
    public class Position
    {
        public int PositionId { get; set; }

        [Required]
        public string PositionDescription { get; set; }
    }
}
