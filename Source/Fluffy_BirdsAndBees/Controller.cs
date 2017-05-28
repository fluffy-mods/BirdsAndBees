#if DEBUG
//#define DEBUG_HARMONY
#endif

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Harmony;
using RimWorld;
using Verse;
using static Fluffy_BirdsAndBees.Resources;

namespace Fluffy_BirdsAndBees
{
    public class Controller : Mod
    {
        public Controller( ModContentPack content ) : base( content ) {
            // Harmony magic
#if DEBUG_HARMONY
            HarmonyInstance.DEBUG = true;
#endif


            var harmony = HarmonyInstance.Create("Fluffy.BirdsAndBees");
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

