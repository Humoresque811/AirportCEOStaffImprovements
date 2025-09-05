using AirportCEOStaffImprovements.SortingEmployees.Models;
using AirportCEOStaffImprovements.SortingEmployees.Util;

namespace AirportCEOStaffImprovements.SortingEmployees.SortBy;

internal class SortBySkill : ISortBy
{
    public SortByEnum Type => SortByEnum.Skill;

    public int Compare(EmployeeController x, EmployeeController y)
    {
        var skillX = x.employeeModel.skill;
        var skillY = y.employeeModel.skill;

        return CompareFunctions.CompareFloat(skillX, skillY);
    }
}
