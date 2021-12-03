using System;
using System.Collections.Generic;
using System.IO;
using Geo.KptData;
using UnityEngine.Assertions;

namespace Geo.KptData.Converters
{
	public class ContourToTxtConverter
	{  
		public static readonly string[] decimals   = new string[] {".", ","};


		public static string GetDecimalSeparatorSafe(int index)
		{
			if (index < 0 || index >= decimals.Length)
				return decimals[0];

			return decimals[index];
		}

		public ContourToTxtConverter()
		{
		}

		public string ConvertToFile(string folderFullName, IContour contour, IParcel parcel, int decimalIndex, string format)
		{
			string shotName = CadastralNumberToFileName(parcel.GetCadastralNumber());
			string fullName = Path.Combine(folderFullName, shotName);

			//TODO проверить что папка не удалена
			File.WriteAllText(fullName, ConvertToString(contour, parcel, decimalIndex, format));
			return fullName;
		}

		public string ConvertToString(IContour contour, IParcel parcel, int decimalIndex, string format)
		{
			Assert.IsFalse(string.IsNullOrWhiteSpace(format));

			string      decimalChar = GetDecimalSeparatorSafe(decimalIndex);
			List<Point> points      = contour.GetPoints();

			string resilt = "";
			for (int i = 0; i < points.Count; i++)
				resilt += PointToString(i, points[i], decimalChar, format) + Environment.NewLine;

			return resilt;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <param name="p"></param>
		/// <param name="decimalSeparator"></param>
		/// <param name="format"> Формат вывода, например: 'pt{i}, {x}, {y}, 0'</param>
		/// <returns></returns>
		public string PointToString(int index, Point p, string decimalSeparator, string format)
		{
			int    n = index + 1;
			string x = ReplaceDecimal(p.x, decimalSeparator);
			string y = ReplaceDecimal(p.y, decimalSeparator);

			string result = format;
			result = result.Replace("{i}", index.ToString());
			result = result.Replace("{n}", (n).ToString());
			result = result.Replace("{x}", x);
			result = result.Replace("{y}", y);

			return result;
		}

		static string ReplaceDecimal(string origin, string newDecimal)
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