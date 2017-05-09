#if DEBUG
#define DEBUG_JOBDRIVER_LOVIN
#endif

using System;
using System.Diagnostics;
using System.Reflection;
using Harmony;
using RimWorld;
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

            // both this pawn and the partner will simultaneously perform the loving job, so we need a quasi random number that will be the same for both.
            int seed = driver.pawn.RandSeedForHour(3) * partner.RandSeedForHour(6);
            Rand.Seed = seed;
            Assert( seed, "seed" );

            // succesful lovin
            if (Rand.Value < driver.pawn.health.capacities.GetLevel(PawnCapacityDefOf.Reproduction))
            {
                Thought_Memory goodLovinMemory = (Thought_Memory)ThoughtMaker.MakeThought(RimWorld.ThoughtDefOf.GotSomeLovin);
                // TODO: figure out where attraction is hiding in A16, and if we want to add it back in as a mood factor.
                // goodLovinMemory.moodPowerFactor = Mathf.Max( driver.pawn.relations.AttractionTo( partner ), 0.1f );
                driver.pawn.needs.mood.thoughts.memories.TryGainMemory(goodLovinMemory, partner);
            }
            // failed lovin
            else
            {
                Thought_Memory badLovinMemory = driver.pawn.gender == Gender.Female
                    ? (Thought_Memory)ThoughtMaker.MakeThought(ThoughtDefOf.FailedLovinFemale)
                    : (Thought_Memory)ThoughtMaker.MakeThought(ThoughtDefOf.FailedLovinMale);
                driver.pawn.needs.mood.thoughts.memories.TryGainMemory(badLovinMemory, partner);
            }

            driver.pawn.mindState.canLovinTick = Find.TickManager.TicksGame + ticksToNextLovin;

            return false;
        }
    }
}