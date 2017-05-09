// Karel Kroeze
// GenerateImpliedDefs_PreResolve.cs
// 2017-05-09

using System.Linq;
using Harmony;
using RimWorld;
using Verse;
using static Fluffy_BirdsAndBees.Resources;

namespace Fluffy_BirdsAndBees
{
    [HarmonyPatch( typeof( DefGenerator ), "GenerateImpliedDefs_PreResolve" )]
    public class GenerateImpliedDefs_PreResolve
    {
        static void Postfix()
        {
            // prepare body part record
            // NOTE: This would normally be read from the body def.
            Debug("Start injection");
            Debug("Find fleshies");
            // find all fleshy races, bodytypes and thinktrees.
            var fleshRaces = DefDatabase<ThingDef>
                .AllDefsListForReading
                .Where(t => t.race?.IsFlesh ?? false); // return this.FleshType != FleshTypeDefOf.Mechanoid;

            var fleshBodies = fleshRaces
                .Select(t => t.race.body)
                .Distinct();

            // insert reproductive parts
            Debug("Insert parts");
            foreach (BodyDef body in fleshBodies)
            {
                Debug(body.defName, 1);
                // insert body part
                body.corePart.parts.Add(New_ReproductiveOrgans);
            }
        }
    }
}