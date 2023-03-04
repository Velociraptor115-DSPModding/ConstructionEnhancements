using System;
using BepInEx.Configuration;
using UnityEngine;

namespace DysonSphereProgram.Modding.ConstructionEnhancements;

public static class Configuration
{
    private const string DragBuildDistSection = "Drag-Build Distances";
    private static ConfigEntry<string> dragBuildDistSplitterConf;
    private static ConfigEntry<string> dragBuildDistTeslaTowerConf;
    private static ConfigEntry<string> dragBuildDistWirelessPowerTowerConf;
    private static ConfigEntry<string> dragBuildDistSatelliteSubstationConf;
    private static ConfigEntry<string> dragBuildDistPLSConf;
    private static ConfigEntry<string> dragBuildDistILSConf;

    public static bool dragBuildDistSplitterEnabled;
    public static bool dragBuildDistTeslaTowerEnabled;
    public static bool dragBuildDistWirelessPowerTowerEnabled;
    public static bool dragBuildDistSatelliteSubstationEnabled;
    public static bool dragBuildDistPLSEnabled;
    public static bool dragBuildDistILSEnabled;
    public static Vector2 dragBuildDistSplitter;
    public static Vector2 dragBuildDistTeslaTower;
    public static Vector2 dragBuildDistWirelessPowerTower;
    public static Vector2 dragBuildDistSatelliteSubstation;
    public static Vector2 dragBuildDistPLS;
    public static Vector2 dragBuildDistILS;

    public static void Initialize(ConfigFile confFile)
    {
        dragBuildDistSplitterConf = confFile.BindDragBuildDist("Splitter", "true, 2.5, 2.5", out dragBuildDistSplitterEnabled, out dragBuildDistSplitter);
        dragBuildDistTeslaTowerConf = confFile.BindDragBuildDist("Tesla Tower", "true, 10, 10", out dragBuildDistTeslaTowerEnabled, out dragBuildDistTeslaTower);
        dragBuildDistWirelessPowerTowerConf = confFile.BindDragBuildDist("Wireless Power Tower", "true, 3.5, 3.5", out dragBuildDistWirelessPowerTowerEnabled, out dragBuildDistWirelessPowerTower);
        dragBuildDistSatelliteSubstationConf = confFile.BindDragBuildDist("Satellite Substation", "true, 26, 26", out dragBuildDistSatelliteSubstationEnabled, out dragBuildDistSatelliteSubstation);
        dragBuildDistPLSConf = confFile.BindDragBuildDist("Planetary Logistics Station", "false, 15, 15", out dragBuildDistPLSEnabled, out dragBuildDistPLS);
        dragBuildDistILSConf = confFile.BindDragBuildDist("Interstellar Logistics Station", "false, 29, 29", out dragBuildDistILSEnabled, out dragBuildDistILS);
    }

    private static string ToConfString(this Vector2 value)
    {
        return $"{value.x}, {value.y}";
    }

    private static readonly string[] commaSeparator = { "," };
    private static bool TryParse(string value, out bool enabled, out Vector2 vector)
    {
        var values = value.Split(commaSeparator, 3, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < values.Length; i++)
            values[i] = values[i].Trim();
        float x = 0, y = 0;
        enabled = false;
        bool parsed = false;

        if (values.Length >= 1)
            parsed = bool.TryParse(values[0], out enabled);

        if (parsed)
        {
            if (values.Length >= 2)
                parsed = float.TryParse(values[1], out x);
        
            if (values.Length >= 3)
                parsed = float.TryParse(values[2], out y);
            else
                y = x;
        }

        vector = new Vector2(x, y);
        return parsed;
    }

    private static ConfigEntry<string> BindDragBuildDist(this ConfigFile confFile, string key, string defaultValue, out bool enabled, out Vector2 vector)
    {
        var entry = confFile.Bind(DragBuildDistSection, key, defaultValue, ConfigDescription.Empty);
        if (TryParse(entry.Value, out enabled, out vector))
            return entry;
        
        entry.Value = defaultValue;
        TryParse(defaultValue, out enabled, out vector);
        
        return entry;
    }
}