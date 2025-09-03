using HarmonyLib;

namespace AirportCEOStaffImprovements.ImmediatelyAddApplication;

[HarmonyPatch(typeof(CandidateController), nameof(CandidateController.RejectEmployee))]
internal class FireEmployeePatch
{
    [HarmonyPostfix]
    static void PostfixPatch(CandidateController __instance)
    {
        __instance.StartCoroutine(__instance.ValidateApplications());
    }
}
