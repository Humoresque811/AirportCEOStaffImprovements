using AirportCEOModLoader.WatermarkUtils;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace AirportCEOStaffImprovements;

[BepInPlugin("org.airportceostaffimprovements.humoresque", "AirportCEO Staff Improvements", PluginInfo.PLUGIN_VERSION)]
[BepInDependency("org.airportceomodloader.humoresque")]
public class AirportCEOStaffImprovements : BaseUnityPlugin
{
    public static AirportCEOStaffImprovements Instance { get; private set; }
    internal static Harmony Harmony { get; private set; }
    internal static ManualLogSource SILogger { get; private set; }
    internal static ConfigFile ConfigReference {  get; private set; }

    private void Awake()
    {
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        Harmony = new Harmony(PluginInfo.PLUGIN_GUID);
        Harmony.PatchAll(); 

        Instance = this;
        SILogger = Logger;
        ConfigReference = Config;

        // Config
        Logger.LogInfo($"{PluginInfo.PLUGIN_GUID} is setting up config.");
        SIConfig.SetUpConfig();
        Logger.LogInfo($"{PluginInfo.PLUGIN_GUID} finished setting up config.");

    }

    private void Start()
    {
        WatermarkUtils.Register(new WatermarkInfo("StI", PluginInfo.PLUGIN_VERSION, true));
    }
}
