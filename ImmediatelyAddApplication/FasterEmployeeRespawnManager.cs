using HarmonyLib;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using static Enums.EmployeeType;

namespace AirportCEOStaffImprovements.ImmediatelyAddApplication;

[HarmonyPatch(typeof(CandidateController))]
internal class FasterEmployeeRespawnManager
{
    internal static void GenerateEmployee(Enums.EmployeeType employeeType)
    {
        int maxNumberOfEmployeesToGenerate = employeeType switch
        {
            CIO or HRDirector or ProcurementDirector or StrategyDirector or CFO or COO => 2,
            _ => SIConfig.MaxStaffGenerated.Value,
        };

        bool hasCommercialLicense = Singleton<ProgressionManager>.Instance.GetProjectCompletionStatus(Enums.SpecificProjectType.CommercialLicense);
        bool shouldGenerateEmployee;
        if (maxNumberOfEmployeesToGenerate == 2)
        {
            shouldGenerateEmployee = true; // this is a way to catch all the admins
        }
        else
        {
            shouldGenerateEmployee = employeeType switch
            {
                PassengerServiceAgent => hasCommercialLicense && CandidateController.Instance.generateAirportStaff,
                SecurityOfficer => hasCommercialLicense && CandidateController.Instance.generateSecurity,
                RampAgent => hasCommercialLicense && CandidateController.Instance.generateRampAgent,
                Janitor => CandidateController.Instance.generateJanitor,
                ServiceTechnician => CandidateController.Instance.generateServiceTechnician,
                Admin => CandidateController.Instance.generateAdmin,
                _ => true,
            };
        }

        if (!shouldGenerateEmployee)
        {
            return;
        }

        System.Random rand = new System.Random();
        for (int i = 0; i < maxNumberOfEmployeesToGenerate - NumberOfPotentialEmployees(employeeType); i++)
        {
            TimeSpan delay = new TimeSpan(0, 0, 0, 0, Mathf.RoundToInt(SIConfig.StaffRespawnSpeed.Value * 1000) + rand.Next(-100, 500)); // Add preset time plus an extra -0.1 to 0.5 seconds
            CandidateController.Instance.StartCoroutine(GenerateEmployeeDelayed(employeeType, maxNumberOfEmployeesToGenerate, delay));
        }
    }

    private static IEnumerator GenerateEmployeeDelayed(Enums.EmployeeType employeeType, int maxNumberOfEmployeesToGenerate, TimeSpan delay)
    {
        yield return new WaitForSeconds(delay.Milliseconds / 1000f);

        if (NumberOfPotentialEmployees(employeeType) < maxNumberOfEmployeesToGenerate)
        {
            CandidateController.Instance.GenerateCandidate(employeeType);
            CandidateController.Instance.Refresh(true);
        }
    }

    private static int NumberOfPotentialEmployees(Enums.EmployeeType employeeType)
    {
        return Singleton<AirportController>.Instance.allEmployees.Where(emp => emp.EmployeeType == employeeType && !emp.employeeModel.isHired && !emp.employeeModel.isFired).Count();

    }

    [HarmonyPatch(nameof(CandidateController.RejectEmployee))]
    [HarmonyPostfix]
    internal static void FireEmployeePatch(CandidateController __instance, EmployeeController employee)
    {
        GenerateEmployee(employee.EmployeeType);
    }

    [HarmonyPatch(nameof(CandidateController.HireEmployee))]
    [HarmonyPostfix]
    internal static void HireEmployeePatch(ref bool __result, CandidateController __instance, EmployeeController employee)
    {
        if (!__result)
        {
            return;
        }

        GenerateEmployee(employee.EmployeeType);
    }
}
