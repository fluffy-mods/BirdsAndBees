using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fluffy_BirdsAndBees.Extensions;
using Harmony;
using HugsLib;
using HugsLib.Source.Detour;
using RimWorld;
using UnityEngine;
using Verse;
using static Fluffy_BirdsAndBees.Resources;

namespace Fluffy_BirdsAndBees
{
    public class Controller : ModBase
    {
        public override void DefsLoaded()
        {
            // prepare body part record
            // NOTE: This would normally be read from the body def.
            Debug( "Start injection" );

            Debug( "Find fleshies" );
            // find all fleshy races, bodytypes and thinktrees.
            var fleshRaces = DefDatabase<ThingDef>
                .AllDefsListForReading
                .Where( t => t.race?.IsFlesh ?? false );

            var fleshBodies = fleshRaces
                .Select( t => t.race.body )
                .Distinct();

            var fleshThinktrees = fleshRaces
                .Select( r => r.race.thinkTreeMain )
                .Distinct();

            // subset of humanoid fleshy races.
            var fleshHumanoids = fleshRaces
                .Where( r => r.race.Humanlike );

            // insert neuter recipe 
            Debug( "Insert recipes" );
            foreach ( ThingDef race in fleshRaces )
            {
                Debug( race.defName, 1 );
                if (race.recipes == null)
                    race.recipes = new List<RecipeDef>();
                race.recipes.Add( neuterRecipeDef );
            }

            // insert reproductive parts and recreate body cache
            Debug( "Insert parts" );
            foreach ( BodyDef body in fleshBodies )
            {
                Debug( body.defName, 1 );

                // insert body part
                body.corePart.parts.Add( reproductiveOrganRecord );
                Debug( $"Inserted part, available space: {body.corePart.fleshCoverage}", 2 );
                if ( body.corePart.fleshCoverage < reproductiveOrganRecord.coverage )
                    foreach ( BodyPartRecord part in body.corePart.parts )
                        Debug( $"{part.def.LabelCap} coverage {part.coverage}", 3 );

                // force recache
                // but first, remove the old cache (We don't really want octo-humans, do we?)
                Traverse.Create( body ).Field( "cachedAllParts" ).SetValue( new List<BodyPartRecord>() );
                body.ResolveReferences();
                Debug( "Resolves", 2 );
            }

            // replace thinktree references to JobGiver_Mate and JobGiver_Lovin
            Debug( "Replace JobDriver_Mate & JobDriver_Lovin" );
            foreach ( ThinkTreeDef thinktree in fleshThinktrees )
            {
                Debug( thinktree.defName, 1 );
                thinktree.thinkRoot.ReplaceThinkNodeClass( typeof( RimWorld.JobGiver_Mate ),
                                                           typeof( JobGiver_Mate ) );
                thinktree.thinkRoot.ReplaceThinkNodeClass( typeof( RimWorld.JobGiver_DoLovin ),
                                                           typeof( JobGiver_DoLovin ) );
            }

            // insert HediffGiverSet
            Debug( "Inserting fertility old age hediffgivers" );
            foreach ( ThingDef humanoid in fleshHumanoids )
            {
                Debug( humanoid.defName );
                if (humanoid.race.hediffGiverSets.NullOrEmpty())
                    humanoid.race.hediffGiverSets = new List<HediffGiverSetDef>();
                humanoid.race.hediffGiverSets.Add( fertilityHediffGiverSetDef );
            }

            // Harmony magic
            var harmony = HarmonyInstance.Create( "rimworld.fluffy.birdsandbees" );
            // patches;
            // HediffGiver.TryApply()
            // PawnUtility.FertileMateTarget()
            // PawnUtility.Mated()
            harmony.PatchAll( Assembly.GetExecutingAssembly() );
            
            // detour the FinishAction in the iterator block of JobDriver_Lovin.MakeNewToils
            // note that this is a horrible idea under all normal circumstances - but I didn't want to rewrite the toil.
            Debug( "Detouring Finish action of JobDriver_Lovin.MakeNewToils" );
            MethodInfo source = _JobDriver_Lovin.iterator.GetMethod( _JobDriver_Lovin.FINISH_ACTION_NAME, (BindingFlags) 60 );
            MethodInfo destination = typeof( _JobDriver_Lovin ).GetMethod( "FinishAction", (BindingFlags) 60 );
            if ( DetourProvider.TryCompatibleDetour( source, destination ) )
                Debug( "success", 1 );
            else
                Debug( "failed", 1 );

            Logger.Message( "ready for lovin'" );
        }

        #region Overrides of ModBase

        public override string ModIdentifier => "BirdsAndBees";

        #endregion
    }
}

