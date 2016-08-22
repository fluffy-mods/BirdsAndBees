﻿using System.Linq;
using System.Reflection;
using CommunityCoreLibrary;
using RimWorld;
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

            Resources.Debug( "Done" );
            Log.Message( "The Birds & The Bees :: ready for lovin'" );

            return true;
        }
    }
}
