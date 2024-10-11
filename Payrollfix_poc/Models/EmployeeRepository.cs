namespace Payrollfix_poc.Models
{
    public class EmployeeRepository:IEmployeeRepository
    {
        public List<Employee> DataSource()
        {
            return new List<Employee>()
            {
                new Employee() {FirstName = "Rithvik",LastName = "Reddy",EmployeeId = 1001,Email="Grithvik@gmail.com",DOB = new DateOnly(1995, 06, 04),Phone_no = "123456789",Address = "Tarnaka, Hyderabad",Gender = "Male",JoinDate = new DateOnly(2020,08,20) },
                new Employee() {FirstName = "Vijay",LastName = "Vatikuti",EmployeeId = 1002,Email="Vijay@gmail.com",DOB = new DateOnly(1995, 06, 04),Phone_no = "123456789",Address = "Tarnaka, Hyderabad",Gender = "Male",JoinDate = new DateOnly(2020,08,20) },
                new Employee() {FirstName = "Purandhar",LastName = "Kola",EmployeeId = 1003,Email="kola@gmail.com",DOB = new DateOnly(1995, 06, 04),Phone_no = "123456789",Address = "Tarnaka, Hyderabad",Gender = "Female",JoinDate = new DateOnly(2020,08,20) },
                new Employee() {FirstName = "Srinivas",LastName = "Reddy",EmployeeId = 1004,Email="srinivas@gmail.com",DOB = new DateOnly(1995, 06, 04),Phone_no = "123456789",Address = "Tarnaka, Hyderabad",Gender = "Male",JoinDate = new DateOnly(2020,08,20) }
            };
        }
        public Employee GetById(int id)
        {
            return DataSource().FirstOrDefault(e => e.EmployeeId == id) ?? new Employee();
        }
    }
}
