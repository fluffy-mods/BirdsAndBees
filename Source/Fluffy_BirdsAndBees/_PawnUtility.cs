using RimWorld;
using UnityEngine;
using Verse;

namespace Fluffy_BirdsAndBees
{
    public static class _PawnUtility
    {
        // 99% vanilla
        public static bool FertileMateTarget( Pawn male, Pawn female )
        {
            Resources.Debug( "_PawnUtility.FertileMateTarget(" + female.LabelShort + ")" );

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
    }
}