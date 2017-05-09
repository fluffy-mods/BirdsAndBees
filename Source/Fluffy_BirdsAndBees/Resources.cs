using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Permissions;
using RimWorld;
using UnityEngine;
using Verse;

namespace Fluffy_BirdsAndBees
{
    public static class Resources
    {
        public static BodyPartRecord New_ReproductiveOrgans => new BodyPartRecord(){
                                                         coverage = 0.005f,
                                                         def = BodyPartDefOf.ReproductiveOrgans,
                                                         depth = BodyPartDepth.Outside,
                                                         groups = new List<BodyPartGroupDef>( new [] { BodyPartGroupDefOf.Torso } ),
                                                         height = BodyPartHeight.Middle
                                                     };

        public static BodyPartRecord ReproductiveOrgans( this Pawn pawn )
        {
            return pawn.RaceProps.body.ReproductiveOrgans();
        }

        public static BodyPartRecord ReproductiveOrgans( this BodyDef body )
        {
            return body.AllParts.FirstOrDefault( part => part.def == BodyPartDefOf.ReproductiveOrgans );
        }

        [Conditional("DEBUG")]
        public static void Debug( string text, int indent = 0 )
        {
            string prefix = "";
            for ( int i = 0; i < indent; i++ )
            {
                prefix += "\t";
            }

            if ( indent == 0 )
                prefix += "BirdsAndBees :: ";
            Log.Message( prefix + text );
        }
    }
}