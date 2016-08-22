using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Fluffy_BirdsAndBees
{
    public class HediffGiver_Birthday_Gender : HediffGiver_Birthday
    {
        public bool female;

        // TODO: Look into injection point for overriding tryapply.
        // NOTE: the method probably cant/shouldn't reside in this class, since it's supposedly a member of HediffGiver.
        public new bool TryApply( Pawn pawn, List<Hediff> outAddedHediffs = null )
        {
            if ( female && pawn.gender != Gender.Female )
                return false;

            return HediffGiveUtility.TryApply( pawn, this.hediff, this.partsToAffect, this.canAffectAnyLivePart, this.countToAffect, outAddedHediffs );
        }
    }
}
