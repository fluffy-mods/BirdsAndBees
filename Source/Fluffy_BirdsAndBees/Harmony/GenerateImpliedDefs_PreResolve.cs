// Karel Kroeze
// GenerateImpliedDefs_PreResolve.cs
// 2017-05-09

using System.Collections.Generic;
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

            var humanoidRaces = fleshRaces.Where( td => td.race.Humanlike );

            // insert neuter and implant recipes
            Debug( "Inject recipe" );
            foreach ( ThingDef race in fleshRaces )
            {
                Debug( race.defName, 1 );
                if ( race.recipes.NullOrEmpty() )
                    race.recipes = new List<RecipeDef>();
                race.recipes.Add( RecipeDefOf.Neuter );
            }

            // insert reproductive parts
            Debug("Insert parts");
            foreach (BodyDef body in fleshBodies)
            {
                Debug(body.defName, 1);
                // insert body part
                body.corePart.parts.Add(New_ReproductiveOrgans);
            }

            // insert old-age hediffgivers
            Debug("Insert hediffgivers");
            foreach ( ThingDef race in humanoidRaces )
            {
                Debug(race.defName, 1);
                if ( race.race.hediffGiverSets.NullOrEmpty() )
                    race.race.hediffGiverSets = new List<HediffGiverSetDef>();
                race.race.hediffGiverSets.Add( HediffGiverSetDefOf.HumanoidFertility );

                // should we apply the recipe to animals as well?
                race.recipes.Add(RecipeDefOf.InstallBasicReproductiveOrgans);
                race.recipes.Add(RecipeDefOf.InstallBionicReproductiveOrgans);
            }
        }
    }
}