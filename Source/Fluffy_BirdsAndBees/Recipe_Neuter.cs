using System.Collections.Generic;
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
            if ( !pawn.health.hediffSet.HasHediff( neuteredHediff ) && !pawn.health.hediffSet.PartIsMissing( reproductiveOrganRecord ))
                yield return reproductiveOrganRecord;
        }

        public override void ApplyOnPawn( Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients )
        {
            Debug( "ApplyOnPawn" );
            if ( billDoer != null )
            {
                if ( CheckSurgeryFail( billDoer, pawn, ingredients, reproductiveOrganRecord ) )
                    return;
                TaleRecorder.RecordTale( TaleDefOf.DidSurgery, billDoer, pawn );
            }
            pawn.health.AddHediff( recipe.addsHediff, part, null );
        }
    }
}
