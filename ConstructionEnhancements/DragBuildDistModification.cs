using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;

namespace DysonSphereProgram.Modding.ConstructionEnhancements;

public static class DragBuildDistModification
{
    public static readonly Dictionary<int, Vector2> draggablePatchData = new();

    public const int itemIdSplitter = 2020;
    public const int itemIdTeslaTower = 2201;
    public const int itemIdWirelessPowerTower = 2202;
    public const int itemIdSatelliteSubstation = 2212;
    public const int itemIdPLS = 2103;
    public const int itemIdILS = 2104;

    static void Initialize()
    {
        if (Configuration.dragBuildDistSplitterEnabled)
            draggablePatchData[itemIdSplitter] = Configuration.dragBuildDistSplitter;
        if (Configuration.dragBuildDistTeslaTowerEnabled)
            draggablePatchData[itemIdTeslaTower] = Configuration.dragBuildDistTeslaTower;
        if (Configuration.dragBuildDistWirelessPowerTowerEnabled)
            draggablePatchData[itemIdWirelessPowerTower] = Configuration.dragBuildDistWirelessPowerTower;
        if (Configuration.dragBuildDistSatelliteSubstationEnabled)
            draggablePatchData[itemIdSatelliteSubstation] = Configuration.dragBuildDistSatelliteSubstation;
        if (Configuration.dragBuildDistPLSEnabled)
            draggablePatchData[itemIdPLS] = Configuration.dragBuildDistPLS;
        if (Configuration.dragBuildDistILSEnabled)
            draggablePatchData[itemIdILS] = Configuration.dragBuildDistILS;
    }

    public static void ApplyPatch(Harmony harmony)
    {
        Initialize();
        harmony.PatchAll(typeof(DragBuildDistModification));
        if (VFPreload.done)
            MakeDraggable();
      
        Plugin.Log.LogDebug(nameof(DragBuildDistModification) + " Patch applied");
    }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(ItemProto), nameof(ItemProto.InitItemIds))]
    static void MakeDraggable()
    {
        foreach (var kvp in draggablePatchData)
        {
            var proto = LDB.items.Select(kvp.Key);
            proto.prefabDesc.dragBuild = true;
            proto.prefabDesc.dragBuildDist = kvp.Value;
        }
    }

    private static float prefixGap;

    [HarmonyPrefix]
    [HarmonyPatch(typeof(BuildTool_Click), nameof(BuildTool_Click.DeterminePreviews))]
    static void FixGap_Prefix(ref BuildTool_Click __instance)
    {
        prefixGap = __instance.gap;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(BuildTool_Click), nameof(BuildTool_Click.DeterminePreviews))]
    static void FixGap_Postfix(ref BuildTool_Click __instance)
    {
        if (__instance.cursorValid)
        {
            if (VFInput._switchModelStyle.onDown)
            {
                if (__instance.handItem.ModelCount > 1)
                {
                }
                else if (__instance.isDragging)
                {
                    // Modify tabGapDir and gap
                }
            }

            if (__instance.isDragging)
            {
                if (VFInput._cursorPlusKey.onDown)
                {
                    // Increase gap beyond 3.5f
                    if (prefixGap == __instance.gap)
                        __instance.gap += 1f;
                }
                if (VFInput._cursorMinusKey.onDown)
                {
                    // Decrease gap below 0f?
                    if (prefixGap == __instance.gap)
                        __instance.gap -= 1f;
                }
            }
        }
    }
}