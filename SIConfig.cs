using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportCEOStaffImprovements;

internal class SIConfig
{
    internal static ConfigEntry<bool> UseTrainAllButton { get; private set; }
    internal static ConfigEntry<float> StaffRespawnSpeed { get; private set; }
    internal static ConfigEntry<int> MaxStaffGenerated { get; private set; }

    internal static void SetUpConfig()
    {
        UseTrainAllButton = AirportCEOStaffImprovements.ConfigReference.Bind("General", "Create Train All Button", true, "Create a button to train all staff with.");
        StaffRespawnSpeed = AirportCEOStaffImprovements.ConfigReference.Bind("General", "Staff Respawn Speed", 1f, 
            new ConfigDescription("Around how fast should staff respawn?", new AcceptableValueRange<float>(0.5f, 5f)));
        MaxStaffGenerated = AirportCEOStaffImprovements.ConfigReference.Bind("General", "Max Staff Generated", 8, 
            new ConfigDescription("The max number of staff generated for hiring at a given time, per type", new AcceptableValueRange<int>(8, 16)));
    }
}
