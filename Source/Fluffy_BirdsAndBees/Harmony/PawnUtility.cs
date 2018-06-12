using Harmony;
using RimWorld;
using UnityEngine;
using Verse;
using static Fluffy_BirdsAndBees.Resources;

namespace Fluffy_BirdsAndBees
{
    [HarmonyPatch( typeof( PawnUtility ) )]
    [HarmonyPatch( "FertileMateTarget" )]
    public class PawnUtility_FertileMateTarget
    {
        static bool Prefix( ref bool __result, Pawn female )
        {
            if ( !female.health.capacities.CapableOf( PawnCapacityDefOf.Reproduction ) ) // add fertility check
            {
                Debug( $"{female.Name.ToStringShort} is NOT FERTILE"  );
                __result = false; // return false from PawnUtility.FertileMateTarget
                return false; // stop further execution
            }

            Debug($"{female.Name.ToStringShort} is FERTILE");
            return true; // let PawnUtility.FertileMateTarget() execute
        }
    }

    [HarmonyPatch(typeof(PawnUtility))]
    [HarmonyPatch("Mated")]
    public class PawnUtility_Mated
    {
        static bool Prefix( Pawn male, Pawn female )
        {
            // add fertility chance check (male.fertility * female.fertility).
            if ( male.health.capacities.GetLevel(PawnCapacityDefOf.Reproduction) *
                 female.health.capacities.GetLevel(PawnCapacityDefOf.Reproduction) < Rand.Value )
            {
                Debug( $"{male.Name.ToStringShort} and {female.Name.ToStringShort} FAILED fertility check" );
                return false;
            }

            Debug( $"{male.Name.ToStringShort} and {female.Name.ToStringShort} PASSED fertility check" );
            return true;
        }
    }
}