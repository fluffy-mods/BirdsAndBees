// Karel Kroeze
// ThoughtWorker_Neuter.cs
// 2017-05-10

using RimWorld;
using Verse;

namespace Fluffy_BirdsAndBees
{
    public class ThoughtWorker_Neutered : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal( Pawn p )
        {
            if ( !p.RaceProps.Humanlike )
                return ThoughtState.Inactive;

            if ( p.health.hediffSet.HasHediff( HediffDefOf.Neutered ) )
                return ThoughtState.ActiveAtStage( 0 );
            if ( p.health.hediffSet.PartIsMissing( p.ReproductiveOrgans() ) )
                return ThoughtState.ActiveAtStage( 1 );
            return ThoughtState.Inactive;
        }
    }
}