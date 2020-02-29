#if DEBUG
#define DEBUG_JOBDRIVER_LOVIN
#endif

using System;
using System.Diagnostics;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;
using static HarmonyLib.AccessTools;

namespace Fluffy_BirdsAndBees
{
    [HarmonyPatch( typeof( JobDriver_Lovin ), "<MakeNewToils>b__11_4")]
    public static class JobDriver_Lovin_MakeNewToils_FinishAction
    {
        [Conditional("DEBUG_JOBDRIVER_LOVIN")]
        static void Assert( object obj, string name )
        {
            Log.Message( $"{name} is { obj ?? "NULL" }" );
        }

        static bool Prefix( JobDriver_Lovin __instance )
        {
            var driverTraverse = Traverse.Create( __instance );
            Assert(driverTraverse, "driverTraverse");
            var partner = driverTraverse.Property( "Partner" ).GetValue<Pawn>();
            Assert(partner, "partner");

            var genTicksToNextLovin = driverTraverse.Method( "GenerateRandomMinTicksToNextLovin", __instance.pawn );
            Assert( genTicksToNextLovin, "GenTicksMethod" );
            var ticksToNextLovin = genTicksToNextLovin.GetValue<int>( __instance.pawn );
            Assert(ticksToNextLovin, "ticksToNextLovin"  );

            var performanceLevel = LovinUtility.LovinLevel( __instance.pawn, partner );

            // apply the thought
            var thought = ThoughtMaker.MakeThought( ThoughtDefOf.LovinPerformance, performanceLevel );
            __instance.pawn.needs.mood.thoughts.memories.TryGainMemory( thought, partner );

            // we're civilized, not bunnies.
            __instance.pawn.mindState.canLovinTick = Find.TickManager.TicksGame + ticksToNextLovin;

            return false;
        }
    }
}