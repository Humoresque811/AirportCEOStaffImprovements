using HarmonyLib;

namespace AirportCEOStaffImprovements.SortingEmployees;

[HarmonyPatch(typeof(EmployeePanelUI), nameof(EmployeePanelUI.InitializePanel))]
internal class RefreshPanelOnSettingChangedPatch
{
    [HarmonyPostfix]
    static void PostfixPatch(EmployeePanelUI __instance)
    {
        SIConfig.SortOptions.SettingChanged += (s, e) =>
        {
            __instance.GenerateEmployeeContainers();
        };
        SIConfig.SortDirection.SettingChanged += (s, e) =>
        {
            __instance.GenerateEmployeeContainers();
        };
    }   
}
