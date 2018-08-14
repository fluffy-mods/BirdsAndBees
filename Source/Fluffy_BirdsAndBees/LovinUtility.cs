// Karel Kroeze
// RandomNormal.cs
// 2017-05-10

using RimWorld;
using UnityEngine;
using Verse;

namespace Fluffy_BirdsAndBees
{
    public class LovinUtility
    {
        public static float RandomNormal( float mean = 0f, float stdDev = 1f )
        {
            float u1 = 1 - Random.value; 
            float u2 = 1 - Random.value;
            float randStdNormal = Mathf.Sqrt(-2 * Mathf.Log(u1)) * Mathf.Sin(2 * Mathf.PI * u2); //random normal(0,1)
            return mean + stdDev * randStdNormal; //random normal(mean,stdDev^2)
        }

        public static int LovinLevel( Pawn pawn, Pawn partner )
        {
            // get pawns fertility stat, manipulation stat and mutual attraction to generate a random lovin' experience
            var performance = pawn.health.capacities.GetLevel(PawnCapacityDefOf.Fertility) *
                              partner.health.capacities.GetLevel(PawnCapacityDefOf.Fertility) *
                              pawn.health.capacities.GetLevel(RimWorld.PawnCapacityDefOf.Manipulation) *
                              partner.health.capacities.GetLevel(RimWorld.PawnCapacityDefOf.Manipulation);

            // talking helps if positive, otherwise _shut up_.
            performance *= Mathf.Max(1f, partner.health.capacities.GetLevel(RimWorld.PawnCapacityDefOf.Talking)) *
                           Mathf.Max(1f, pawn.health.capacities.GetLevel(RimWorld.PawnCapacityDefOf.Talking));

            // opinion factors in too
            performance *= (pawn.relations.OpinionOf(partner) / 100f) + 1f; // opinion is on a -100 -- 100 scale, divide by 100, add 1 to get a 0, 2 scale

            // note; while I believe attraction should factor in heavily, the main proxy variables we have available are 
            // traits, age, skinColor and gender. These are largely the factors that went into generating the relationship 
            // in the first place, and - with the exception of traits - I don't feel they should have much effect on the success of lovin`
            if (pawn.health.capacities.CapableOf(RimWorld.PawnCapacityDefOf.Sight))
            {
                if (RelationsUtility.IsDisfigured(partner))
                    performance *= .8f;
                performance *= (partner.story.traits.DegreeOfTrait(TraitDefOf.Beauty) / 10f) + 1;
                // 2 = beautiful, 1 = pretty, 0 = inactive. / 10 + 1 gives 1 - 1.4 scale.
            }

            // we now have a number. Lets get another number with an N(2,.5) distribution, multiply with our number,
            // and floor it. That will give us a number from 0-3, corresponding to the stages of the thoughtdef.
            var random = RandomNormal(2, .5f);
            int performanceLevel = Mathf.FloorToInt(random * performance);
            performanceLevel = Mathf.Clamp(performanceLevel, 0, 3);

            return performanceLevel;
        }
    }
}