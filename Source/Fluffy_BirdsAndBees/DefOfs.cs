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
    }

    [DefOf]
    public class ThoughtDefOf
    {
        public static ThoughtDef FailedLovinMale;
        public static ThoughtDef FailedLovinFemale;
        public static ThoughtDef SomeoneNeutered;
        public static ThoughtDef Neutered;
    }

    [DefOf]
    public class RecipeDefOf
    {
        public static RecipeDef Neuter;
    }

    [DefOf]
    public class HediffGiverSetDefOf
    {
        public static HediffGiverSetDef HumanoidFertility;
    }
}