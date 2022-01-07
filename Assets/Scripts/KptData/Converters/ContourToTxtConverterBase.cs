using System;
using System.Collections.Generic;
using System.IO;

namespace Geo.KptData.Converters
{
	public abstract class ContourToTxtConverterBase
	{
		public static readonly string[] decimals = new string[] {".", ","};
		
		int decimalSeparatorIndex;

		protected ContourToTxtConverterBase(int decimalSeparatorIndex)
		{
			this.decimalSeparatorIndex = decimalSeparatorIndex;
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
			string      decimalSeparator = GetDecimalSeparatorSafe(decimalSeparatorIndex);
			List<Point> points           = contour.GetPoints();

			string resilt = "";
			for (int i = 0; i < points.Count; i++)
				resilt += PointToString(i, points[i],  decimalSeparator) + Environment.NewLine;

			return resilt;
		}

		public string PointToString(int i, Point point)
		{
			return PointToString(i, point, GetDecimalSeparatorSafe(decimalSeparatorIndex));
		}

		protected abstract string PointToString(int i, Point point, string decimalSeparator);


		public static string GetDecimalSeparatorSafe(int index)
		{
			if (index < 0 || index >= decimals.Length)
				return decimals[0];

			return decimals[index];
		}

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
	}
}