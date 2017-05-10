#if DEBUG
#define DEBUG_JOBDRIVER_LOVIN
#endif

using System;
using System.Diagnostics;
using System.Reflection;
using Harmony;
using RimWorld;
using UnityEngine;
using UnityEngine.Assertions;
using Verse;
using static Harmony.AccessTools;

namespace Fluffy_BirdsAndBees
{
    [HarmonyPatch]
    public static class JobDriver_Lovin_MakeNewToils_FinishAction
    {
        public const string ITERATOR_NAME = "<MakeNewToils>c__Iterator32";
        public const string FINISH_ACTION_NAME = "<>m__92";
        public const string DRIVER_FIELD_NAME = "<>f__this";

        static MethodInfo TargetMethod()
        {
            var iterator = Inner( typeof( JobDriver_Lovin ), ITERATOR_NAME );
            if( iterator == null )
                throw new NullReferenceException( "iterator null" );

            var finish_action = Method( iterator, FINISH_ACTION_NAME );
            if ( finish_action == null )
                throw new NullReferenceException( "finish action null" );

            return finish_action;
        }

        [Conditional("DEBUG_JOBDRIVER_LOVIN")]
        static void Assert( object obj, string name )
        {
            Log.Message( $"{name} is { obj ?? "NULL" }" );
        }

        static bool Prefix( object __instance )
        {
            Assert(__instance, "instance");
            var traverse = Traverse.Create( __instance );
            Assert( traverse, "traverse" );
            var driver = traverse.Field( DRIVER_FIELD_NAME ).GetValue<JobDriver_Lovin>();
            Assert(driver, "driver");

            var driverTraverse = Traverse.Create( driver );
            Assert(driverTraverse, "driverTraverse");
            var partner = driverTraverse.Property( "Partner" ).GetValue<Pawn>();
            Assert(partner, "partner");

            var genTicksToNextLovin = driverTraverse.Method( "GenerateRandomMinTicksToNextLovin", driver.pawn );
            Assert( genTicksToNextLovin, "GenTicksMethod" );
            var ticksToNextLovin = genTicksToNextLovin.GetValue<int>( driver.pawn );
            Assert(ticksToNextLovin, "ticksToNextLovin"  );

            var performanceLevel = LovinUtility.LovinLevel( driver.pawn, partner );

            // apply the thought
            var thought = ThoughtMaker.MakeThought( ThoughtDefOf.LovinPerformance, performanceLevel );
            driver.pawn.needs.mood.thoughts.memories.TryGainMemory( thought, partner );

            // we're civilized, not bunnies.
            driver.pawn.mindState.canLovinTick = Find.TickManager.TicksGame + ticksToNextLovin;

            return false;
        }
    }
}