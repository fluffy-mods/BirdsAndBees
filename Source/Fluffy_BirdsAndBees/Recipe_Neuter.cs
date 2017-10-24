using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;
using static Fluffy_BirdsAndBees.Resources;

namespace Fluffy_BirdsAndBees
{
    public class Recipe_Neuter : Recipe_Surgery
    {

        public override IEnumerable<BodyPartRecord> GetPartsToApplyOn( Pawn pawn, RecipeDef recipe )
        {
            Debug( "GetPartsToApplyOn" );
            if ( !pawn.health.hediffSet.HasHediff( HediffDefOf.Neutered ) && !pawn.health.hediffSet.PartIsMissing( pawn.ReproductiveOrgans() ))
                yield return pawn.ReproductiveOrgans();
        }

        // TODO: Verify working for A18 (added bill argument)
        public override void ApplyOnPawn( Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill )
        {
            Debug( "ApplyOnPawn" );
            if ( billDoer != null )
            {
                if ( CheckSurgeryFail( billDoer, pawn, ingredients, pawn.ReproductiveOrgans(), bill ) )
                    return;
                TaleRecorder.RecordTale( TaleDefOf.DidSurgery, billDoer, pawn );
            }
            pawn.health.AddHediff( recipe.addsHediff, part, null );

            GiveThoughtsForPawnNeutered( pawn );
        }

        public void GiveThoughtsForPawnNeutered( Pawn victim )
        {
            // animals are fine
            if ( !victim.RaceProps.Humanlike )
                return;

            int stage;
            
            // should we really be doing this?
            if ( victim.IsColonist )
                stage = 3;
            else if ( victim.IsPrisonerOfColony )
                stage = 1;
            else
                stage = 2;

            // guilty had it coming
            if (victim.guilt.IsGuilty)
                stage = 0;

            foreach (
                Pawn pawn in
                PawnsFinder.AllMapsCaravansAndTravelingTransportPods.Where( p => p.IsColonist || p.IsPrisonerOfColony )
            )
                pawn.needs.mood.thoughts.memories.TryGainMemory( ThoughtMaker.MakeThought( ThoughtDefOf.SomeoneNeutered, stage ) );
        }
    }
}
