using HarmonyLib;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Locations;
using StardewValley.Objects;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ichortower.GrandpasStardrop
{
    internal class Patches
    {
        public static void Apply()
        {
            Harmony harmony = new(GrandpasStardrop.ModId);
            PatchMethod(harmony, typeof(StardewValley.Event).GetNestedType("DefaultCommands"),
                    "GrandpaEvaluation", //nameof(ClassToPatch.Method),
                    null,
                    nameof(Patches.GrandpaEvaluation_Postfix));
            PatchMethod(harmony, typeof(StardewValley.Event).GetNestedType("DefaultCommands"),
                    "GrandpaEvaluation2", //nameof(ClassToPatch.Method),
                    null,
                    nameof(Patches.GrandpaEvaluation_Postfix));
        }

        private static void PatchMethod(Harmony harmony, Type t, string name,
                Type[] argTypes, string patch)
        {
            string[] parts = patch.Split("_");
            string last = parts[parts.Length-1];
            if (last != "Prefix" && last != "Postfix" && last != "Transpiler") {
                Log.Error($"Skipping patch method '{patch}': bad type '{last}'");
                return;
            }
            try {
                MethodInfo m;
                if (argTypes is null) {
                    m = t.GetMethod(name,
                            BindingFlags.Public | BindingFlags.NonPublic |
                            BindingFlags.Instance | BindingFlags.Static);
                }
                else {
                    m = t.GetMethod(name,
                            BindingFlags.Public | BindingFlags.NonPublic |
                            BindingFlags.Instance | BindingFlags.Static,
                            null, argTypes, null);
                }
                HarmonyMethod func = new(typeof(Patches), patch);
                if (last == "Prefix") {
                    harmony.Patch(original: m, prefix: func);
                }
                else if (last == "Postfix") {
                    harmony.Patch(original: m, postfix: func);
                }
                else if (last == "Transpiler") {
                    harmony.Patch(original: m, transpiler: func);
                }
                Log.Trace($"Patched method '{t.Name}.{m.Name}' ({last})");
            }
            catch (Exception e) {
                Log.Error($"Patch failed ({patch}): {e}");
            }
        }

        /*
         * Applied to both GrandpaEvaluation and GrandpaEvaluation2
         */
        public static void GrandpaEvaluation_Postfix()
        {
            int candles = Utility.getGrandpaCandlesFromScore(Utility.getGrandpaScore());
            if (candles < 4) {
                return;
            }
            if (Game1.player.mailReceived.Contains("CF_Spouse")) {
                return;
            }
            FarmHouse fh = Utility.getHomeOfFarmer(Game1.player);
            Vector2 chestPosition = Utility.PointToVector2(fh.getEntryLocation()) +
                    new Vector2(0f, -1f);
            Chest chest = new Chest(new List<Item>{ItemRegistry.Create("(O)434")},
                    chestPosition, giftbox: true, 1);
            fh.overlayObjects[chestPosition] = chest;
        }

    }
}
