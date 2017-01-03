using RimWorld;
using UnityEngine;
using Verse;

namespace Fluffy_BirdsAndBees
{
    public static class _PawnUtility
    {
        public static bool FertileMateTarget( Pawn male, Pawn female )
        {
            Resources.Debug( "_PawnUtility.FertileMateTarget(" + female.LabelShort + ")" );

            // return false if target is not female, reproductive or fertile
            if ( female.gender != Gender.Female 
                || !female.ageTracker.CurLifeStage.reproductive 
                || !female.health.capacities.CapableOf( Resources.reproductionCapacityDef ) ) // add fertility check
            {
                return false;
            }

            Resources.Debug( female.NameStringShort + " passed fertility check", 1 );

            CompEggLayer compEggLayer = female.TryGetComp<CompEggLayer>();
            if ( compEggLayer != null )
            {
                return !compEggLayer.FullyFertilized;
            }
            return !female.health.hediffSet.HasHediff( HediffDefOf.Pregnant );
        }

        public static void Mated( Pawn male, Pawn female )
        {
            if ( !female.ageTracker.CurLifeStage.reproductive )
            {
                return;
            }
            
            Resources.Debug( "PawnUtility_Mated( " + male.NameStringShort + ", " + female.NameStringShort + ")" );

            // add fertility chance check (male.fertility * female.fertility).
            if ( male.health.capacities.GetEfficiency( Resources.reproductionCapacityDef ) *
                 female.health.capacities.GetEfficiency( Resources.reproductionCapacityDef ) < Rand.Value )
                return;

            Resources.Debug( "passed fertility check", 1 );


            CompEggLayer compEggLayer = female.TryGetComp<CompEggLayer>();
            if ( compEggLayer != null )
            {
                compEggLayer.Fertilize( male );
            }
            else if ( Rand.Value < 0.5f && !female.health.hediffSet.HasHediff( HediffDefOf.Pregnant ) )
            {
                Hediff_Pregnant hediff_Pregnant = (Hediff_Pregnant)HediffMaker.MakeHediff( HediffDefOf.Pregnant, female, null );
                hediff_Pregnant.father = male;
                female.health.AddHediff( hediff_Pregnant, null, null );
            }
        }
    }
}