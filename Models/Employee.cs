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
    public int Gross { get; set; }
    public int BasicSalary { get; set; }
    public int HouseAllowance { get; set; }
    public int MedicalAllowance { get; set; }
    public int TravelAllowance { get; set; }
    public int Miscellaneous { get; set; }
    public int TransportAllowance { get; set; }
    public int PerformanceAllowance { get; set; }
    public int MonthlyTaxDeduction { get; set; }
}