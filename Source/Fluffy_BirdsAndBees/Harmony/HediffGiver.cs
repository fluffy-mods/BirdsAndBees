using System.Collections.Generic;
using Harmony;
using RimWorld;
using UnityEngine;
using Verse;

namespace Fluffy_BirdsAndBees
{
    [HarmonyPatch( typeof( HediffGiver ) )]
    [HarmonyPatch( "TryApply" )]
    public class HediffGiver_TryApply
    {
        static bool Prefix( HediffGiver __instance, ref bool __result, Pawn pawn )
        {
            // if the instance is of gendered type, check pawn gender before applying.
            Resources.Debug( "HediffGiver.TryApply(" + __instance.hediff.defName + ", " + pawn.NameStringShort + " [" +
                             pawn.gender + "])" );
            HediffGiver_Birthday_Gender gendered = __instance as HediffGiver_Birthday_Gender;
            if ( gendered != null && gendered.gender != pawn.gender )
            {
                __result = false; // return false from TryApply
                return false; // stop further execution
            }

            Resources.Debug( "OK", 1 );
            return true; // allow further execution of vanilla HediffGiver.TryApply()
        }
    }
}