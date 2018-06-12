// Karel Kroeze
// PawnCapacityWorker_Fertility.cs
// 2017-06-05

using System.Collections.Generic;
using Verse;

namespace Fluffy_BirdsAndBees
{
    public class PawnCapacityWorker_Fertility : PawnCapacityWorker
    {
        public override float CalculateCapacityLevel( HediffSet diffSet,
                                                      List<PawnCapacityUtility.CapacityImpactor> impactors = null )
        {
            return PawnCapacityUtility.CalculateTagEfficiency( 
                diffSet, 
                BodyPartTagDefOf.FertilitySource, 
                3.40282347E+38f, 
                default( FloatRange ),
                impactors );
        }

        public override bool CanHaveCapacity( BodyDef body )
        {
            return body.HasPartWithTag(BodyPartTagDefOf.FertilitySource);
        }
    }
}

