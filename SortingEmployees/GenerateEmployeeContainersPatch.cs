using AirportCEOStaffImprovements.SortingEmployees.Models;
using AirportCEOStaffImprovements.SortingEmployees.SortBy;
using HarmonyLib;
using System;

namespace AirportCEOStaffImprovements.SortingEmployees;

[HarmonyPatch(typeof(EmployeePanelUI), nameof(EmployeePanelUI.GenerateEmployeeContainers))]
internal class GenerateEmployeeContainersPatch
{
    [HarmonyPostfix]
    static void PostfixPatch(EmployeePanelUI __instance)
    {
        var sortBy = SIConfig.SortOptions.Value;

        // This does the function of the original method
        if (sortBy == SortByEnum.Default)
        {
            return;
        }

        try
        {
            var currentlyDisplayedEmployees = __instance.currentlyDisplayedEmployees;

            currentlyDisplayedEmployees.Sort((x, y) =>
            {
                // First, compare by EmployeeType
                var employeeTypeX = x.currentEmployee.EmployeeType.ToString();
                var employeeTypeY = y.currentEmployee.EmployeeType.ToString();

                var employeeTypeComparison = string.Compare(employeeTypeX, employeeTypeY);

                if (employeeTypeComparison != 0)
                {
                    return employeeTypeComparison;
                }


                ISortBy sorter = SortService.GetSortingStrategy(sortBy);

                return sorter.Compare(x, y);
            });

            // Update sibling indices based on the sorted order
            for (int i = 0; i < currentlyDisplayedEmployees.Count; i++)
            {
                currentlyDisplayedEmployees[i].transform.SetSiblingIndex(i);
            }
        }
        catch (Exception ex)
        {
            AirportCEOStaffImprovements.SILogger.LogError($"Error resolving sorter for type {sortBy}: {ex.Message}");
        }
    }
}

interface ISortBy
{
    SortByEnum Type { get; }


    int Compare(EmployeeContainerUI x, EmployeeContainerUI y);
}
