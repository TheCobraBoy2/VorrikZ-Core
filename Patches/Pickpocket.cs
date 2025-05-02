using HarmonyLib;
using Il2CppScheduleOne.UI;

namespace VorrikZ_Core.Patches
{
    public static class Pickpocket
    {
        [HarmonyPatch(typeof(PickpocketScreen), "Update")]
        [HarmonyPostfix]
        private static void PatchPickpockUpdate(PickpocketScreen __instance)
        {
            if (__instance.IsOpen && __instance.isSliding)
            {
                try
                {
                    for (int i = 0; i < __instance.Slots.Count; i++)
                    {
                        __instance.SetSlotLocked(i, false);
                    }
                }
                catch { }
            }
        }
    }
}
