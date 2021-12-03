﻿using System;

namespace Geo
{
	[Serializable]
	public class GeoConfig
	{
		public string[] defaultContourToTxtFormats;
            
		public GeoConfig()
		{
			defaultContourToTxtFormats = new[]
			{
				"{x}, {y}",
				"pt{i}, {x}, {y}",
				"pt{i}, {x}, {y}, 0",
				"{x}:::{y}",
				"pt{i} разделитель {{x}} разделитель {{y}}"
			};
		}
	}
}