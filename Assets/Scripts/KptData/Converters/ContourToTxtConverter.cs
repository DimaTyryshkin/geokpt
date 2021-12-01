using System;
using System.Collections.Generic;
using System.IO;
using Geo.KptData;
using UnityEngine.Assertions;

namespace Geo.KptData.Converters
{
	public class ContourToTxtConverter
	{  
		public static readonly string[] separators = new string[] {",", " ", ", "};
		public static readonly string[] decimals   = new string[] {".", ","};

		IContour contour;
		IParcel parcel;

		public ContourToTxtConverter(IContour contour, IParcel parcel)
		{
			Assert.IsNotNull(contour);
			Assert.IsNotNull(parcel);
			this.contour = contour;
			this.parcel = parcel;
		}

		public string ConvertToString(int separatorIndex, int decimalIndex)
		{
			string separator   = separators[separatorIndex];
			string decimalChar = decimals[decimalIndex];

			var points = contour.GetPoints();

			string resilt = "";
			for (int i = 0; i < points.Count; i++)
			{
				var    p = points[i];
				string x = ReplaceDecimal(p.x, decimalChar);
				string y = ReplaceDecimal(p.y, decimalChar);
				resilt += $"pt{i}{separator}{x}{separator}{y}" + Environment.NewLine;
			}

			return resilt;
		}

		public string ConvertToFile(string folderFullName, int separatorIndex, int decimalIndex)
		{
			string shotName = CadastralNumberToFileName(parcel.GetCadastralNumber()) ;
			string fullName = Path.Combine(folderFullName, shotName);

			//TODO проверить что папка не удалена
			File.WriteAllText(fullName, ConvertToString( separatorIndex, decimalIndex));
			return fullName;
		}

		string ReplaceDecimal(string origin, string newDecimal)
		{
			if (origin.Contains(","))
				return origin.Replace(",", newDecimal);
			
			if (origin.Contains("."))
				return origin.Replace(".", newDecimal);

			return origin;
		}
		
		string CadastralNumberToFileName(string number)
		{
			string name = "";
			foreach (var c in number)
			{
				if (char.IsLetterOrDigit(c))
					name += c;
				else
					name += "_";
			}


			return name + ".txt";
		}
	}
}