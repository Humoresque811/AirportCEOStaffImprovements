using HarmonyLib;

namespace AirportCEOStaffImprovements.FullyTrained;

[HarmonyPatch]
internal class FullyTrainedPatch
{

    [HarmonyPatch(typeof(EmployeeController))]
    [HarmonyPatch(nameof(EmployeeController.CanTrain), MethodType.Getter)]
    [HarmonyPrefix]
    internal static bool CanTrain_Prefix(ref bool __result, EmployeeController __instance)
    {
        if (!SIConfig.AllowStaffToBeFullyTrained.Value)
        {
            // If setting is disabled 
            return true;
        }

        // Needs to be less than 1.99 to make sure it shows up in the container
        // See function EmployeePanelUI.GenerateEmployeeContainers
        __result = __instance.EmployeeModel.skill < 1.99f;
        return false;
    }
}
