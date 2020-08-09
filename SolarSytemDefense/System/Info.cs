using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SolarSystemDefense
{
	static class Info
	{
		[DataContract]
		public class MapData
		{
			[DataMember]
			public int Code = -1;

			[DataMember]
			public List<Vector2> Walkpoints = new List<Vector2>();
		}

		[DataContract]
		public class WebServerJSONPut
		{
			[DataMember]
			public string machine_name { get; set; }

			[DataMember]
			public string map_code { get; set; }

			[DataMember]
			public string map_content { get; set; }
		}

		public static Dictionary<string, Color[]> Colors = new Dictionary<string, Color[]>()
		{
			{ "Container", new Color(55, 55, 65).Collection() },
			{ "Button", new Color[] { new Color(50, 80, 130), new Color(54, 151, 168) } },
			{ "ButtonText", new Color[] { new Color(57, 205, 205), new Color(57, 205, 205) } },
			{ "InfoBox", new Color(78, 4, 99).Collection() },
			{ "GhostButton", new Color[] { new Color(240, 240, 240), Color.GhostWhite } },
			{ "GhostButtonText", Color.White.Collection() }
		};

		public static Dictionary<string, float> LayerDepth = new Dictionary<string, float>()
		{
			{ "Background", 0 },
			{ "Middleground", .5f },
			{ "Foreground", 1 },
		};

		public static Dictionary<string, string> DatabaseURL = new Dictionary<string, string>()
		{
			{ "WebServer", "https://mono-ssd-mapserver.000webhostapp.com/write.php" },
			{ "UserMaps", "https://raw.githubusercontent.com/SolarSystemDefense/gamedb/master/ExportedMaps.json" },
			{ "OfficialMaps", "https://raw.githubusercontent.com/SolarSystemDefense/gamedb/master/OfficialMaps.json" }
		};
	}
}
