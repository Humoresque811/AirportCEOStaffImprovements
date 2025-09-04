using HarmonyLib;

namespace AirportCEOStaffImprovements.SyncStaffFilters;

[HarmonyPatch(typeof(EmployeePanelUI))]
internal class FilterDropdownPatch
{
    [HarmonyPatch(nameof(EmployeePanelUI.FilterCategory), MethodType.Setter)]
    [HarmonyPostfix]
    static void SetFilterCategoryPatch(EmployeePanelUI __instance)
    {
        var currentValue = __instance.FilterCategory;

        __instance.employeeSearchCategory = currentValue;
        __instance.applicantSearchCategory = currentValue;
    }

    [HarmonyPatch(nameof(EmployeePanelUI.FilterValue), MethodType.Setter)]
    [HarmonyPostfix]
    static void SetFilterValuePatch(EmployeePanelUI __instance)
    {
        var currentValue = __instance.FilterValue;

        __instance.employeeFilterValue = currentValue;
        __instance.applicantFilterValue = currentValue;
    }
}
