#if DEBUG
//#define DEBUG_HARMONY
#endif

using System.Reflection;
using HarmonyLib;
using Verse;

namespace Fluffy_BirdsAndBees
{
    public class Controller : Mod
    {
        public Controller( ModContentPack content ) : base( content ) {
            // Harmony magic
#if DEBUG_HARMONY
            HarmonyInstance.DEBUG = true;
#endif


            var harmony = new Harmony("Fluffy.BirdsAndBees");
            // patches;
            // HediffGiver.TryApply()
            // PawnUtility.FertileMateTarget()
            // PawnUtility.Mated()
            // JobGiver_mate.TryGiveJob()
            // JobGiver_DoLovin.TryGiveJob()
            // JobDriver_DoLovin.MakeNewToils() => finishAction of final toil !FRAGILE!
            harmony.PatchAll( Assembly.GetExecutingAssembly() );


        }
    }
}

