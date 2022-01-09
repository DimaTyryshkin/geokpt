using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Assertions;

namespace Geo.KptData.Converters
{ 
	public abstract class ContourToTxtConverterBase
	{ 
		string decimalSeparator;

		public string DecimalSeparator => decimalSeparator;

		protected ContourToTxtConverterBase(string decimalSeparator)
		{
			Assert.IsFalse(string.IsNullOrEmpty(decimalSeparator));
			this.decimalSeparator = decimalSeparator;
		}

		public string ConvertToFile(string folderFullName, IContour contour, IParcel parcel)
		{
			string shotName = CadastralNumberToFileName(parcel.GetCadastralNumber());
			string fullName = Path.Combine(folderFullName, shotName);

			//TODO проверить что папка не удалена
			File.WriteAllText(fullName, ConvertToString(contour, parcel));
			return fullName;
		}

		public string ConvertToString(IContour contour, IParcel parcel)
		{ 
			List<Point> points           = contour.GetPoints();

			string resilt = "";
			for (int i = 0; i < points.Count; i++)
				resilt += PointToString(i, points[i],  decimalSeparator) + Environment.NewLine;

			return resilt;
		}

		public string PointToString(int i, Point point)
		{
			return PointToString(i, point, decimalSeparator);
		}

		protected string PointToString(int i, Point point, string decimalSeparator)
		{
			string format = GetFormat();
			return PointToStringWithFormat(i, point, decimalSeparator, format);
		}

		public abstract string GetFormat();
		
		protected static string ReplaceDecimal(string origin, string newDecimal)
		{
			if (origin.Contains(","))
				return origin.Replace(",", newDecimal);
			
			if (origin.Contains("."))
				return origin.Replace(".", newDecimal);

			return origin;
		}
		
		protected static string CadastralNumberToFileName(string number)
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
		
		protected string PointToStringWithFormat(int index, Point p, string decimalSeparator, string format)
		{
			int    n = index + 1;
			string x = ReplaceDecimal(p.x, decimalSeparator);
			string y = ReplaceDecimal(p.y, decimalSeparator);

			string result = format;
			result = result.Replace("(i)", index.ToString());
			result = result.Replace("(n)", (n).ToString());
			result = result.Replace("(x)", x);
			result = result.Replace("(y)", y);

			return result;
		}
	}
}