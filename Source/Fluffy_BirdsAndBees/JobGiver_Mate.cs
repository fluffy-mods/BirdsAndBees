using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;

namespace Fluffy_BirdsAndBees
{
    public class JobGiver_Mate : RimWorld.JobGiver_Mate
    {
        protected override Job TryGiveJob( Pawn pawn )
        {
            Resources.Debug( "JobGiver_Mate :: TryGiveJob("  + pawn.LabelShort + ")" );
            // add check for reproductive capability.
            if ( !pawn.health.capacities.CapableOf( Resources.reproductionCapacityDef ) )
                return null;
            
            Resources.Debug( pawn.LabelShort + " passed fertility check", 1 );
            
            // RimWorld.JobGiver_Mate does the rest of the normal checks
            // We're detouring PawnUtility.FertileMateTarget to limit the female partner.
            return base.TryGiveJob( pawn );
        }
    }
}