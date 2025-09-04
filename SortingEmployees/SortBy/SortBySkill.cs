using AirportCEOStaffImprovements.SortingEmployees.Models;
using AirportCEOStaffImprovements.SortingEmployees.Util;

namespace AirportCEOStaffImprovements.SortingEmployees.SortBy;

internal class SortBySkill : ISortBy
{
    public SortByEnum Type => SortByEnum.Skill;

    public int Compare(EmployeeContainerUI x, EmployeeContainerUI y)
    {
        var skillX = x.currentEmployee.employeeModel.skill;
        var skillY = y.currentEmployee.employeeModel.skill;

        return CompareFunctions.CompareFloat(skillX, skillY);
    }
}
