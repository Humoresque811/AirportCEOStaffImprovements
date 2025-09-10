using HarmonyLib;

namespace AirportCEOStaffImprovements.Confirmations;



[HarmonyPatch(typeof(CandidateController))]
internal class FireConfirmationPatch
{
    static bool SkipDialogCheck = false;

    [HarmonyPatch(nameof(CandidateController.FireEmployee))]
    [HarmonyPrefix]
    internal static bool FireConfirmation_Prefix(EmployeeController employee)
    {
        if (!SIConfig.ShowFireConfirmation.Value)
        {
            return true;
        }

        if (SkipDialogCheck)
        {
            return true;
        }

        DialogPanel.Instance.ShowQuestionPanel(
            (result) => FireEmployee(result, employee),
            $"Are you sure you want to fire {employee.employeeModel.FullName}?",
            true
        );


        return false;
    }

    private static void FireEmployee(bool result, EmployeeController employee)
    {
        if (!result)
        {
            return;
        }

        SkipDialogCheck = true;
        Singleton<CandidateController>.Instance.FireEmployee(employee);
        SkipDialogCheck = false;

        var employeePanelUI = Singleton<EmployeePanelUI>.Instance;

        if (employeePanelUI)
        {
            employeePanelUI.GenerateEmployeeContainers();
        }
    }

}
