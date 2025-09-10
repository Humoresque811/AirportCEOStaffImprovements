using HarmonyLib;

namespace AirportCEOStaffImprovements.SortingEmployees;

[HarmonyPatch(typeof(EmployeePanelUI), nameof(EmployeePanelUI.InitializePanel))]
internal class RefreshPanelOnSettingChangedPatch
{
    [HarmonyPostfix]
    static void PostfixPatch(EmployeePanelUI __instance)
    {
        SIConfig.SortByEmployeeType.SettingChanged += (s, e) =>
        {
            RefreshPanelOnSettingChanged(__instance);
        };
        SIConfig.SortOptions.SettingChanged += (s, e) =>
        {
            RefreshPanelOnSettingChanged(__instance);
        };
        SIConfig.SortDirection.SettingChanged += (s, e) =>
        {
            RefreshPanelOnSettingChanged(__instance);
        };
    }   

    private static void RefreshPanelOnSettingChanged(EmployeePanelUI instance)
    {
        if(!instance)
        {
            return;
        }

        instance.GenerateEmployeeContainers();
    }
}
