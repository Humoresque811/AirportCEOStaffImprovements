using HarmonyLib;

namespace AirportCEOStaffImprovements.Confirmations;



[HarmonyPatch(typeof(CandidateController))]
internal class RejectConfirmationPatch
{
    static bool SkipDialogCheck = false;

    [HarmonyPatch(nameof(CandidateController.RejectEmployee))]
    [HarmonyPrefix]
    internal static bool RejectConfirmation_Prefix(EmployeeController employee)
    {
        if (!SIConfig.ShowRejectConfirmation.Value)
        {
            return true;
        }

        if (SkipDialogCheck)
        {
            return true;
        }

        DialogPanel.Instance.ShowQuestionPanel(
            (result) => RejectEmployee(result, employee),
            $"Are you sure you want to reject {employee.employeeModel.FullName}?",
            true
        );


        return false;
    }

    private static void RejectEmployee(bool result, EmployeeController employee)
    {
        if (!result)
        {
            return;
        }

        SkipDialogCheck = true;
        Singleton<CandidateController>.Instance.RejectEmployee(employee);
        SkipDialogCheck = false;

        var employeePanelUI = Singleton<EmployeePanelUI>.Instance;

        if (employeePanelUI)
        {
            employeePanelUI.GenerateEmployeeContainers();
        }
    }

}
