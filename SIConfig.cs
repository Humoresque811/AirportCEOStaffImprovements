using AirportCEOStaffImprovements.SortingEmployees.Models;
using BepInEx.Configuration;

namespace AirportCEOStaffImprovements;

internal class SIConfig
{
    internal static ConfigEntry<bool> UseTrainAllButton { get; private set; }
    internal static ConfigEntry<float> StaffRespawnSpeed { get; private set; }
    internal static ConfigEntry<int> MaxStaffGenerated { get; private set; }


    internal static ConfigEntry<bool> SyncStaffFilters { get; private set; }

    internal static ConfigEntry<bool> SortByEmployeeType { get; private set; }
    internal static ConfigEntry<SortByEnum> SortOptions { get; private set; }
    internal static ConfigEntry<SortDirectionEnum> SortDirection { get; private set; }

    internal static ConfigEntry<bool> AllowStaffToBeFullyTrained { get; private set; }

    internal static ConfigEntry<bool> ShowHireConfirmation { get; private set; }
    internal static ConfigEntry<bool> ShowFireConfirmation { get; private set; }
    internal static ConfigEntry<bool> ShowTrainConfirmation { get; private set; }
    internal static ConfigEntry<bool> ShowRejectConfirmation { get; private set; }


    internal static void SetUpConfig()
    {
        UseTrainAllButton = AirportCEOStaffImprovements.ConfigReference.Bind("General", "Create Train All Button", true, "Create a button to train all staff with.");

        AllowStaffToBeFullyTrained = AirportCEOStaffImprovements.ConfigReference.Bind("General", "Allow Staff To Be Fully Trained", true, "Allow staff to be fully trained instead of only 3 times");

        StaffRespawnSpeed = AirportCEOStaffImprovements.ConfigReference.Bind("General Applicants", "Staff Respawn Speed", 1f,
            new ConfigDescription("Around how fast should staff respawn?", new AcceptableValueRange<float>(0.5f, 5f)));
        MaxStaffGenerated = AirportCEOStaffImprovements.ConfigReference.Bind("General Applicants", "Max Staff Generated", 8,
            new ConfigDescription("The max number of staff generated for hiring at a given time, per type", new AcceptableValueRange<int>(8, 16)));

        SyncStaffFilters = AirportCEOStaffImprovements.ConfigReference.Bind("General Filters", "Sync Staff Filters", true, "Sync the staff filters between the staff and applicants screens");

        SortByEmployeeType = AirportCEOStaffImprovements.ConfigReference.Bind("General Sorting", "Sort By Employee Type", true, "Sort staff and applicants by their employee type");
        SortOptions = AirportCEOStaffImprovements.ConfigReference.Bind("General Sorting", "Sort Options", SortByEnum.Skill, "Sort staff and applicants by skill when hiring");
        SortDirection = AirportCEOStaffImprovements.ConfigReference.Bind("General Sorting", "Sort Direction", SortDirectionEnum.Descending, "Ascending means low to hight, Descending means high to low");


        ShowHireConfirmation = AirportCEOStaffImprovements.ConfigReference.Bind("General Confirmations", "Show Hire Confirmation", false, "Show a confirmation dialog when hiring an employee");
        ShowFireConfirmation = AirportCEOStaffImprovements.ConfigReference.Bind("General Confirmations", "Show Fire Confirmation", true, "Show a confirmation dialog when firing an employee");
        ShowTrainConfirmation = AirportCEOStaffImprovements.ConfigReference.Bind("General Confirmations", "Show Train Confirmation", false, "Show a confirmation dialog when training an employee");
        ShowRejectConfirmation = AirportCEOStaffImprovements.ConfigReference.Bind("General Confirmations", "Show Reject Confirmation", true, "Show a confirmation dialog when rejecting an employee");
    }
}
