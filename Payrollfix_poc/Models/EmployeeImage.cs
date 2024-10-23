namespace Payrollfix_poc.Models
{
    public class EmployeeImage
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public byte[] Image { get; set; }
        public string ImageName { get; set; }
        public string ContentType { get; set; }
    }

}
