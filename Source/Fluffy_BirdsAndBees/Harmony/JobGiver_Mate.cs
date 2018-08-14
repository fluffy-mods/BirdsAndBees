// Karel Kroeze
// JobGiver_Mate.cs
// 2017-05-08

using Harmony;
using RimWorld;
using Verse;
using Verse.AI;
using static Fluffy_BirdsAndBees.Resources;

namespace Fluffy_BirdsAndBees
{
    [HarmonyPatch(typeof(JobGiver_Mate), "TryGiveJob")]
    public class JobGiver_Mate_TryGiveJob
    {
        static bool Prefix( Pawn pawn, ref Job __result )
        {
            // not fertile, won't mate.
            if ( !pawn.health.capacities.CapableOf( PawnCapacityDefOf.Fertility ) )
            {
                Debug( $"{pawn.LabelShort} is incapable of reproduction"  );
                __result = null;
                return false;
            }

            // let vanilla handle further checks.
            Debug($"{pawn.LabelShort} is capable of reproduction");
            return true;
        }
    }
}