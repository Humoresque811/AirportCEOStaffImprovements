using AirportCEOStaffImprovements.SortingEmployees.Models;
using AirportCEOStaffImprovements.SortingEmployees.Util;

namespace AirportCEOStaffImprovements.SortingEmployees.SortBy;

internal class SortBySalary: ISortBy
{
    public SortByEnum Type => SortByEnum.Salary;

    public int Compare(EmployeeContainerUI x, EmployeeContainerUI y)
    {
        var skillX = x.currentEmployee.employeeModel.salary;
        var skillY = y.currentEmployee.employeeModel.salary;

        return CompareFunctions.CompareFloat(skillX, skillY);
    }
}
