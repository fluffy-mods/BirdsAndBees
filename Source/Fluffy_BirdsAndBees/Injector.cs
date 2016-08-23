using System;
using System.Linq;
using System.Reflection;
using CommunityCoreLibrary;
using RimWorld;
using UnityEngine;
using Verse;

namespace Fluffy_BirdsAndBees
{
    public class Injector : SpecialInjector
    {
        public override bool Inject()
        {
            // prepare body part record
            // NOTE: This would normally be read from the body def.
            Resources.Debug( "Start injection" );

            Resources.Debug( "Find fleshies" );
            // find all fleshy body types.
            var fleshRaces = DefDatabase<ThingDef>
                .AllDefsListForReading
                .Where( t => t.race?.IsFlesh ?? false );

            var fleshBodies = fleshRaces
                .Select( t => t.race.body )
                .Distinct();

            var fleshThinktrees = fleshRaces
                .Select( r => r.race.thinkTreeMain )
                .Distinct();

            var fleshHumanoids = fleshRaces
                .Where( r => r.race.Humanlike );

            // insert neuter recipe 
            Resources.Debug( "Insert recipes" );
            foreach ( ThingDef race in fleshRaces )
            {
                Resources.Debug( race.defName, 1 );
                race.recipes.Add( Resources.neuterRecipeDef );
            }

            // insert reproductive parts and recreate body cache
            Resources.Debug( "Insert parts" );
            foreach ( BodyDef body in fleshBodies )
            {
                Resources.Debug( body.defName, 1 );
                // insert body part
                body.corePart.parts.Add( Resources.reproductiveOrganRecord );
                Resources.Debug( "Inserted part", 2 );

                // force recache
                body.ResolveReferences();
                Resources.Debug( "Resolves", 2 );
            }

            // replace thinktree references to JobGiver_Mate and JobGiver_Lovin
            Resources.Debug( "Replace JobDriver_Mate & JobDriver_Lovin" );
            foreach ( ThinkTreeDef thinktree in fleshThinktrees )
            {
                Resources.Debug( thinktree.defName, 1 );
                thinktree.thinkRoot.ReplaceThinkNodeClass( typeof( RimWorld.JobGiver_Mate ),
                                                           typeof( Fluffy_BirdsAndBees.JobGiver_Mate ) );
                thinktree.thinkRoot.ReplaceThinkNodeClass( typeof( RimWorld.JobGiver_DoLovin ),
                                                           typeof( Fluffy_BirdsAndBees.JobGiver_DoLovin ) );
            }

            // insert HediffGiverSet
            Resources.Debug( "Inserting fertility old age hediffgivers" );
            foreach ( ThingDef humanoid in fleshHumanoids )
            {
                Resources.Debug( humanoid.defName );
                humanoid.race.hediffGiverSets.Add( Resources.fertilityHediffGiverSetDef );
            }

            // detour PawnUtility.FertileMateTarget to check fertility of the female partner.
            Resources.Debug( "Detouring PawnUtility.FertileMateTarget" );
            MethodInfo source = typeof( RimWorld.PawnUtility ).GetMethod( "FertileMateTarget", (BindingFlags) 60 );
            MethodInfo destination = typeof( _PawnUtility ).GetMethod( "FertileMateTarget", (BindingFlags) 60 );
            if ( Detours.TryDetourFromTo( source, destination ) )
                Resources.Debug( "success", 1 );
            else
                Resources.Debug( "failed", 1 );

            // detour PawnUtility.Mated to add fertility stat check for male + female
            Resources.Debug( "Detouring PawnUtility.Mated" );
            source = typeof( RimWorld.PawnUtility ).GetMethod( "Mated", (BindingFlags)60 );
            destination = typeof( _PawnUtility ).GetMethod( "Mated", (BindingFlags)60 );
            if ( Detours.TryDetourFromTo( source, destination ) )
                Resources.Debug( "success", 1 );
            else
                Resources.Debug( "failed", 1 );

            // detour HediffGiver.TryApply to check gender for HediffGiver_Birthday_Gender.
            Resources.Debug( "Detouring HediffGiver.TryApply" );
            source = typeof( Verse.HediffGiver ).GetMethod( "TryApply", (BindingFlags)60 );
            destination = typeof( _HediffGiver ).GetMethod( "TryApply", (BindingFlags)60 );
            if ( Detours.TryDetourFromTo( source, destination ) )
                Resources.Debug( "success", 1 );
            else
                Resources.Debug( "failed", 1 );

            // detour the FinishAction in the iterator block of JobDriver_Lovin.MakeNewToils
            Resources.Debug( "Detouring Finish action of JobDriver_Lovin.MakeNewToils" );
            source = _JobDriver_Lovin.iterator.GetMethod( _JobDriver_Lovin.FINISH_ACTION_NAME, (BindingFlags) 60 );
            destination = typeof( _JobDriver_Lovin ).GetMethod( "FinishAction", (BindingFlags) 60 );
            if ( Detours.TryDetourFromTo( source, destination ) )
                Resources.Debug( "success", 1 );
            else
                Resources.Debug( "failed", 1 );

            Log.Message( "The Birds & The Bees :: ready for lovin'" );

           // LogClass( typeof( JobDriver_Lovin ) );

            return true;
        }

        public void LogClass( Type type, int inset = 0 )
        {
            Resources.Debug( type.FullName, inset );
            Resources.Debug( "==================="  );
            foreach ( MemberInfo member in type.GetMembers( (BindingFlags)60) )
                Resources.Debug( member.Name, inset );
            foreach ( Type nestedType in type.GetNestedTypes((BindingFlags)60) )
                LogClass( nestedType, inset + 1 );
        }
    }
}

