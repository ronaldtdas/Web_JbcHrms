using System;

namespace Web_JbcHrms.Models;

public class Employee
{
    public int EmployeeID { get; set; }
    public required string Salutation { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public DateTime JoiningDate { get; set; }
    public required string Designation { get; set; }
    public decimal Gross { get; set; }
    public decimal BasicSalary { get; set; }
    public decimal HouseAllowance { get; set; }
    public decimal MedicalAllowance { get; set; }
    public decimal TravelAllowance { get; set; }
    public decimal Miscellaneous { get; set; }
    public decimal TransportAllowance { get; set; }
    public decimal PerformanceAllowance { get; set; }
    public decimal MonthlyTaxDeduction { get; set; }
}