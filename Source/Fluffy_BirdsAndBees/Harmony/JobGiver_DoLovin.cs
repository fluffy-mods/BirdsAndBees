// Karel Kroeze
// JobGiver_DoLovin.cs
// 2017-05-08

using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;
using static Fluffy_BirdsAndBees.Resources;

namespace Fluffy_BirdsAndBees
{
    [HarmonyPatch( typeof(JobGiver_DoLovin), "TryGiveJob" )]
    public class JobGiver_DoLovin_TryGiveJob
    {
        static bool Prefix(Pawn pawn, ref Job __result)
        {
            // doesn't have reproductive organs, don't give job.
            if ( pawn.health.hediffSet.PartIsMissing( pawn.ReproductiveOrgans() ) )
            {
                Debug( $"{pawn.LabelShort} (initiator) has no reproductive organs" );
                __result = null;
                return false;
            }

            // otherwise pass it on to vanilla method
            Debug($"{pawn.LabelShort} (initiator) has reproductive organs");
            return true;
        }

        static void Postfix( ref Job __result )
        {
            // if vanilla returned null, we're done.
            if ( __result == null )
            {
                Debug($"(initiator) rejected by vanilla");
                return;
            }

            // partner doesn't have reproductive organs, don't give job.
            Pawn partner = __result.targetA.Thing as Pawn;
            if ( partner.health.hediffSet.PartIsMissing( partner.ReproductiveOrgans() ) )
            {
                Debug($"{partner.LabelShort} (partner) has no reproductive organs");
                __result = null;
            }
            Debug($"{partner.LabelShort} (partner) has reproductive organs");
        }
    }
}