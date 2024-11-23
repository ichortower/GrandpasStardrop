using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace ichortower.GrandpasStardrop
{
    internal sealed class GrandpasStardrop : Mod
    {
        public static GrandpasStardrop instance;
        public static string ModId;

        public override void Entry(IModHelper helper)
        {
            instance = this;
            ModId = instance.ModManifest.UniqueID;
            Patches.Apply();
        }
    }

    internal class Log
    {
        public static void Trace(string text) {
            GrandpasStardrop.instance.Monitor.Log(text, LogLevel.Trace);
        }
        public static void Debug(string text) {
            GrandpasStardrop.instance.Monitor.Log(text, LogLevel.Debug);
        }
        public static void Info(string text) {
            GrandpasStardrop.instance.Monitor.Log(text, LogLevel.Info);
        }
        public static void Warn(string text) {
            GrandpasStardrop.instance.Monitor.Log(text, LogLevel.Warn);
        }
        public static void Error(string text) {
            GrandpasStardrop.instance.Monitor.Log(text, LogLevel.Error);
        }
        public static void Alert(string text) {
            GrandpasStardrop.instance.Monitor.Log(text, LogLevel.Alert);
        }
        public static void Verbose(string text) {
            GrandpasStardrop.instance.Monitor.VerboseLog(text);
        }
    }
}
