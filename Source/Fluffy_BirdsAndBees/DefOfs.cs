// Karel Kroeze
// DefOfs.cs
// 2017-05-08

using RimWorld;
using Verse;

namespace Fluffy_BirdsAndBees
{
    [DefOf]
    public class BodyPartDefOf
    {
        public static BodyPartDef ReproductiveOrgans;
    }

    [DefOf]
    public class PawnCapacityDefOf
    {
        public static PawnCapacityDef Reproduction;
    }

    [DefOf]
    public class HediffDefOf
    {
        public static HediffDef Neutered;
        public static HediffDef Menopause;
        public static HediffDef Impotence;
    }

    [DefOf]
    public class ThoughtDefOf
    {
        public static ThoughtDef LovinPerformance;
        public static ThoughtDef SomeoneNeutered;
        public static ThoughtDef Neutered;
    }

    [DefOf]
    public class RecipeDefOf
    {
        public static RecipeDef Neuter;
        public static RecipeDef InstallBasicReproductiveOrgans;
        public static RecipeDef InstallBionicReproductiveOrgans;
    }

    [DefOf]
    public class HediffGiverSetDefOf
    {
        public static HediffGiverSetDef HumanoidFertility;
    }
}