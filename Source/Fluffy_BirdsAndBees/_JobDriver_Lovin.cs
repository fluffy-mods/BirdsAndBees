using System;
using System.Reflection;
using RimWorld;
using UnityEngine;
using Verse;

namespace Fluffy_BirdsAndBees
{
    public static class _JobDriver_Lovin
    {
        public const string ITERATOR_NAME = "<MakeNewToils>c__Iterator33";
        public const string DRIVER_FIELD_NAME = "<>f__this";
        public const string FINISH_ACTION_NAME = "<>m__96";

        const BindingFlags ALL = (BindingFlags) 60;

        private static Type driverType;
        internal static Type iterator;
        private static FieldInfo driverFieldInfo;
        private static MethodInfo partnerGetMethodInfo;
        private static MethodInfo genTicksToNextLovinMethodInfo;

        static _JobDriver_Lovin()
        {
            driverType = typeof( JobDriver_Lovin );
            if (driverType == null)
                Resources.Debug( "driverType NULL" );
            iterator = driverType.GetNestedType( ITERATOR_NAME, ALL );
            if ( iterator == null )
                Resources.Debug( "iterator NULL" );
            driverFieldInfo = iterator.GetField( DRIVER_FIELD_NAME, ALL );
            if ( driverFieldInfo == null )
                Resources.Debug( "driverFieldInfo NULL" );
            partnerGetMethodInfo = driverType.GetProperty( "Partner", ALL ).GetGetMethod( true );
            if ( partnerGetMethodInfo == null )
                Resources.Debug( "partnerGetMethodInfo NULL" );
            genTicksToNextLovinMethodInfo = driverType.GetMethod( "GenerateRandomMinTicksToNextLovin", ALL );
            if ( genTicksToNextLovinMethodInfo == null )
                Resources.Debug( "genTicksToNextLovinMethodInfo NULL" );
        }

        public static void FinishAction( object _this )
        {
            if ( _this == null )
                throw new Exception( "COuld not get iterator" );
            JobDriver_Lovin driver = driverFieldInfo.GetValue( _this ) as JobDriver_Lovin;
            if ( driver == null )
                throw new Exception( "Could not get driver" );
            Pawn partner = partnerGetMethodInfo.Invoke( driver, null ) as Pawn;
            if ( partner == null )
                throw new Exception( "Could not get partner" );

            int ticksToNextLovin = (int)genTicksToNextLovinMethodInfo.Invoke( driver, new object[] {driver.pawn} );

            // both this pawn and the partner will simultaneously perform the loving job, so we need a quasi random number that will be the same for both.
            int seed = driver.pawn.RandSeedForHour( 3 ) * partner.RandSeedForHour( 6 );
            Rand.Seed = seed;
            
            // succesful lovin
            if ( Rand.Value < driver.pawn.health.capacities.GetEfficiency( Resources.reproductionCapacityDef ) )
            {
                Thought_Memory goodLovinMemory = (Thought_Memory) ThoughtMaker.MakeThought( ThoughtDefOf.GotSomeLovin );
                goodLovinMemory.moodPowerFactor = Mathf.Max( driver.pawn.relations.AttractionTo( partner ), 0.1f );
                driver.pawn.needs.mood.thoughts.memories.TryGainMemoryThought( goodLovinMemory, partner );
            }
            // failed lovin
            else
            {
                Thought_Memory badLovinMemory = driver.pawn.gender == Gender.Female
                    ? (Thought_Memory)ThoughtMaker.MakeThought( Resources.failedLovingThoughtDef_Female )
                    : (Thought_Memory) ThoughtMaker.MakeThought( Resources.failedLovingThoughtDef_Male );
                driver.pawn.needs.mood.thoughts.memories.TryGainMemoryThought( badLovinMemory, partner );
            }
            
            driver.pawn.mindState.canLovinTick = Find.TickManager.TicksGame + ticksToNextLovin;
        }
    }
}