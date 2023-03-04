using System;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace DysonSphereProgram.Modding.ConstructionEnhancements
{
  [BepInAutoPlugin("dev.raptor.dsp.ConstructionEnhancements", "ConstructionEnhancements")]
  [BepInProcess("DSPGAME.exe")]
  public partial class Plugin : BaseUnityPlugin
  {
    private Harmony _harmony;
    internal static ManualLogSource Log;

    private void Awake()
    {
      Plugin.Log = Logger;
      Configuration.Initialize(Config);
      _harmony = new Harmony(Plugin.Id);
      DragBuildDistModification.ApplyPatch(_harmony);
      Logger.LogInfo("ConstructionEnhancements Awake() called");
    }

    private void OnDestroy()
    {
      Logger.LogInfo("ConstructionEnhancements OnDestroy() called");
      _harmony?.UnpatchSelf();
      Plugin.Log = null;
    }
  }
}

// namespace System.Runtime.CompilerServices
// {
//   public record IsExternalInit;
// }