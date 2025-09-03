using HarmonyLib;

namespace AirportCEOStaffImprovements.ImmediatelyAddApplication;

[HarmonyPatch(typeof(CandidateController), nameof(CandidateController.HireEmployee))]
internal class HireEmployeePatch
{
    [HarmonyPostfix]
    static void PostfixPatch(ref bool __result, CandidateController __instance)
    {
        if (!__result)
        {
            return;
        }

        __instance.StartCoroutine(__instance.ValidateApplications());
    }
}
