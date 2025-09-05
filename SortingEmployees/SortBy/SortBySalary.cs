using AirportCEOStaffImprovements.SortingEmployees.Models;
using AirportCEOStaffImprovements.SortingEmployees.Util;

namespace AirportCEOStaffImprovements.SortingEmployees.SortBy;

internal class SortBySalary: ISortBy
{
    public SortByEnum Type => SortByEnum.Salary;

    public int Compare(EmployeeController x, EmployeeController y)
    {
        var skillX = x.employeeModel.salary;
        var skillY = y.employeeModel.salary;

        return CompareFunctions.CompareFloat(skillX, skillY);
    }
}
