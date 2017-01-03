// Karel Kroeze
// JobGiver_DoLovin.cs
// 2017-01-03

using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;

namespace Fluffy_BirdsAndBees
{
    public class JobGiver_DoLovin : RimWorld.JobGiver_DoLovin
    {
        protected override Job TryGiveJob( Pawn pawn )
        {
            // add check for presence of reproductive organ.
            if ( pawn.health.hediffSet.PartIsMissing( Resources.reproductiveOrganRecord ) )
                return null;

            // vanilla
            if ( Find.TickManager.TicksGame < pawn.mindState.canLovinTick )
                return null;
            if ( pawn.CurJob == null || !pawn.jobs.curDriver.layingDown || pawn.jobs.curDriver.layingDownBed == null ||
                 pawn.jobs.curDriver.layingDownBed.Medical || !pawn.health.capacities.CanBeAwake )
                return null;

            Pawn partnerInMyBed = LovePartnerRelationUtility.GetPartnerInMyBed( pawn );
            if ( partnerInMyBed == null || !partnerInMyBed.health.capacities.CanBeAwake ||
                 Find.TickManager.TicksGame < partnerInMyBed.mindState.canLovinTick )
                return null;
            // end vanilla

            // add check for partners reproductive organs
            if ( partnerInMyBed.health.hediffSet.PartIsMissing( Resources.reproductiveOrganRecord ) )
                return null;

            // vanilla
            if ( !pawn.CanReserve( partnerInMyBed, 1 ) || !partnerInMyBed.CanReserve( pawn, 1 ) )
                return null;

            return new Job( JobDefOf.Lovin, partnerInMyBed, pawn.jobs.curDriver.layingDownBed );
            // end vanilla
            // TODO: Add impotence checks and moodlets to JobDriver_Lovin'
        }
    }
}
