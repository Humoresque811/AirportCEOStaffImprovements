using HarmonyLib;

namespace AirportCEOStaffImprovements.Confirmations;



[HarmonyPatch(typeof(CandidateController))]
internal class HireConfirmationPatch
{
    static bool SkipDialogCheck = false;

    [HarmonyPatch(nameof(CandidateController.HireEmployee))]
    [HarmonyPrefix]
    internal static bool HireConfirmation_Prefix(EmployeeController employee)
    {
        if (!SIConfig.ShowHireConfirmation.Value)
        {
            return true;
        }

        if (SkipDialogCheck)
        {
            return true;
        }

        DialogPanel.Instance.ShowQuestionPanel(
            (result) => HireEmployee(result, employee),
            $"Are you sure you want to hire {employee.employeeModel.FullName}?",
            true
        );


        return false;
    }

    private static void HireEmployee(bool result, EmployeeController employee)
    {
        if (!result)
        {
            return;
        }

        SkipDialogCheck = true;
        Singleton<CandidateController>.Instance.HireEmployee(employee);
        SkipDialogCheck = false;

        var employeePanelUI = Singleton<EmployeePanelUI>.Instance;

        if (employeePanelUI)
        {
            employeePanelUI.GenerateEmployeeContainers();
        }
    }

}
