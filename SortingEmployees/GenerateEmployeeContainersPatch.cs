using AirportCEOStaffImprovements.SortingEmployees.Models;
using AirportCEOStaffImprovements.SortingEmployees.SortBy;
using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AirportCEOStaffImprovements.SortingEmployees;

[HarmonyPatch(typeof(EmployeePanelUI), nameof(EmployeePanelUI.GenerateEmployeeContainers))]
internal class GenerateEmployeeContainersPatch
{

    static EmployeePanelUI Instance;

    [HarmonyPostfix]
    static void HarmonyPostfix(EmployeePanelUI __instance)
    {
        var sortBy = SIConfig.SortOptions.Value;

        // This does the function of the original method
        if (sortBy == SortByEnum.Default)
        {
            return;
        }

        // Regenerate the list to ensure that employees are sorted correctly. 
        // Without this step, an employee with a higher sorting priority may appear lower in the list 
        // simply because their name comes earlier in the alphabet.
        try
        {
            Instance = __instance;
            Instance.ClearContainers();

            List<EmployeeController> allEmployees = Singleton<AirportController>.Instance.allEmployees;
            if (allEmployees == null)
            {
                return;
            }

            allEmployees.Sort((a, b) => SortEmployees(a, b, sortBy));

            List<EmployeeController> list = GetCorrectEmployees(allEmployees);

            AddEmployeeToDisplayList(list);

        }
        catch (Exception ex)
        {
            AirportCEOStaffImprovements.SILogger.LogError($"Error resolving sorter for type {sortBy}: {ex.Message}");
        }

    }

    private static int SortEmployees(EmployeeController a, EmployeeController b, SortByEnum sortBy)
    {
        if (SIConfig.SortByEmployeeType.Value)
        {
            var employeeTypeX = a.EmployeeType.ToString();
            var employeeTypeY = b.EmployeeType.ToString();

            var employeeTypeComparison = string.Compare(employeeTypeX, employeeTypeY);

            if (employeeTypeComparison != 0)
            {
                return employeeTypeComparison;
            }
        }

        ISortBy sorter = SortService.GetSortingStrategy(sortBy);

        return sorter.Compare(a, b);
    }

    private static List<EmployeeController> GetCorrectEmployees(List<EmployeeController> allEmployees)
    {
        List<EmployeeController> list = new List<EmployeeController>();
        for (int num = 0; num < allEmployees.Count; num++)
        {
            EmployeeController employeeController = allEmployees[num];
            bool hasEmployee = !(employeeController == null);
            bool employeeIsNotCEO = employeeController.EmployeeModel.employeeType != Enums.EmployeeType.CEO;
            bool employeeIsNotFranchiseStaff = employeeController.EmployeeModel.employeeType != Enums.EmployeeType.FranchiseStaff;
            bool isNotFired = !employeeController.EmployeeModel.isFired;
            bool isNotIgnoreActivityExecution = !employeeController.EmployeeModel.ignoreActivityExecution;
            bool isBetweenSkillFilter = employeeController.EmployeeModel.skill >= Instance.minSkillFilter * 2f && employeeController.EmployeeModel.skill <= Instance.maxSkillFilter * 2f;
            bool isInCorrectOverview = (employeeController.EmployeeModel.isHired || ManagementPanelController.specificPanelTypeDisplayed != Enums.ManagementSpecificPanelType.StaffOverview) && (!employeeController.EmployeeModel.isHired || ManagementPanelController.specificPanelTypeDisplayed != Enums.ManagementSpecificPanelType.Applicants);

            bool filterCategoryIsAll = Instance.FilterCategory == Enums.EmployeeType.All;
            bool filterEmployeeTypeMatches = employeeController.EmployeeModel.employeeType == Instance.FilterCategory;
            bool filterIsExecutivesAndEmployeeIsExecutive = Instance.FilterCategory == Enums.EmployeeType.Executives && Singleton<AirportController>.Instance.IsExecutiveEmployee(employeeController.EmployeeModel.employeeType);

            bool passesFilterCategory = filterCategoryIsAll || filterEmployeeTypeMatches || filterIsExecutivesAndEmployeeIsExecutive;


            if (hasEmployee &&
                employeeIsNotCEO &&
                employeeIsNotFranchiseStaff &&
                isNotFired &&
                isNotIgnoreActivityExecution &&
                isInCorrectOverview &&
                isBetweenSkillFilter &&
                passesFilterCategory)
            {
                list.Add(employeeController);
            }
        }

        return list;
    }

    private static void AddEmployeeToDisplayList(List<EmployeeController> allViewableEmployees)
    {
        int num3 = 0;
        for (int num4 = Instance.CurrentPage * 15; num4 < allViewableEmployees.Count; num4++)
        {
            if (num3 >= 15)
            {
                break;
            }
            EmployeeController employeeController2 = allViewableEmployees[num4];
            if (!(employeeController2 == null) && Instance.pool.TryGetPoolObject(launchDirect: true, out var obj))
            {
                obj.transform.localScale = Vector3.one;
                obj.transform.SetAsLastSibling();
                obj.SetContainerValues(employeeController2, Instance.GenerateEmployeeContainers);
                Instance.currentlyDisplayedEmployees.Add(obj);
                num3++;
            }
        }
    }
}

interface ISortBy
{
    SortByEnum Type { get; }


    int Compare(EmployeeController x, EmployeeController y);
}
