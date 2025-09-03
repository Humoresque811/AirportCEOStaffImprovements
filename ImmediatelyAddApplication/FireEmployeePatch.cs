using HarmonyLib;

namespace AirportCEOStaffImprovements.ImmediatelyAddApplication;

[HarmonyPatch(typeof(CandidateController), nameof(CandidateController.RejectEmployee))]
internal class FireEmployeePatch
{
    [HarmonyPostfix]
    static void PostfixPatch()
    {
        var instance = Singleton<CandidateController>.instance;

        instance.StartCoroutine(instance.ValidateApplications());
    }
}
