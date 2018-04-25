using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SolarSystemDefense
{
    static class Info
    {
        [DataContract]
        public class StageTable
        {
            [DataMember]
            public List<Vector2> Walkpoints = new List<Vector2>();
        }

        public static Dictionary<string, Color[]> Colors = new Dictionary<string, Color[]>()
        {
            { "Container", new Color(55, 55, 65).Collection() },
            { "Button", new Color[] { new Color(50, 80, 130), new Color(54, 151, 168) } },
            { "ButtonText", new Color[] { new Color(57, 205, 205), new Color(44, 61, 99) } },
        };
    }
}
