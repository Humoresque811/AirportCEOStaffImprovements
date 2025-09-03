using HarmonyLib;

namespace AirportCEOStaffImprovements.ImmediatelyAddApplication;

[HarmonyPatch(typeof(CandidateController), nameof(CandidateController.HireEmployee))]
internal class HireEmployeePatch
{
    [HarmonyPostfix]
    static void PostfixPatch(ref bool __result)
    {
        if (!__result)
        {
            return;
        }

        var instance = Singleton<CandidateController>.instance;

        instance.StartCoroutine(instance.ValidateApplications());
    }
}
