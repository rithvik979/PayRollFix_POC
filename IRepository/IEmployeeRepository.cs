using System;
using Payrollfix_poc.Models;

namespace Payrollfix_poc.IRepository
{
    public interface IEmployeeRepository
    {
        Employee GetEmployeeDetails(int id);

        Employee GetEmployeeById(int id);
    }
}