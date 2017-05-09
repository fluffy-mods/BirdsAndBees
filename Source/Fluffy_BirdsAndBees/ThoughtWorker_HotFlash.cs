using System;
using RimWorld;
using UnityEngine;
using Verse;
using static Fluffy_BirdsAndBees.Resources;

namespace Fluffy_BirdsAndBees
{
    public class ThoughtWorker_HotFlash : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal( Pawn p )
        {
            Debug( "ThoughtWorker_HotFlash.CurrentStateInternal(" + p.NameStringShort + ")"  );
            // only applies to females in the hormonal state of menopause
            if ( p.gender != Gender.Female
                 || p.health.hediffSet.GetFirstHediffOfDef( HediffDefOf.Menopause )?.CurStageIndex != 1 )
                return ThoughtState.Inactive;
            
            // lasts an hour, for on average an hour per day.
            // note that the following 'random' number stays static for the entire hour, to prevent too frequent on/off behaviour.
            int quasiRandomStaticNumber = p.RandSeedForHour( 5 ) % 24; 
            Debug( "valid, number:" + quasiRandomStaticNumber, 1 );

            if ( Math.Abs( quasiRandomStaticNumber ) == 5)
                return ThoughtState.ActiveDefault;

            // not currently experiencing hot flashes
            return ThoughtState.Inactive;
        }
    }
}
