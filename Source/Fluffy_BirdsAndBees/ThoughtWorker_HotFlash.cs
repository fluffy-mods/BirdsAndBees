using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse;

namespace Fluffy_BirdsAndBees
{
    public class ThoughtWorker_HotFlash : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal( Pawn p )
        {
            Resources.Debug( "ThoughtWorker_HotFlash.CurrentStateInternal(" + p.NameStringShort + ")"  );
            // only applies to females in the hormonal state of menopause
            if ( p.gender != Gender.Female
                 || !p.health.hediffSet.HasHediff( Resources.menopauseHediff )
                 || p.health.hediffSet.GetFirstHediffOfDef( Resources.menopauseHediff ).CurStageIndex != 1 )
                return ThoughtState.Inactive;
            
            // lasts an hour, for on average an hour per day.
            // note that the following 'random' number stays static for the entire hour, to prevent too frequent on/off behaviour.
            int quasiRandomStaticNumber = p.RandSeedForHour( 5 ) % 24; 
            Resources.Debug( "valid, number:" + quasiRandomStaticNumber, 1 );

            if ( Math.Abs( quasiRandomStaticNumber ) == 5)
                return ThoughtState.ActiveDefault;

            // not currently experiencing hot flashes
            return ThoughtState.Inactive;
        }
    }
}
