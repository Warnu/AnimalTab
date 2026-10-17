// Controller.cs
// Copyright Karel Kroeze, 2017-2017

using System.Reflection;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace AnimalTab {
    public class Controller: Mod {
        public static Settings Settings { get; private set; }

        public Controller(ModContentPack content) : base(content) {
            Settings = GetSettings<Settings>();

            // execute them patches.
#if DEBUG
            Harmony.DEBUG = true;
#endif
            Harmony harmony = new( "Fluffy.AnimalTab" );
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            Log.Message("[Animal Tab] Gatherable-status fix loaded (milk/wool null-reference guard).");

        }

        public override string SettingsCategory() {
            return "Fluffy.AnimalTab".Translate();
        }

        public override void DoSettingsWindowContents(Rect canvas) {
            Settings.DoWindowContents(canvas);
        }
    }
}
