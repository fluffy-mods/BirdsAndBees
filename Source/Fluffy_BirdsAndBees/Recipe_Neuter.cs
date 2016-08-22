using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using static Fluffy_BirdsAndBees.Resources;

namespace Fluffy_BirdsAndBees
{
    public class Recipe_Neuter : Recipe_MedicalOperation
    {

        public override IEnumerable<BodyPartRecord> GetPartsToApplyOn( Pawn pawn, RecipeDef recipe )
        {
            Debug( "GetPartsToApplyOn" );
            if ( !pawn.health.hediffSet.HasHediff( Resources.neuteredHediff ) && !pawn.health.hediffSet.PartIsMissing( Resources.reproductiveOrganRecord ))
                yield return Resources.reproductiveOrganRecord;
        }

        public override void ApplyOnPawn( Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients )
        {
            Debug( "ApplyOnPawn" );
            if ( billDoer != null )
            {
                if ( base.CheckSurgeryFail( billDoer, pawn, ingredients ) )
                {
                    return;
                }
                TaleRecorder.RecordTale( TaleDefOf.DidSurgery, new object[]
                {
                    billDoer,
                    pawn
                } );
            }
            pawn.health.AddHediff( this.recipe.addsHediff, part, null );
        }
    }
}
