using HarmonyLib;

namespace AirportCEOStaffImprovements.FullyTrained;

[HarmonyPatch]
internal class FullyTrainedPatch
{

    [HarmonyPatch(typeof(EmployeeController))]
    [HarmonyPatch(nameof(EmployeeController.CanTrain), MethodType.Getter)]
    [HarmonyPostfix]
    internal static void IsFullyTrained_Postfix(ref bool __result, EmployeeController __instance)
    {
        if (!SIConfig.AllowStaffToBeFullyTrained.Value)
        {
            return;
        }

        __result = __instance.EmployeeModel.skill < 2f;
    }

    [HarmonyPatch(typeof(EmployeeController))]
    [HarmonyPatch(nameof(EmployeeController.SkillAfterTrain), MethodType.Getter)]
    internal static void Postfix(ref float __result)
    {
        if (__result > 1.99f)
        {
            __result = 2f;
        }
    }

}
