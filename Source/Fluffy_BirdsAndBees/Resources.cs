using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace Fluffy_BirdsAndBees
{
    public static class Resources
    {
        internal static HediffDef neuteredHediff = HediffDef.Named( "Neutered" );
        internal static BodyPartDef reproductiveOrganDef = DefDatabase<BodyPartDef>.GetNamed( "ReproductiveOrgans" );
        internal static RecipeDef neuterRecipeDef = DefDatabase<RecipeDef>.GetNamed( "Neuter" );
        internal static PawnCapacityDef reproductionCapacityDef = DefDatabase<PawnCapacityDef>.GetNamed( "Reproduction" );
        internal static HediffDef menopauseHediff = HediffDef.Named( "Menopause" );

        internal static HediffGiverSetDef fertilityHediffGiverSetDef =
            DefDatabase<HediffGiverSetDef>.GetNamed( "HumanoidFertility" );

        internal static BodyPartRecord reproductiveOrganRecord = new BodyPartRecord
                                                                 {
                                                                     coverage = .01f,
                                                                     depth = BodyPartDepth.Outside,
                                                                     height = BodyPartHeight.Bottom,
                                                                     def = reproductiveOrganDef,
                                                                     groups =
                                                                         new List<BodyPartGroupDef>
                                                                         {
                                                                             BodyPartGroupDefOf
                                                                                 .Torso
                                                                         }
                                                                 };

        public static ThoughtDef failedLovingThoughtDef_Male = ThoughtDef.Named( "FailedLovinMale" );
        public static ThoughtDef failedLovingThoughtDef_Female = ThoughtDef.Named( "FailedLovinFemale" );

        public static void Debug( string text, int indent = 0 )
        {
#if DEBUG
            string prefix = "";
            for ( int i = 0; i < indent; i++ )
            {
                prefix += "\t";
            }

            if ( indent == 0 )
                prefix += "BirdsAndBees :: ";
            Log.Message( prefix + text );
#endif
        }
    }
}