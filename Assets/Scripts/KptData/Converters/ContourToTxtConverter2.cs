using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Geo.KptData.Converters
{
	public class ContourToTxtConverter2 : ContourToTxtConverterBase
	{
		string separator;
		string pointIndexFormat;
		bool   height;

		public ContourToTxtConverter2(int decimalSeparatorIndex, string separator, string pointIndexFormat, bool height) : base(decimalSeparatorIndex)
		{
			Assert.IsFalse(string.IsNullOrWhiteSpace(separator));

			this.separator        = separator;
			this.pointIndexFormat = pointIndexFormat;
			this.height           = height;
		}

		protected override string PointToString(int index, Point p, string decimalSeparator)
		{
			string x = ReplaceDecimal(p.x, decimalSeparator);
			string y = ReplaceDecimal(p.y, decimalSeparator);

			List<string> paths = new List<string>();

			if (!string.IsNullOrEmpty(pointIndexFormat))
				paths.Add(pointIndexFormat.Replace("(i)", index.ToString()));

			paths.Add(x);
			paths.Add(y);

			if (height)
				paths.Add("0");

			return string.Join(separator, paths);
		}
	}
}