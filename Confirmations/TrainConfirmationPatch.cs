using HarmonyLib;
using Mono.Cecil;

namespace AirportCEOStaffImprovements.Confirmations;



[HarmonyPatch(typeof(EmployeeContainerUI))]
internal class TrainConfirmationPatch
{
    static bool SkipDialogCheck = false;

    [HarmonyPatch(nameof(EmployeeContainerUI.TrainEmployee))]
    [HarmonyPrefix]
    internal static bool TrainConfirmation_Prefix(EmployeeController employee)
    { 
        if(!SIConfig.ShowTrainConfirmation.Value)
        {
            return true;
        }

        if (SkipDialogCheck)
        {
            return true;
        }

        DialogPanel.Instance.ShowQuestionPanel(
            (result) => TrainEmployee(result, employee),
            $"Are you sure you want to train {employee.employeeModel.FullName}?",
            true
        );


        return false;
    }

    private static void TrainEmployee(bool result, EmployeeController employee)
    {
        if (!result)
        {
            return;
        }

        SkipDialogCheck = true;
        var employeePanelUI = Singleton<EmployeePanelUI>.Instance;
        Singleton<EmployeeContainerUI>.Instance.TrainEmployee(employee, () => employeePanelUI.GenerateEmployeeContainers());
        SkipDialogCheck = false;

        if (employeePanelUI)
        {
            employeePanelUI.GenerateEmployeeContainers();
        }
    }
}
