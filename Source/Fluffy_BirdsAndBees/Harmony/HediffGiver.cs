using HarmonyLib;
using Verse;
using static Fluffy_BirdsAndBees.Resources;

namespace Fluffy_BirdsAndBees
{
    [HarmonyPatch( typeof( HediffGiver ), nameof( HediffGiver.TryApply ) )]
    public class HediffGiver_TryApply
    {
        static bool Prefix( HediffGiver __instance, ref bool __result, Pawn pawn )
        {
            // if the instance is of gendered type, check pawn gender before applying.
            // NOTE: This debug call can give nullref errors in certain cases (what cases?). Disabled for now.
            // Debug( "HediffGiver.TryApply(" + __instance.hediff.defName + ", " + pawn.Name.ToStringShort + " [" +
            //         pawn.gender + "])" );
            HediffGiver_Birthday_Gender gendered = __instance as HediffGiver_Birthday_Gender;
            if ( gendered != null && gendered.gender != pawn.gender )
            {
                __result = false; // return false from TryApply
                return false; // stop further execution
            }

            Debug( "OK", 1 );
            return true; // allow further execution of vanilla HediffGiver.TryApply()
        }
    }
}