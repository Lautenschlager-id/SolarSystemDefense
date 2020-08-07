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
		public class GitHubJSONPut
		{
			[DataMember]
			public string message { get; set; }

			[DataMember]
			public string sha { get; set; }

			[DataMember]
			public string content { get; set; }
		}

		public static Dictionary<string, string> headers = new Dictionary<string, string>()
		{
			{ "User-Agent", "Game"},
			{ "Token", Encoding.UTF8.GetString(Convert.FromBase64String("ZDE2NDc0NDM1ZGNjODlmZGVlYTU1MGQ5OWI3ZTE0NmU1Mzk2NTIxMQ==")) }, // This is a shame
		};

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
	}
}
