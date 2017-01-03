using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace Fluffy_BirdsAndBees
{
    public static class _HediffGiver
    {
        public static bool TryApply( HediffGiver _this, Pawn pawn, List<Hediff> outAddedHediffs = null )
        {
            // if the instance is of gendered type, check pawn gender before applying.
            Resources.Debug( "HediffGiver.TryApply(" + _this.hediff.defName + ", " + pawn.NameStringShort + " [" + pawn.gender + "])" );
            HediffGiver_Birthday_Gender gendered = _this as HediffGiver_Birthday_Gender;
            if ( gendered != null && gendered.gender != pawn.gender )
                return false;
            Resources.Debug( "OK", 1 );

            return HediffGiveUtility.TryApply( pawn, _this.hediff, _this.partsToAffect, _this.canAffectAnyLivePart, _this.countToAffect, outAddedHediffs );
        }
    }
}